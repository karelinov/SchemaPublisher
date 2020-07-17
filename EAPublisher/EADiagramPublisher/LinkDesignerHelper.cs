using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EADiagramPublisher.Enums;

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


        public static EA.Connector CreateConnector(LinkType linkType, EA.DiagramObject firstDA, EA.DiagramObject secondDA, bool putOnDiagram = true, string flowID = null, string segmentID = null)
        {
            EA.Connector newConnector = null;

            EA.Element firstElement = EARepository.GetElementByID(firstDA.ElementID);
            EA.Element secondElement = EARepository.GetElementByID(secondDA.ElementID);

            // Создаём
            newConnector = firstElement.Connectors.AddNew("", "Dependency");
            newConnector.Direction = "Unspecified";
            newConnector.ClientID = firstElement.ElementID;
            newConnector.SupplierID = secondElement.ElementID;

            newConnector.Name = linkType.ToString();
            if (flowID == null)
                newConnector.Name += " " + flowID + " " + ((segmentID == null) ? "" : segmentID);

            newConnector.Update();

            EAHelper.SetTaggedValue(newConnector, DAConst.DP_LibraryTag, "");
            EAHelper.SetTaggedValue(newConnector, DAConst.DP_LinkTypeTag, Enum.GetName(typeof(LinkType), linkType));
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
