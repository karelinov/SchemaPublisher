using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


        public static EA.Connector CreateConnector(Forms.CreateNewLinkData createNewLinkData, EA.DiagramObject firstDA, EA.DiagramObject secondDA, bool putOnDiagram = true)
        {
            EA.Connector newConnector = null;

            EA.Element firstElement = EARepository.GetElementByID(firstDA.ElementID);
            EA.Element secondElement = EARepository.GetElementByID(secondDA.ElementID);

            // Определяем тип создаваемого коннектора
            string creatingConnectorType;
            switch (createNewLinkData.linkType)
            {
                case LinkType.Communication:
                case LinkType.Deploy:
                    creatingConnectorType = "Dependency";
                    break;
                case LinkType.InformationFlow:
                    creatingConnectorType = "InformationFlow";
                    break;
                default:
                    throw new Exception("Непредусмотренный тип коннектора при создании");
            }

            // Создаём
            newConnector = firstElement.Connectors.AddNew("", creatingConnectorType);

            if (createNewLinkData.linkType == LinkType.InformationFlow || createNewLinkData.linkType == LinkType.Deploy)
            {
                newConnector.Direction = "Source -> Destination";
            }
            else
            {
                newConnector.Direction = "Unspecified";
            }
            newConnector.ClientID = firstElement.ElementID;
            newConnector.SupplierID = secondElement.ElementID;

            newConnector.Name = createNewLinkData.linkType.ToString();
            if (createNewLinkData.flowID == null)
                newConnector.Name += " " + createNewLinkData.flowID + " " + ((createNewLinkData.segmentID == null) ? "" : createNewLinkData.segmentID);

            newConnector.Update();

            EAHelper.TaggedValueSet(newConnector, DAConst.DP_LibraryTag, "");
            EAHelper.TaggedValueSet(newConnector, DAConst.DP_LinkTypeTag, Enum.GetName(typeof(LinkType), createNewLinkData.linkType));
            EAHelper.TaggedValueSet(newConnector, DAConst.DP_FlowIDTag, createNewLinkData.flowID);
            EAHelper.TaggedValueSet(newConnector, DAConst.DP_SegmentIDTag, createNewLinkData.segmentID);
            EAHelper.TaggedValueSet(newConnector, DAConst.DP_TempLinkTag, createNewLinkData.tempLink.ToString());
            EAHelper.TaggedValueSet(newConnector, DAConst.DP_TempLinkDiagramIDTag, createNewLinkData.tempLink ? createNewLinkData.tempLinkDiagramID : "");


            newConnector.Update();
            newConnector.TaggedValues.Refresh();

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
    }
}
