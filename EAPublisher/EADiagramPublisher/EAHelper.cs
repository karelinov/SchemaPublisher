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
                if (diagramLink.ConnectorID == connectorID)
                {
                    result = diagramLink;
                    break;
                }
            }

            return result;
        }

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
        /// Выполнение SQL запроса в БД
        /// </summary>
        /// <param name="queryText"></param>
        /// <returns></returns>
        public static string RunQuery(string queryText)
        {
            return EARepository.SQLQuery(queryText);
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

                EATVHelper.TaggedValueSet(curElement, DAConst.defaultWidthTag, GetElementSizeOnLibDiagramResult.value.Width.ToString());
                EATVHelper.TaggedValueSet(curElement, DAConst.defaultHeightTag, GetElementSizeOnLibDiagramResult.value.Height.ToString());

                Logger.Out("Найден элемент диаграммы для установки размеров " + GetElementSizeOnLibDiagramResult.value.Width.ToString() + "x" + GetElementSizeOnLibDiagramResult.value.Height.ToString());

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }
    }
}

