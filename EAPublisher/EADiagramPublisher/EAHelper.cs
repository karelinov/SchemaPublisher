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
using System.Xml.Linq;
using EADiagramPublisher.SQL;
using System.Xml.XPath;

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


        /// <summary>
        /// Возвращает список элементов ElementData для текущей диаграммы
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, ElementData> GetCurDiagramElementData()
        {
            Dictionary<int, ElementData> result = new Dictionary<int, ElementData>();


            string[] args = new string[] { Context.CurrentDiagram.DiagramGUID };
            XDocument sqlResultSet = SQLHelper.RunSQL("CurDiagramObjects.sql", args);

            IEnumerable<XElement> rowNodes = sqlResultSet.Root.XPathSelectElements("/EADATA/Dataset_0/Data/Row");
            foreach (XElement rowNode in rowNodes)
            {
                ElementData elementData;

                int object_id = int.Parse(rowNode.Descendants("object_id").First().Value);

                if (result.ContainsKey(object_id))
                {
                    elementData = result[object_id];
                }
                else
                {
                    elementData = new ElementData();
                    elementData._ElementID = object_id;
                    elementData.Name = rowNode.Descendants("name").First().Value;
                    elementData.EAType = rowNode.Descendants("object_type").First().Value;
                    elementData.Note = rowNode.Descendants("note").First().Value;
                    string sClassifierID = rowNode.Descendants("classifier_id").First().Value;
                    if (sClassifierID != "")
                    {
                        elementData.ClassifierID = int.Parse(rowNode.Descendants("classifier_id").First().Value);
                        elementData.ClassifierName = rowNode.Descendants("classifier_name").First().Value;
                        elementData.ClassifierEAType = rowNode.Descendants("classifier_type").First().Value;
                    }

                    result.Add(object_id, elementData);
                }

                string tagName = rowNode.Descendants("property").First().Value;
                string tagValue = rowNode.Descendants("value").First().Value;

                if (tagName == DAConst.DP_LibraryTag)
                    elementData.IsLibrary = true;
                if (tagName == DAConst.DP_ComponentLevelTag)
                    elementData.ComponentLevel = Enum.Parse(typeof(ComponentLevel), tagValue) as ComponentLevel?;
                if (tagName == DAConst.DP_NodeGroupsTag)
                    elementData.NodeGroups = tagValue.Split(',');


            }

            return result;
        }

        /// <summary>
        /// Возвращает список элементов ElementData для текущей библиотеки
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, ElementData> GetCurLibElementData()
        {
            Dictionary<int, ElementData> result = new Dictionary<int, ElementData>();


            string[] args = new string[] { String.Join(",", Context.CurLibPackageIDs)};
            XDocument sqlResultSet = SQLHelper.RunSQL("CurLibObjects.sql", args);

            IEnumerable<XElement> rowNodes = sqlResultSet.Root.XPathSelectElements("/EADATA/Dataset_0/Data/Row");
            foreach (XElement rowNode in rowNodes)
            {
                ElementData elementData;

                int object_id = int.Parse(rowNode.Descendants("object_id").First().Value);

                if (result.ContainsKey(object_id))
                {
                    elementData = result[object_id];
                }
                else
                {
                    elementData = new ElementData();
                    elementData._ElementID = object_id;
                    elementData.Name = rowNode.Descendants("name").First().Value;
                    elementData.EAType = rowNode.Descendants("object_type").First().Value;
                    elementData.Note = rowNode.Descendants("note").First().Value;
                    string sClassifierID = rowNode.Descendants("classifier_id").First().Value;
                    if (sClassifierID != "")
                    {
                        elementData.ClassifierID = int.Parse(rowNode.Descendants("classifier_id").First().Value);
                        elementData.ClassifierName = rowNode.Descendants("classifier_name").First().Value;
                        elementData.ClassifierEAType = rowNode.Descendants("classifier_type").First().Value;
                    }

                    result.Add(object_id, elementData);
                }

                string tagName = rowNode.Descendants("property").First().Value;
                string tagValue = rowNode.Descendants("value").First().Value;

                if (tagName == DAConst.DP_LibraryTag)
                    elementData.IsLibrary = true;
                if (tagName == DAConst.DP_ComponentLevelTag)
                    elementData.ComponentLevel = Enum.Parse(typeof(ComponentLevel), tagValue) as ComponentLevel?;
                if (tagName == DAConst.DP_NodeGroupsTag)
                    elementData.NodeGroups = tagValue.Split(',');


            }

            return result;
        }



        /// <summary>
        /// Возвращает список ID коннекторов для элементов текущей диаграммы
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int,bool> GetCurDiagramConnectorsID()
        {
            Dictionary<int, bool> result = new Dictionary<int, bool>();

            string[] args = new string[] { Context.CurrentDiagram.DiagramGUID };
            XDocument sqlResultSet = SQLHelper.RunSQL("CurDiagramConnectorsID.sql", args);

            IEnumerable<XElement> rowNodes = sqlResultSet.Root.XPathSelectElements("/EADATA/Dataset_0/Data/Row");
            foreach (XElement rowNode in rowNodes)
            {
                int connector_id = int.Parse(rowNode.Descendants("connector_id").First().Value);
                bool hidden = bool.Parse(rowNode.Descendants("hidden").First().Value);
                result.Add(connector_id, hidden);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список коннекторов для элементов текущей диаграммы
        /// </summary>
        /// <returns></returns>
        public static List<ConnectorData> GetCurDiagramConnectors()
        {
            List<ConnectorData> result = new List<ConnectorData>();

            string[] args = new string[] { Context.CurrentDiagram.DiagramGUID };
            XDocument sqlResultSet = SQLHelper.RunSQL("CurDiagramConnectorsID.sql", args);

            IEnumerable<XElement> rowNodes = sqlResultSet.Root.XPathSelectElements("/EADATA/Dataset_0/Data/Row");
            foreach (XElement rowNode in rowNodes)
            {
                int connector_id = int.Parse(rowNode.Descendants("connector_id").First().Value);
                bool hidden = bool.Parse(rowNode.Descendants("hidden").First().Value);

                if (!hidden)
                {
                    ConnectorData connectorData = Context.ConnectorData[connector_id];
                    result.Add(connectorData);
                }
            }

            return result;
        }

    }
}

