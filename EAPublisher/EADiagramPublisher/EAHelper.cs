using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;

using System.Windows.Forms;
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
        /// Возвращает Дочернюю иерархию размещения объектов начиная от указанного
        /// </summary>
        /// <param name="eaElement"></param>
        /// <returns></returns>
        public static List<EA.Element> GetChildHierarchy(EA.Element eaElement)
        {
            List<EA.Element> result = new List<EA.Element>();

            // Добавляем к результату непосредственных детей
            List<EA.Element> children = GetDeployChildren(eaElement);
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
        /// Ищет линк коннектора на текущей диаграмме
        /// </summary>
        /// <param name="connector"></param>
        /// <returns></returns>
        public static EA.DiagramLink GetConnectorLink(EA.Connector connector)
        {
            EA.DiagramLink result = null;

            foreach (EA.DiagramLink diagramLink in Context.CurrentDiagram.DiagramLinks)
            {
                if (diagramLink.ConnectorID == connector.ConnectorID)
                {
                    result = diagramLink;
                    break;
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
                if (IsDeploymentLink(connector) && connector.SupplierID == element.ElementID)
                {
                    result.Add(EARepository.GetElementByID(connector.ClientID));
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает дерево дочерних (deploy) элементов для указанного элемента
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static DPTreeNode<EA.Element> GetDeployChildrenHierarchy(EA.Element element)
        {
            // Создаём корень
            DPTreeNode<EA.Element> result = new DPTreeNode<EA.Element>(element);

            // Получаем непосредственно размещённые элементы
            List<EA.Element> children = GetDeployChildren(element);

            // Проходимся по вложениям
            foreach (EA.Element child in children)
            {
                // получаем дочернее дерево и добавляем его к корневому узлу
                DPTreeNode<EA.Element> childTree = GetDeployChildrenHierarchy(child);
                result.AddChildNode(childTree);
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
        /// Возвращает если есть родительский элемент размещения
        /// </summary>
        public static EA.Element GetDeployParent(EA.Element eaElement)
        {
            EA.Element result = null;


            foreach (EA.Connector connector in eaElement.Connectors)
            {
                if (IsDeploymentLink(connector) && connector.ClientID == eaElement.ElementID)
                {
                    result = EARepository.GetElementByID(connector.SupplierID);
                    break;
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
                        if (LibraryHelper.IsLibrary(curPackage.Element))
                            foundDPLibraryPackage = true;
                        if (!LibraryHelper.IsLibrary(curPackage.Element) && foundDPLibraryPackage)
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
                parentEAElement = GetDeployParent(parentEAElement);
                if (parentEAElement != null) result.Add(parentEAElement);

            } while (parentEAElement != null);

            return result;
        }

        public static EA.Collection GetTaggedValues(EA.Element eaElement)
        {
            // Для обычных элементов возвращаем их TaggedValuesEx
            if (eaElement.Type != "Boundary")
                return eaElement.TaggedValuesEx;

            // Для рамочек ищем связанный с рамочкой объект DeploymentSpecification и возвращаем его TaggedValuesEx
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
        /// Возвращает в виде строки значение запрошенного TaggedValue из объекта
        /// Если такого TaggedValue нет - возвращается пустая строка
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="taggedValueName"></param>
        /// <returns></returns>
        public static string GetTaggedValue(EA.Connector connector, string taggedValueName)
        {
            EA.ConnectorTag taggedValue = null;
            try
            {
                taggedValue = connector.TaggedValues.GetByName(taggedValueName);
            }
            catch { };

            if (taggedValue == null)
                return "";
            else
                return taggedValue.Value;
        }
        public static string GetTaggedValue(EA.Element element, string taggedValueName, bool useEx = true)
        {
            EA.Collection elementTags;
            if (useEx)
                elementTags = element.TaggedValuesEx;
            else
                elementTags = element.TaggedValues;

            EA.TaggedValue taggedValue = elementTags.GetByName(taggedValueName);
            if (taggedValue == null)
                return "";
            else
                return taggedValue.Value;
        }


        /// <summary>
        /// Проверяет, что child размещён в parent
        /// т.е. есть библиотечный линк, направленный от child к parent , тип линка = Deploy
        /// </summary>
        /// <param name="childElement"></param>
        /// <param name="parentElement"></param>
        /// <returns></returns>
        public static bool IsDeployed(EA.Element childElement, EA.Element parentElement)
        {
            bool result = false;

            foreach (EA.Connector connector in childElement.Connectors)
            {
                if (IsDeploymentLink(connector) && connector.ClientID == childElement.ElementID)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Проверяет, что указанный линк - Deploy линк 
        /// </summary>
        /// <param name="connector"></param>
        /// <returns></returns>
        public static bool IsDeploymentLink(EA.Connector connector)
        {
            if (LibraryHelper.IsLibrary(connector) && connector.TaggedValues.GetByName(DAConst.DP_LinkTypeTag) != null && ((EA.ConnectorTag)connector.TaggedValues.GetByName(DAConst.DP_LinkTypeTag)).Value == LinkType.Deploy.ToString())
                return true;
            else
                return false;
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
            foreach (var curContainedDA in containedDAList)
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
            string result = DateTime.Now.ToString("HH:mm:sss.fff ");
            string resultLevel = new StringBuilder(DesignerHelper.CallLevel).Insert(0, " ", DesignerHelper.CallLevel).ToString();

            if (writeout || writelog)
            {
                if (objectsToOUT != null)
                    result += DumpObjects(objectsToOUT);

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


        public static void SetDiagramLinkVisibility(EA.DiagramLink diagramLink, bool visibility)
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
                if (!(obj is EA.Element) || !LibraryHelper.IsLibrary((EA.Element)obj))
                {
                    throw new Exception("Выделен не библиотечный элемент");
                }
                EA.Element curElement = (EA.Element)obj;

                // Ищем размер на библиотечных диаграммах
                ExecResult<Size> GetElementSizeOnLibDiagramResult = GetElementSizeOnLibDiagram(curElement);
                if (GetElementSizeOnLibDiagramResult.code != 0) throw new Exception(GetElementSizeOnLibDiagramResult.message);

                TaggedValueSet(curElement, DAConst.defaultWidthTag, GetElementSizeOnLibDiagramResult.value.Width.ToString());
                TaggedValueSet(curElement, DAConst.defaultHeightTag, GetElementSizeOnLibDiagramResult.value.Height.ToString());

                Out("Найден элемент диаграммы для установки размеров " + GetElementSizeOnLibDiagramResult.value.Width.ToString() + "x" + GetElementSizeOnLibDiagramResult.value.Height.ToString());

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        /// <summary>
        /// Функция записывает в выделенные элементы признак библиотечного
        /// </summary>
        /// <returns></returns>
        public static ExecResult<Boolean> SetDPLibratyTag(string location)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();

            try
            {
                switch (location)
                {
                    case "Diagram":
                        if (Context.CurrentDiagram != null)
                        {
                            if (Context.CurrentDiagram.SelectedObjects.Count > 0)
                            {
                                foreach (EA.DiagramObject curDA in Context.CurrentDiagram.SelectedObjects)
                                {
                                    EA.Element curElement = EARepository.GetElementByID(curDA.ElementID);
                                    TaggedValueSet(curElement, DAConst.DP_LibraryTag, "");
                                }
                            }
                            else if (Context.CurrentDiagram.SelectedConnector != null)
                            {
                                TaggedValueSet(Context.CurrentDiagram.SelectedConnector, DAConst.DP_LibraryTag, "");
                            }
                        }
                        break;
                    case "TreeView":
                        foreach (EA.Element curElement in EARepository.GetTreeSelectedElements())
                        {
                            TaggedValueSet(curElement, DAConst.DP_LibraryTag, "");
                        }
                        break;

                    case "MainMenu":
                        if (Context.CurrentDiagram != null)
                        {
                            if (Context.CurrentDiagram.SelectedObjects.Count > 0)
                            {

                                foreach (EA.DiagramObject curDA in Context.CurrentDiagram.SelectedObjects)
                                {
                                    EA.Element curElement = EARepository.GetElementByID(curDA.ElementID);
                                    TaggedValueSet(curElement, DAConst.DP_LibraryTag, "");
                                }
                            }
                            else if (Context.CurrentDiagram.SelectedConnector != null)
                            {
                                TaggedValueSet(Context.CurrentDiagram.SelectedConnector, DAConst.DP_LibraryTag, "");
                            }
                        }
                        else
                        {
                            foreach (EA.Element curElement in EARepository.GetTreeSelectedElements())
                            {
                                TaggedValueSet(curElement, DAConst.DP_LibraryTag, "");
                            }
                            break;
                        }



                        break;
                }

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }


        public static void TaggedValueSet(EA.Element element, string tagName, string tagValue)
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

        public static void TaggedValueSet(EA.Connector connector, string tagName, string tagValue)
        {
            EA.ConnectorTag tag = null;
            try
            {
                tag = connector.TaggedValues.GetByName(tagName);
            }
            catch (Exception ex)
            {
                if (ex.Message != "Index out of bounds") throw ex;
            }

            if (tag == null)
                tag = connector.TaggedValues.AddNew(tagName, tagValue);

            tag.Value = tagValue;
            bool res = tag.Update();
            //connector.Update(); // is it needed?

        }

        /// <summary>
        /// Удаляет TaggedValue из элемента
        /// </summary>
        /// <param name="element"></param>
        /// <param name="tagName"></param>
        public static void TaggedValueRemove(EA.Element element, string tagName)
        {
            for (short i = 0; i < element.TaggedValues.Count; i++)
            {
                EA.TaggedValue tag = element.TaggedValues.GetAt(i);
                if (tag.Name == tagName)
                {
                    element.TaggedValues.DeleteAt(i, true);
                }
            }
        }
        /// <summary>
        /// Удаляет TaggedValue из элемента
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="tagName"></param>
        public static void TaggedValueRemove(EA.Connector connector, string tagName)
        {
            for (short i = 0; i < connector.TaggedValues.Count; i++)
            {
                EA.ConnectorTag tag = connector.TaggedValues.GetAt(i);
                if (tag.Name == tagName)
                {
                    connector.TaggedValues.DeleteAt(i, true);
                }
            }
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
                    result += DumpObject(curElement) + ";";
            }
            else if ((objectsToOUT as EA.DiagramObject[]) != null)
            {
                foreach (EA.DiagramObject curDA in (objectsToOUT as EA.DiagramObject[]))
                    result += DumpObject(curDA) + ";";
            }
            else if ((objectsToOUT as EA.Package[]) != null)
            {
                foreach (EA.Package curPackage in (objectsToOUT as EA.Package[]))
                    result += DumpObject(curPackage) + ";";
            }
            else if ((objectsToOUT as EA.Connector[]) != null)
            {
                foreach (EA.Connector curConnector in (objectsToOUT as EA.Connector[]))
                    result += DumpObject(curConnector) + ";";
            }
            else if ((objectsToOUT[0].GetType() == typeof(Point)))
            {
                foreach (Point point in objectsToOUT)
                    result += DumpObject(point) + ";";
            }
            else if ((objectsToOUT[0].GetType() == typeof(Size)))
            {
                foreach (Size size in objectsToOUT)
                    result += DumpObject(size) + ";";

            }
            else
            {
                foreach (var obj in (objectsToOUT))
                    result += DumpObject(obj) + ";";
            }

            return result;
        }


        /// <summary>
        /// Преобразует объект в строку. Для объектов EA визуализирует значения важных свойств
        /// </summary>        
        public static string DumpObject(object objectToOUT)
        {
            if (objectToOUT == null)
            {
                return "null";
            }

            string result = ":";

            if ((objectToOUT as EA.Element) != null)
            {
                EA.Element curElement = objectToOUT as EA.Element;
                {
                    if (curElement.ClassfierID != 0)
                    {
                        result += "instance(" + curElement.Type;
                        EA.Element classifier = EARepository.GetElementByID(curElement.ClassfierID);
                        result += "," + curElement.Name + "/" + classifier.Name + "," + classifier.Stereotype + ")";
                    }
                    else
                    {
                        result += "classifier(" + curElement.Type;
                        result += "," + curElement.Name + "," + curElement.Stereotype + ")";
                    }
                }
            }
            else if ((objectToOUT as EA.DiagramObject) != null)
            {
                EA.DiagramObject curDA = objectToOUT as EA.DiagramObject;
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
                }
            }
            else if ((objectToOUT as EA.Package) != null)
            {
                EA.Package curPackage = objectToOUT as EA.Package;
                result += "package(" + curPackage.Name + ")";
            }
            else if ((objectToOUT as EA.Connector) != null)
            {
                EA.Connector curConnector = objectToOUT as EA.Connector;
                EA.Element elementFrom = EARepository.GetElementByID(curConnector.ClientID);
                string elementFromDump = DumpObject(elementFrom);
                EA.Element elementTo = EARepository.GetElementByID(curConnector.SupplierID);
                string elementToDump = DumpObject(elementTo);

                result += "connector(" + curConnector.Name + "," + curConnector.Type + ") from-to " + elementFromDump + "-" + elementToDump;
            }
            else if (objectToOUT.GetType() == typeof(Point))
            {
                Point point = (Point)objectToOUT;
                result += "(X=" + point.X.ToString() + ", Y=" + point.Y.ToString() + ");";
            }
            else if (objectToOUT.GetType() == typeof(Size))
            {
                Size size = (Size)objectToOUT;
                result += "(Width=" + size.Width.ToString() + ", Height=" + size.Height.ToString() + ");";
            }
            else
            {
                result += objectToOUT.ToString() + ";";
            }

            return result;
        }


        /// <summary>
        ///  Возыращает список выделенных в дереве библиотечных элементов
        /// </summary>
        /// <returns></returns>
        public static List<EA.Element> GetSelectedLibElement_Tree()
        {
            List<EA.Element> result = new List<EA.Element>();

            foreach (EA.Element curElement in EARepository.GetTreeSelectedElements())
            {
                if (LibraryHelper.IsLibrary(curElement))
                {
                    result.Add(curElement);
                }
            }

            return result;
        }

        /// <summary>
        ///  Возыращает список выделенных в диаграмме библиотечных элементов
        /// </summary>
        /// <returns></returns>
        public static List<EA.Element> GetSelectedLibElement_Diagram()
        {
            List<EA.Element> result = new List<EA.Element>();

            foreach (EA.DiagramObject curDA in Context.CurrentDiagram.SelectedObjects)
            {
                EA.Element curElement = EARepository.GetElementByID(curDA.ElementID);

                if (LibraryHelper.IsLibrary(curElement))
                {
                    result.Add(curElement);
                }
            }

            return result;
        }

        /// <summary>
        ///  Возыращает список выделенных в диаграмме коннекторов
        /// </summary>
        /// <returns></returns>
        public static EA.Connector GetSelectedLibConnector_Diagram(bool checkISLibrary = true)
        {
            EA.Connector result = null;

            EA.Connector selectedConnector = Context.CurrentDiagram.SelectedConnector;
            if (selectedConnector != null)
            {
                if (!checkISLibrary)
                    result = selectedConnector;
                else if (LibraryHelper.IsLibrary(selectedConnector))
                    result = selectedConnector;
            }

            return result;

        }

        /// <summary>
        /// Возвращает перечисление со списокм возможных групп узлов из заведённого в библиотеке элемента
        /// </summary>
        /// <returns></returns>
        public static List<string> GetNodeGroupEnum()
        {
            List<string> result = new List<string>();

            EA.Element ngEnumElement = EARepository.GetElementByGuid(DAConst.DP_NodeGroupsEnumGUID);
            foreach (EA.Attribute attribute in ngEnumElement.Attributes)
            {
                result.Add(attribute.Name);
            }

            return result;
        }

        public static bool CheckCurrentDiagram(bool showUI = true, bool autoSetCurrentDiagram = false)
        {
            bool result = false;

            EA.Diagram currentOpenedDiagram = EARepository.GetCurrentDiagram();
            EA.Diagram currentLibDiagram = Context.CurrentDiagram;

            if (currentOpenedDiagram == null && currentLibDiagram != null)
            {
                if (autoSetCurrentDiagram)
                {
                    EARepository.ActivateDiagram(currentLibDiagram.DiagramID);
                    result = true;
                }
                else if (showUI)
                {
                    if (MessageBox.Show("Текущая библиотечная диаграмма не открыта. Открыть?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        EARepository.ActivateDiagram(currentLibDiagram.DiagramID);
                        result = true;
                    }
                }
            }
            else if (currentOpenedDiagram.DiagramID != currentLibDiagram.DiagramID)
            {
                if (autoSetCurrentDiagram)
                {
                    Context.CurrentDiagram = currentOpenedDiagram;
                    result = true;
                }
                else if (showUI)
                {
                    DialogResult dr = MessageBox.Show("Текущая Открытая диаграмма не библиотечная библиотечная диаграмма не открыта. Назначить текущей открытую (Да) /Открыть библиотечную (Нет)? ", "", MessageBoxButtons.YesNoCancel);
                    if (dr == DialogResult.No)
                    {
                        EARepository.ActivateDiagram(currentLibDiagram.DiagramID);
                        result = true;
                    }
                    else if (dr == DialogResult.Yes)
                    {
                        Context.CurrentDiagram = currentOpenedDiagram;
                        result = true;
                    }
                }
            }
            else if (currentOpenedDiagram.DiagramID == currentLibDiagram.DiagramID)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Дерьмометод EA.Package.Elements не возвращает вложенные (а при вкладывании элементов на диаграмме, элементы вкладываются (мать....))
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static List<EA.Element> GetAllPackageElements(EA.Package package)
        {
            List<EA.Element> result = new List<EA.Element>();

            List<EA.Element> curLevelElements = new List<EA.Element>();
            foreach (EA.Element element in package.Elements)
            {
                curLevelElements.Add(element);
            }


            while (curLevelElements.Count > 0)
            {
                List<EA.Element> nextLevelElements = new List<EA.Element>();

                foreach (EA.Element element in curLevelElements)
                {
                    result.Add(element);

                    foreach (EA.Element nextElement in element.Elements)
                    {
                        nextLevelElements.Add(nextElement);
                    }
                }

                curLevelElements = nextLevelElements;

            }



            return result;
        }

        /// <summary>
        /// Возвращает элементы, связанные с указанным заданной связью + находящиеся с указанного конца
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static List<EA.Element> GetConnectedElements(EA.Element element, LinkType linkType, byte connectorEnd = 1 /*0=source 1=target*/ )
        {
            List<EA.Element> result = new List<EA.Element>();

            foreach (EA.Connector connector in element.Connectors)
            {
                if ((connectorEnd == 0 /*source*/ && connector.ClientID == element.ElementID) || connectorEnd == 1 /*source*/ && connector.SupplierID == element.ElementID)
                    continue; // не тем концом в другой элемент упирается

                if (LibraryHelper.IsLibrary(connector) && LTHelper.GetConnectorType(connector) == linkType)
                { // если связь нужного типа 
                    EA.Element otherEndElement = EARepository.GetElementByID((connectorEnd == 0) ? connector.ClientID : connector.SupplierID);
                    if (LibraryHelper.IsLibrary(otherEndElement))
                    {
                        result.Add(otherEndElement);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Возвращает если есть линк на текущей диаграмме для указанного коннектора
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static EA.DiagramLink GetDLFromConnector(int connectorID)
        {
            EA.DiagramLink result = null;

            EA.Collection diagramLinks = Context.CurrentDiagram.DiagramLinks;

            foreach (EA.DiagramLink diagramLink in diagramLinks)
            {
                if (diagramLink.ConnectorID == connectorID )
                {
                    result = diagramLink;
                    break;
                }
            }

            return result;
        }


    }
}

