using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;
using EADiagramPublisher.Forms;
using EADiagramPublisher.SQL;

namespace EADiagramPublisher
{
    /// <summary>
    /// Класс вспомогательных функций для коннекторов 
    /// </summary>
    class ConnectorHelper
    {
        /// <summary>
        /// Shortcut до глобальной переменной с EA.Diagram + логика установки
        /// </summary>
        public static EA.Diagram CurrentDiagram
        {
            get
            {
                return Context.CurrentDiagram;
            }

        }

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

        public static EA.Connector CreateConnector(ConnectorData createNewLinkData, bool putOnDiagram = true)
        {
            EA.Connector newConnector = null;

            EA.Element firstElement = EARepository.GetElementByID(createNewLinkData.SourceElementID);
            EA.Element secondElement = EARepository.GetElementByID(createNewLinkData.TargetElementID);

            // Определяем тип создаваемого коннектора
            string creatingConnectorType;
            switch (createNewLinkData.LinkType)
            {
                case LinkType.Communication:
                case LinkType.Deploy:
                    creatingConnectorType = "Dependency";
                    break;
                case LinkType.InformationFlow:
                    creatingConnectorType = "InformationFlow";
                    break;
                case LinkType.SoftwareClassification:
                    creatingConnectorType = "Generalization";
                    break;

                default:
                    throw new Exception("Непредусмотренный тип коннектора при создании");
            }

            // Создаём
            newConnector = firstElement.Connectors.AddNew("", creatingConnectorType);

            if (createNewLinkData.LinkType == LinkType.InformationFlow || createNewLinkData.LinkType == LinkType.Deploy)
            {
                newConnector.Direction = "Source -> Destination";
            }
            else
            {
                newConnector.Direction = "Unspecified";
            }
            newConnector.ClientID = firstElement.ElementID;
            newConnector.SupplierID = secondElement.ElementID;

            newConnector.Name = createNewLinkData.Name;
            newConnector.Notes = createNewLinkData.Notes;

            newConnector.Update();

            EATVHelper.TaggedValueSet(newConnector, DAConst.DP_LibraryTag, "");
            EATVHelper.TaggedValueSet(newConnector, DAConst.DP_LinkTypeTag, Enum.GetName(typeof(LinkType), createNewLinkData.LinkType));
            EATVHelper.TaggedValueSet(newConnector, DAConst.DP_FlowIDTag, createNewLinkData.FlowID);
            EATVHelper.TaggedValueSet(newConnector, DAConst.DP_SegmentIDTag, createNewLinkData.SegmentID);
            //EAHelper.TaggedValueSet(newConnector, DAConst.DP_TempLinkTag, createNewLinkData.tempLink.ToString());
            //EAHelper.TaggedValueSet(newConnector, DAConst.DP_TempLinkDiagramIDTag, createNewLinkData.tempLink ? createNewLinkData.tempLinkDiagramID : "");


            newConnector.Update();
            newConnector.TaggedValues.Refresh();

            // Добавляем коннектор к кэш информации о коннекторах
            createNewLinkData.ConnectorID = newConnector.ConnectorID;
            Context.ConnectorData.Add(createNewLinkData.ConnectorID, createNewLinkData);

            if (putOnDiagram)
            {
                // Помещаем на диаграмму
                EA.DiagramLink diagramLink = DiagramLinkHelper.CreateDiagramLink(newConnector);
            }

            return newConnector;
        }



        /*
        /// <summary>
        /// Функция загружает список коннекторов из текущей библиотеки
        /// </summary>
        /// <returns></returns>
        public static Dictionary<LinkType, Dictionary<string, List<ConnectorData>>> LoadConnectorData()
        {
            var result = new Dictionary<LinkType, Dictionary<string, List<ConnectorData>>>();
            IEqualityComparer<ConnectorData> ConnectorDataEqualityComparer = new IEqualityComparer_ConnectorData();


            foreach (LinkType linkType in Enum.GetValues(typeof(LinkType)))
            {
                result.Add(linkType, new Dictionary<string, List<ConnectorData>>());
            }

            // Получаем библиотеку
            EA.Package libRoot = Context.CurrentLibrary;
            if (libRoot == null)
                throw new Exception("Не установлена библиотека");

            // Ползём по библиотеке и добавляем найденные библиотечные коннекторы в коллекцию
            List<EA.Package> curLibpackages = new List<EA.Package>();
            curLibpackages.Add(libRoot);

            while (curLibpackages.Count > 0)
            {
                List<EA.Package> nextLibpackages = new List<EA.Package>();

                foreach (EA.Package curPackage in curLibpackages)
                {
                    //EAHelper.Out("Checking package elements ", new EA.Package[] { curPackage });

                    foreach (EA.Element curElement in EAHelper.GetAllPackageElements(curPackage))
                    {
                        //EAHelper.Out("Checking element connectors ", new EA.Element[] { curElement });

                        foreach (EA.Connector curConnector in curElement.Connectors)
                        {

                            if (LibraryHelper.IsLibrary(curConnector))
                            {
                                LinkType connectorLinkType = LTHelper.GetConnectorType(curConnector);
                                string connectorFlowID = EAHelper.GetTaggedValue(curConnector, DAConst.DP_FlowIDTag);
                                string connectorSegmentID = EAHelper.GetTaggedValue(curConnector, DAConst.DP_SegmentIDTag);

                                if (!result[connectorLinkType].ContainsKey(connectorFlowID))
                                    result[connectorLinkType].Add(connectorFlowID, new List<ConnectorData>());

                                // Добавляем
                                ConnectorData connectorData = new ConnectorData();
                                connectorData.Name = curConnector.Name;
                                connectorData._ConnectorID = curConnector.ConnectorID;
                                connectorData.Connector = curConnector;
                                connectorData.LinkType = connectorLinkType;
                                connectorData.FlowID = connectorFlowID;
                                connectorData.SegmentID = connectorSegmentID;

                                connectorData.SourceElementID = curConnector.ClientID;
                                connectorData.TargetElementID = curConnector.SupplierID;

                                if (!result[connectorLinkType][connectorFlowID].Contains(connectorData, ConnectorDataEqualityComparer))
                                    result[connectorLinkType][connectorFlowID].Add(connectorData);
                            }

                        }
                    }

                    foreach (EA.Package nextPackage in curPackage.Packages)
                        nextLibpackages.Add(nextPackage);
                }

                curLibpackages = nextLibpackages;
            }

            return result;
        }
        */

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
        /// Возвращает список FlowID, имеющихся в свойствах коннекторов библиотеки
        /// </summary>
        /// <returns></returns>
        public static string[] GetCurrentFlowIDs()
        {
            return Context.ConnectorData.Values.Select(connectorData => connectorData.FlowID).Where(FlowID => FlowID != "" && FlowID != null).Distinct().ToArray<string>();
        }

        /// <summary>
        /// Ищет на текущей диаграмме коннекторы для элементов, которые принадлежат к перечисленному ПО
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<ConnectorData> GetDAForSoftwareOnDiagram(List<string> selectedSoftware)
        {
            List<ConnectorData> result = new List<ConnectorData>();

            foreach (EA.DiagramObject diagramObject in CurrentDiagram.DiagramObjects)
            {
                EA.Element element = EARepository.GetElementByID(diagramObject.ElementID);
                string elementSoftware = LibraryHelper.GetElementSoftwareName(element);

                if (selectedSoftware.Contains(elementSoftware))
                {
                    foreach (EA.Connector connector in element.Connectors)
                    {
                        EA.DiagramLink diagramLink = DiagramLinkHelper.GetDLFromConnector(connector.ConnectorID);
                        if (diagramLink != null)
                        {
                            ConnectorData connectorData = new ConnectorData(connector);
                            result.Add(connectorData);
                        }
                    }
                }
            }



            return result;
        }


        /// <summary>
        /// Возвращает список SegmentID, имеющиеся в свойствах коннекторов библиотеки для коннекторов с указанным FlowID
        /// </summary>
        /// <returns></returns>
        public static string[] GetSegmentsForFlowID(string flowID)
        {
            return Context.ConnectorData.Values.Where(connectorData => connectorData.FlowID == flowID).Select(connectorData => connectorData.SegmentID).ToArray<string>();
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
        /// Функция загружает список коннекторов из текущей библиотеки (используя SQL - запрос)
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, ConnectorData> LoadConnectorData2()
        {
            var result = new Dictionary<int, ConnectorData>();

            string[] args = new string[] { String.Join(",", Context.CurLibPackageIDs)};
            XDocument sqlResultSet = SQLHelper.RunSQL("GetConnectors.sql", args);

            // Коннекторы
            IEnumerable<XElement> rowNodes = sqlResultSet.Root.XPathSelectElements("/EADATA/Dataset_0/Data/Row");
            foreach(XElement rowNode in rowNodes)
            {
                ConnectorData connectorData = new ConnectorData();
                connectorData.ConnectorID = int.Parse(rowNode.Descendants("connector_id").First().Value);
                connectorData.Name = rowNode.Descendants("name").First().Value;
                connectorData.Notes = rowNode.Descendants("notes").First().Value;
                connectorData.SourceElementID = int.Parse(rowNode.Descendants("start_object_id").First().Value);
                connectorData.TargetElementID = int.Parse(rowNode.Descendants("end_object_id").First().Value);

                result.Add(connectorData.ConnectorID, connectorData);
            }

            // Тэги коннекторов
            sqlResultSet = SQLHelper.RunSQL("GetConnectorsTags.sql", args);
            rowNodes = sqlResultSet.Root.XPathSelectElements("/EADATA/Dataset_0/Data/Row");
            foreach (XElement rowNode in rowNodes)
            {
                int elementID = int.Parse(rowNode.Descendants("elementid").First().Value);
                string property = rowNode.Descendants("property").First().Value;
                string value = rowNode.Descendants("value").First().Value;

                if (result.ContainsKey(elementID))
                {
                    ConnectorData connectorData = result[elementID]; // получаем уже созданный объект ConnectorData

                    if (property == DAConst.DP_LibraryTag)
                        connectorData.IsLibrary = true;
                    if (property == DAConst.DP_LinkTypeTag)
                        connectorData.LinkType = (LinkType)Enum.Parse(typeof(LinkType), value);
                    if (property == DAConst.DP_FlowIDTag)
                        connectorData.FlowID = value;
                    if (property == DAConst.DP_SegmentIDTag)
                        connectorData.SegmentID = value;
                }
                else
                {
                    throw new Exception("рассогласование набора коннекторов при загрузке");

                }
            }

            return result;
        }


        /// <summary>
        /// Проверяет, что элементы-концы (линка) помещены на текущую диаграмму
        /// </summary>
        /// <param name="connector"></param>
        public static bool IsConnectorEndsOnCurrentDiagram(int sourceElementID, int targetElementID)

        {
            bool result = false;

            if (Context.CurrentDiagram == null) return false; // Диаграмма не открыта

            EA.DiagramObject sourceDA = Context.CurrentDiagram.GetDiagramObjectByID(sourceElementID,"");
            EA.DiagramObject targetDA = Context.CurrentDiagram.GetDiagramObjectByID(targetElementID,"");

            if (sourceDA != null && targetDA != null)
                result = true;

            return result;
        }


        /// <summary>
        /// Функция обновляет данные коннектора пор данным переданного ConnectorData
        /// </summary>
        /// <param name="connectorData"></param>
        public static void UpdateConnectorByData(ConnectorData connectorData)
        {
            EA.Connector connector = connectorData.Connector;

            connector.Name = connectorData.Name;
            connector.Notes = connectorData.Notes;
            connector.ClientID = connectorData.SourceElementID;
            connector.SupplierID = connectorData.TargetElementID;
            connector.Update();

            if (connectorData.IsLibrary ) {
                EATVHelper.TaggedValueSet(connector, DAConst.DP_LibraryTag, "");
                EATVHelper.TaggedValueSet(connector, DAConst.DP_LinkTypeTag, connectorData.LinkType.ToString());
                EATVHelper.TaggedValueSet(connector, DAConst.DP_FlowIDTag, connectorData.FlowID);
                EATVHelper.TaggedValueSet(connector, DAConst.DP_SegmentIDTag, connectorData.SegmentID);
            }

            Context.ConnectorData[connectorData.ConnectorID] = connectorData;
        }


    }
}
