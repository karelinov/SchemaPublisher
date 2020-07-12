using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using EADiagramPublisher.Enums;

namespace EADiagramPublisher

{
    /// <summary>
    /// Класс для взаимодействий с EA
    /// Сюда помещаются методы, читающие информацию из EA или записывающие её
    /// </summary>
    class EAHelper
    {
        private static bool writelog = true;

        /// <summary>
        /// OUT в консоль спаркса
        /// </summary>        
        private static bool writeout = false;

        /// <summary>
        /// Shortcut для глобальной переменной EA.Repository
        /// </summary>
        private static EA.Repository EARepository
        {
            get
            {
                return Context.EARepository;
            }
        }

        /// <summary>
        /// Функция передвигает DA в указанную точку
        /// </summary>
        /// <param name="diagramObject"></param>
        /// <param name="newStart"></param>
        /// <param name="doUpdate"></param>
        public static void ApplyPointToDA(EA.DiagramObject diagramObject, Point newStart, bool doUpdate = true)
        {
            Size diagramObjectSize = DesignerHelper.GetSize(diagramObject);

            diagramObject.left = newStart.X;
            diagramObject.right = diagramObject.left + diagramObjectSize.Width;
            diagramObject.top = newStart.Y;
            diagramObject.bottom = diagramObject.top - diagramObjectSize.Height;
            if (doUpdate) diagramObject.Update();
        }

        /// <summary>
        /// Функция передвигает DA по указанному вектору (т.е. смещает на размеры точки)
        /// </summary>
        /// <param name="diagramObject"></param>
        /// <param name="newStart"></param>
        /// <param name="doUpdate"></param>
        public static void ApplyVectorToDA(EA.DiagramObject diagramObject, Point vector, bool doUpdate = true)
        {
            Size diagramObjectSize = DesignerHelper.GetSize(diagramObject);

            diagramObject.left += vector.X;
            diagramObject.right += vector.X;
            diagramObject.top += vector.Y;
            diagramObject.bottom += vector.Y;
            if (doUpdate) diagramObject.Update();
        }

        /// <summary>
        /// Функция применяет указанный размер к объекту DiagramObject
        /// </summary>
        /// <param name="diagramObject"></param>
        /// <param name="size"></param>
        /// <param name="doUpdate"></param>
        public static void ApplySizeToDA(EA.DiagramObject diagramObject, Size size, bool doUpdate = true)
        {
            diagramObject.right = diagramObject.left + size.Width;
            diagramObject.bottom = diagramObject.top - size.Height;
            if (doUpdate) diagramObject.Update();
        }

        /// <summary>
        /// Возвращает Дочернюю иерархию размещения объектов начиная от указанного
        /// </summary>
        /// <param name="eaElement"></param>
        /// <returns></returns>
        public static List<EA.Element> GetChildHierarchy(EA.Element eaElement)
        {
            List<EA.Element> result = new List<EA.Element>();

            // Добавляем к результату непосредственных детей
            List<EA.Element> children = GetChildrenDeploymentElement(eaElement);
            result.AddRange(children);

            // Проходимся по непосредственных детей и тянем из них рекурсией уже внуков
            foreach (EA.Element childElement in children)
            {
                List<EA.Element> grandСhildren = GetChildHierarchy(childElement);
                result.AddRange(grandСhildren);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список непосредственных deploy - детей элемента
        /// </summary>
        /// <param name="eaElement"></param>
        /// <returns></returns>
        public static List<EA.Element> GetChildrenDeploymentElement(EA.Element eaElement)
        {
            List<EA.Element> result = new List<EA.Element>();


            foreach (EA.Connector connector in eaElement.Connectors)
            {
                if (connector.Type == "Dependency" && connector.Stereotype == "deploy" && connector.SupplierID == eaElement.ElementID)
                {
                    result.Add(EARepository.GetElementByID(connector.ClientID));
                }
            }

            return result;
        }

        /// <summary>
        /// Фунция возвращает список непосредственных вставленных DA для указанного
        /// Вставленные это:
        /// - С границами внутри указанного
        /// - С Z-Order "выше" указанного
        /// - Которые не могут быть названными вставленными другими da с Z-Order "выше" указанного 
        /// 
        /// </summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static List<EA.DiagramObject> GetContainedForDA(EA.DiagramObject da)
        {
            List<EA.DiagramObject> result = new List<EA.DiagramObject>();

            // Сначала проходимся по элементам диаграммы и смотрим кто укладывается в рамки и Z-order
            foreach (EA.DiagramObject curDA in DPAddin.Designer.CurrentDiagram.DiagramObjects)
            {
                if (curDA.ElementID == da.ElementID) continue;
                // если такой элемент есть, проходимся по элементам даграммы ещё раз и смотрим, 
                // не может ли он принадлежать ещё кому-то с меньшим Z-order
                if (DesignerHelper.DAFitInside(curDA, da) && da.Sequence > curDA.Sequence)
                {
                    bool hasAnotherParent = false;

                    foreach (EA.DiagramObject curDA1 in DPAddin.Designer.CurrentDiagram.DiagramObjects)
                    {
                        if (curDA1.ElementID == da.ElementID) continue;
                        if (DesignerHelper.DAFitInside(curDA, curDA1) && curDA1.Sequence < da.Sequence)
                        {
                            hasAnotherParent = true;
                            break;
                        }

                    }
                    if (!hasAnotherParent)
                        result.Add(curDA);

                }
                else
                {
                    //EAHelper.Out("Не Укладывается в рамки и Zorder: ", new EA.DiagramObject[] { curDA });
                }
            }


            return result;
        }


        /// <summary>
        /// Фунция возвращает DA, в который непосредственно вставлен указанный DA
        /// Вставленные это:
        /// - С границами внутри указанного
        /// - С Z-Order "выше" указанного
        /// - Которые не могут быть названными вставленными другими da с Z-Order "выше" указанного 
        /// 
        /// </summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static EA.DiagramObject GetContainerOfDA(EA.DiagramObject da)
        {
            EA.DiagramObject result = null;

            // Сначала проходимся по элементам диаграммы и смотрим кто укладывается в рамки и Z-order

            foreach (EA.DiagramObject curDA in DPAddin.Designer.CurrentDiagram.DiagramObjects)
            {
                if (curDA.ElementID == da.ElementID) continue;

                if (DesignerHelper.DAFitInside(da, curDA) && da.Sequence < curDA.Sequence)
                {
                    if (result == null)
                    {
                        result = curDA;
                    }
                    else
                    {
                        if (result.Sequence < curDA.Sequence) // если уже кого то отобрали, то новый становится "папой" только если у него больше Zorder
                        {
                            result = curDA;
                        }
                    }
                }
            }


            return result;
        }

        /// <summary>
        /// Возвращает Список дочерних (deploy) объектов для указанного (дальнейшая иерархия не раскручивается)
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static List<EA.Element> GetDeployChildren(EA.Element element)
        {
            List<EA.Element> result = new List<EA.Element>();

            foreach (EA.Connector connector in element.Connectors)
            {
                if (connector.Type == "Dependency" && connector.Stereotype == "deploy" && connector.SupplierID == element.ElementID)
                {
                    result.Add(EARepository.GetElementByID(connector.ClientID));
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает Список всех дочерних (deploy, всё дерево) элементов диаграммы, присутствующих на диаграмме
        /// </summary>
        /// <param name="eaElement"></param>
        /// <returns></returns>
        public static List<EA.DiagramObject> GetDeployChildrenHierarchyDA(EA.Element eaElement)
        {
            List<EA.DiagramObject> result = new List<EA.DiagramObject>();

            // Получаем собственно дочерних
            List<EA.Element> childrenElements = GetChildHierarchy(eaElement);

            // для каждого проверяем, нет ли его на диаграмме
            foreach (EA.Element childElement in childrenElements)
            {
                EA.DiagramObject childElementDA = Context.CurrentDiagram.GetDiagramObjectByID(childElement.ElementID, "");
                if (childElementDA != null) // если есть на диаграмме - добавляем его в результат
                {
                    result.Add(childElementDA);
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает Список элементов диаграммы, являющихся для него родительскими
        /// </summary>
        /// <param name="element"></param>
        /// <returns>Отсортированный в сооответствии с род. иерархией список DiagramObjects</returns>
        public static List<EA.DiagramObject> GetDeployParentHierarchyDA(EA.Element element)
        {
            List<EA.DiagramObject> result = new List<EA.DiagramObject>();

            // Получаем список родительской иерархии
            List<EA.Element> parentHierarchy = GetParentHierarchy(element);

            foreach (EA.Element curParent in parentHierarchy)
            {
                EA.DiagramObject curParentDA = DPAddin.Designer.CurrentDiagram.GetDiagramObjectByID(curParent.ElementID, "");
                if (curParentDA != null)
                    result.Add(curParentDA);
            }

            return result;
        }

        /*
        /// <summary>
        /// Возвращает DiagramObject для объекта EA.Element если он есть на диаграмме
        /// </summary>
        public static EA.DiagramObject GetDiagramObject(EA.Element eaElement)
        {
            EA.DiagramObject result = null;

            if (DPAddin.Designer.CurrentDiagram != null)
            {
                foreach (EA.DiagramObject curDiagramObject in Context.CurrentDiagram.DiagramObjects)
                {
                    if (curDiagramObject.ElementID == eaElement.ElementID)
                    {
                        result = curDiagramObject;
                        break;
                    }
                }
            }
            return result;
        }
        */

        /// <summary>
        /// Функция возвращает размер элемента, соответствующий размеру элемента на библиотечной диаграмме
        /// </summary>
        /// <returns></returns>
        public static ExecResult<Size> GetElementSizeOnLibDiagram(EA.Element element)
        {
            ExecResult<Size> result = new ExecResult<Size>() { code = -1 };

            try
            {
                // лезем от элемента вверх по дереву пакетов, пока не достигнем верха либо не достигнем пакета без тэга DP_Library после нахождения такого тэга
                EA.Package curPackage = EARepository.GetPackageByID(element.PackageID);
                bool foundPackageAfterDPLibrary = false;
                bool foundDPLibraryPackage = false;

                while (curPackage != null & !(foundPackageAfterDPLibrary))
                {
                    // Проходимся по диаграммам пакета
                    foreach (EA.Diagram curDiagram in curPackage.Diagrams)
                    {
                        // ... В в диаграмме - по объектам 
                        foreach (EA.DiagramObject diagramObject in curDiagram.DiagramObjects)
                        {
                            // если объект на диаграмме - наш объект, то срисовываем его размеры как дефолтные
                            if (diagramObject.ElementID == element.ElementID)
                            {
                                Size curSize = DesignerHelper.GetSize(diagramObject);
                                result.value = curSize;
                                result.code = 0;
                                return result;
                            }
                        }

                        if (curPackage.ParentID != 0)
                            curPackage = EARepository.GetPackageByID(curPackage.ParentID);
                        else
                            curPackage = null;
                        if (IsLibrary(curPackage.Element))
                            foundDPLibraryPackage = true;
                        if (!IsLibrary(curPackage.Element) && foundDPLibraryPackage)
                            foundPackageAfterDPLibrary = true;
                    }
                }

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        /// <summary>
        /// Возвращает Список первых из имеющейся иерархии дочерних объектов, присутствующих на диаграмме
        /// </summary>
        /// <param name="eaElement"></param>
        /// <returns></returns>
        public static List<EA.DiagramObject> GetNearestChildrenDA(EA.Element eaElement)
        {
            List<EA.DiagramObject> result = new List<EA.DiagramObject>();

            // Получаем собственно дочерних
            List<EA.Element> childrenElements = GetDeployChildren(eaElement);
            // для каждого проверяем, нет ли его на диаграмме
            foreach (EA.Element childElement in childrenElements)
            {
                // Проверяем, что элемент на диаграмме
                EA.DiagramObject childElementDA = Context.CurrentDiagram.GetDiagramObjectByID(childElement.ElementID, "");

                if (childElementDA != null) // ... и если есть - добавляем в результат
                {
                    result.Add(childElementDA);
                }
                else // а если нет - надо проверить дочерних для эьтих дочерних - вдруг они на диаграмме
                {
                    List<EA.DiagramObject> grandchildrenDA = GetNearestChildrenDA(childElement);
                    result.AddRange(grandchildrenDA);
                }
                
            }

            return result;
        }

        /// <summary>
        /// Возвращает если есть родительский элемент размещения
        /// </summary>
        public static EA.Element GetParentDeploymentElement(EA.Element eaElement)
        {
            EA.Element result = null;


            foreach (EA.Connector connector in eaElement.Connectors)
            {
                if (connector.Type == "Dependency" && connector.Stereotype == "deploy" && connector.ClientID == eaElement.ElementID)
                {
                    result = EARepository.GetElementByID(connector.SupplierID);
                    break;
                }
            }


            return result;
        }

        /// <summary>
        /// Возвращает Родительскую иерархию размещения объектов начиная от указанного
        /// </summary>
        /// <param name="eaElement"></param>
        /// <returns></returns>
        public static List<EA.Element> GetParentHierarchy(EA.Element eaElement)
        {
            List<EA.Element> result = new List<EA.Element>();

            EA.Element parentEAElement = eaElement;
            do
            {
                parentEAElement = GetParentDeploymentElement(parentEAElement);
                if (parentEAElement != null) result.Add(parentEAElement);

            } while (parentEAElement != null);

            return result;
        }

        public static EA.Collection GetTaggedValues(EA.Element eaElement)
        {
            // для обычных элементов возвращаем их TaggedValuesEx
            if (eaElement.Type != "Boundary")
                return eaElement.TaggedValuesEx;

            // Рамочек ищем связанный с рамочкой объект DeploymentSpecification и возвращаем его TaggedValuesEx
            EA.Collection result = null;
            foreach (EA.Connector connector in eaElement.Connectors)
            {
                if (connector.Type == "Dependency" && connector.Stereotype == "")
                {
                    EA.Element dependedElement = EARepository.GetElementByID(connector.ClientID);
                    if (dependedElement.Type == "DeploymentSpecification")
                    {
                        return dependedElement.TaggedValuesEx;
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// Проверяет, что элемент является библиотечным. Если это инстанс, проверяет также класс
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsLibrary(EA.Element element)
        {
            bool result = false;

            if (GetTaggedValues(element).GetByName(DAConst.DP_LibraryTag) != null)
            {
                result = true;
            }
            else
            {
                if (element.ObjectType == EA.ObjectType.otElement)
                {
                    EA.Element classifier = EARepository.GetElementByID(element.ClassifierID);
                    if (GetTaggedValues(classifier).GetByName(DAConst.DP_LibraryTag) != null)
                    {
                        result = true;
                    }
                }

            }
            return result;
        }


        /// <summary>
        /// Подвигает DA и вписанные в него DA на указанный вектор
        /// </summary>
        /// <param name="diagramObject"></param>
        /// <param name="vector"></param>
        public static void MoveContainedHierarchy(EA.DiagramObject diagramObject, Point vector)
        {
            // Сначала получаем и подывигаем детей
            List<EA.DiagramObject> containedDAList = GetContainedForDA(diagramObject);
            foreach(var curContainedDA in containedDAList)
            {
                MoveContainedHierarchy(curContainedDA, vector);
            }

            // потом сам объект
            ApplyVectorToDA(diagramObject, vector);
        }


        /// <summary>
        /// Выводит в лог /+ консоль сообщение+список объектов
        /// </summary>
        /// <param name="outStr"></param>
        /// <param name="objectsToOUT"></param>
        /// <param name="tabname"></param>
        /// <param name="addCallername"></param>
        public static void Out(string outStr, Object[] objectsToOUT = null, string tabname = "System", bool addCallername = true)
        {
            string result = "";
            string resultLevel = new StringBuilder(DesignerHelper.CallLevel).Insert(0, " ", DesignerHelper.CallLevel).ToString();

            if (writeout || writelog)
            {
                if (objectsToOUT != null)
                    result += result + DumpObjects(objectsToOUT);

                if (addCallername)
                    result = new StackFrame(1, true).GetMethod().Name + ":" + outStr + " " + result;
            }

            result = resultLevel + result;

            if (writeout)
            {
                EARepository.WriteOutput("System", resultLevel + result, 0);
            }

            if (writelog)
                using (StreamWriter outputFile = new StreamWriter(DPAddin.logpath, true))
                    outputFile.WriteLine(result);
        }

        /// <summary>
        /// Выводит во все возможные файлы, лог /+ консоль сообщение+список объектов
        /// </summary>
        /// <param name="outStr"></param>
        /// <param name="objectsToOUT"></param>
        /// <param name="tabname"></param>
        /// <param name="addCallername"></param>
        public static void OutA(string outStr, Object[] objectsToOUT = null, string tabname = "System", bool addCallername = true)
        {

            bool oldwritelog = writelog;
            bool oldwriteout = writeout;

            writelog = true;
            writeout = true;

            Out(outStr, objectsToOUT, tabname, addCallername);

            writelog = oldwritelog;
            writeout = oldwriteout;

        }


        public static void SetConnectorVisibility(EA.DiagramLink diagramLink, bool visibility)
        {
            diagramLink.IsHidden = !visibility;
            diagramLink.Update();
        }

        /// <summary>
        /// Функция устанавливает размер элемента по умолчанию, соответствующий размеру элемента на библиотечной диаграмме
        /// </summary>
        /// <returns></returns>
        public static ExecResult<Boolean> SetElementDefaultSize()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();

            try
            {
                var obj = EARepository.GetContextObject();
                if (obj == null)
                {
                    throw new Exception("Нет текущего объекта");
                }
                if (!(obj is EA.Element) || !IsLibrary((EA.Element)obj))
                {
                    throw new Exception("Выделен не библиотечный элемент");
                }
                EA.Element curElement = (EA.Element)obj;

                // Ищем размер на библиотечных диаграммах
                ExecResult<Size> GetElementSizeOnLibDiagramResult = GetElementSizeOnLibDiagram(curElement);
                if (GetElementSizeOnLibDiagramResult.code != 0) throw new Exception(GetElementSizeOnLibDiagramResult.message);

                SetTaggedValue(curElement, DAConst.defaultWidthTag, GetElementSizeOnLibDiagramResult.value.Width.ToString());
                SetTaggedValue(curElement, DAConst.defaultHeightTag, GetElementSizeOnLibDiagramResult.value.Height.ToString());

                Out("Найден элемент диаграммы для установки размеров " + GetElementSizeOnLibDiagramResult.value.Width.ToString() + "x" + GetElementSizeOnLibDiagramResult.value.Height.ToString());

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        /// <summary>
        /// Преобразует объекты в строку. Для объектов EA визуализирует значения важных свойств
        /// </summary>        
        private static string DumpObjects(object[] objectsToOUT)
        {
            if (objectsToOUT == null || objectsToOUT.Length == 0)
            {
                return "null";
            }

            string result;
            if (objectsToOUT.Length == 1)
                result = ": ";
            else
                result = "(" + objectsToOUT.Length.ToString() + "):";

            if ((objectsToOUT as EA.Element[]) != null)
            {
                foreach (EA.Element curElement in (objectsToOUT as EA.Element[]))
                {
                    if (curElement == null)
                    {
                        result += "NULL;";
                    }
                    else
                    {
                        if (curElement.ClassfierID != 0)
                        {
                            result += "instance(" + curElement.Type;
                            EA.Element classifier = EARepository.GetElementByID(curElement.ClassfierID);
                            result += "," + classifier.Name + "," + classifier.Stereotype + ")";
                        }
                        else
                        {
                            result += "classifier(" + curElement.Type;
                            result += "," + curElement.Name + "," + curElement.Stereotype + ")";
                        }
                        result += ";";
                    }
                }
            }
            else if ((objectsToOUT as EA.DiagramObject[]) != null)
            {
                foreach (EA.DiagramObject curDA in (objectsToOUT as EA.DiagramObject[]))
                {
                    if (curDA == null)
                    {
                        result += "NULL;";
                    }
                    else
                    {
                        result += "da(" + (curDA.right - curDA.left).ToString() + "x" + Math.Abs(curDA.top - curDA.bottom).ToString() + ")(" + curDA.left.ToString() + "," + curDA.right.ToString() + "," + curDA.top.ToString() + "," + curDA.bottom.ToString() + ")";
                        EA.Element curElement = EARepository.GetElementByID(curDA.ElementID);
                        if (curElement.ClassfierID != 0)
                        {
                            result += "instance(" + curElement.Type;
                            EA.Element classifier = EARepository.GetElementByID(curElement.ClassfierID);
                            result += "," + curElement.Name + "," + classifier.Name + "," + classifier.Stereotype + ")";
                        }
                        else
                        {
                            result += "classifier(" + curElement.Type;
                            result += "," + curElement.Name + "," + curElement.Stereotype + ")";
                        }

                        result += ";";
                    }
                }
            }
            else if ((objectsToOUT[0].GetType() == typeof(Point)))
            {
                foreach (Point point in objectsToOUT)
                {
                    result += "(X=" + point.X.ToString() + ", Y=" + point.Y.ToString() + ");";
                }

            }
            else if ((objectsToOUT[0].GetType() == typeof(Size)))
            {
                foreach (Size size in objectsToOUT)
                {
                    result += "(Width=" + size.Width.ToString() + ", Height=" + size.Height.ToString() + ");";
                }

            }
            else
            {
                foreach (var obj in (objectsToOUT))
                {
                    result += obj.ToString() + ";";
                }
            }

            return result;
        }
        private static void SetTaggedValue(EA.Element element, string tagName, string tagValue)
        {
            EA.TaggedValue tag = element.TaggedValues.GetByName(tagName);
            if (tag == null)
            {
                tag = element.TaggedValues.AddNew(tagName, tagValue);
            }
            else
            {
                tag.Value = tagValue;
            }

            tag.Update();
            //element.Update(); // is it needed?

        }
    }
}
