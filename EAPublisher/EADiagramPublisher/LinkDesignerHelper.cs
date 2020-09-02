using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;
using EADiagramPublisher.Forms;

namespace EADiagramPublisher
{
    /// <summary>
    /// Класс вспомогательных функций для линков
    /// </summary>
    class LinkDesignerHelper
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
        /// Shortcut до глобальной переменной с EA.Diagram + логика установки
        /// </summary>
        public static EA.Diagram CurrentDiagram
        {
            get
            {
                return Context.CurrentDiagram;
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

            newConnector.Name = createNewLinkData.LinkType.ToString();
            if (createNewLinkData.FlowID == null)
                newConnector.Name += " " + createNewLinkData.FlowID + " " + ((createNewLinkData.SegmentID == null) ? "" : createNewLinkData.SegmentID);

            newConnector.Update();

            EAHelper.TaggedValueSet(newConnector, DAConst.DP_LibraryTag, "");
            EAHelper.TaggedValueSet(newConnector, DAConst.DP_LinkTypeTag, Enum.GetName(typeof(LinkType), createNewLinkData.LinkType));
            EAHelper.TaggedValueSet(newConnector, DAConst.DP_FlowIDTag, createNewLinkData.FlowID);
            EAHelper.TaggedValueSet(newConnector, DAConst.DP_SegmentIDTag, createNewLinkData.SegmentID);
            //EAHelper.TaggedValueSet(newConnector, DAConst.DP_TempLinkTag, createNewLinkData.tempLink.ToString());
            //EAHelper.TaggedValueSet(newConnector, DAConst.DP_TempLinkDiagramIDTag, createNewLinkData.tempLink ? createNewLinkData.tempLinkDiagramID : "");


            newConnector.Update();
            newConnector.TaggedValues.Refresh();

            // Добавляем коннектор к кэш информации о коннекторах
            if (Context.ConnectorData != null)
            {
                if (!Context.ConnectorData[createNewLinkData.LinkType].ContainsKey(createNewLinkData.FlowID))
                {
                    Context.ConnectorData[createNewLinkData.LinkType].Add(createNewLinkData.FlowID, new List<ConnectorData>());
                }

                Context.ConnectorData[createNewLinkData.LinkType][createNewLinkData.FlowID].Add(createNewLinkData);
            }




            if (putOnDiagram)
            {
                // Помещаем на диаграмму
                EA.DiagramLink diagramLink = CreateLink(newConnector);
            }

            return newConnector;
        }


        public static EA.DiagramLink CreateLink(EA.Connector connector)
        {
            EA.DiagramLink diagramLink = CurrentDiagram.DiagramLinks.AddNew("", "");
            diagramLink.ConnectorID = connector.ConnectorID;
            diagramLink.Update();

            return diagramLink;
        }

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




        public static void ApplyStyleToDiagramLink(EA.DiagramLink diagramLink, bool setLineWidth, int lineWidth, bool setColor, Color color, bool setLineStyle, EA.LinkLineStyle lineStyle)
        {
            if (setLineWidth)
                diagramLink.LineWidth = lineWidth;

            if (setColor)
                diagramLink.LineColor = (color.B * 256 + color.G) * 256 + color.R;

            if (setLineStyle)
                diagramLink.LineStyle = lineStyle;


            diagramLink.Update();
        }

        /// <summary>
        /// LINQ+lambda слажали по производительности, пришлось написать свой Comparer для ConnectorData
        /// </summary>
        public class IEqualityComparer_ConnectorData : IEqualityComparer<ConnectorData>
        {
            public bool Equals(ConnectorData x, ConnectorData y)
            {
                return x._ConnectorID == y._ConnectorID;
            }

            public int GetHashCode(ConnectorData obj)
            {
                return obj.Connector.ConnectorID.GetHashCode();
            }
        }


        /// <summary>
        /// Ищет на текущей диаграмме коннекторы для элементов, которые принадлежат к перечисленному ПО
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<ConnectorData> GetDAForSoftwareOnDiagram(List<string> selectedSoftware)
        {
            List<ConnectorData> result = new List<ConnectorData>();

            foreach(EA.DiagramObject diagramObject in CurrentDiagram.DiagramObjects)
            {
                EA.Element element = EARepository.GetElementByID(diagramObject.ElementID);
                string elementSoftware = LibraryHelper.GetElementSoftwareName(element);

                if (selectedSoftware.Contains(elementSoftware))
                {
                    foreach(EA.Connector connector in element.Connectors)
                    {
                        EA.DiagramLink diagramLink = EAHelper.GetDLFromConnector(connector.ConnectorID);
                        if(diagramLink !=null)
                        {
                            ConnectorData connectorData = new ConnectorData(connector);
                            result.Add(connectorData);
                        }
                    }
                }
            }



            return result;
        }

    }
}
