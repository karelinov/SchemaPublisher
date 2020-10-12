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

    }
}

