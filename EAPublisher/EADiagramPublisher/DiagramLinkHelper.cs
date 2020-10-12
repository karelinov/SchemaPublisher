using EADiagramPublisher.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher
{
    /// <summary>
    /// Хэлпер для манипулирования линками диаграммы EA
    /// </summary>
    class DiagramLinkHelper
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


        public static void SetDiagramLinkVisibility(EA.DiagramLink diagramLink, bool visibility)
        {
            diagramLink.IsHidden = !visibility;
            diagramLink.Update();
        }


        public static void ApplyStyleToDL(EA.DiagramLink diagramLink, SetLinkStyle setLinkStyle)
        {
            if (setLinkStyle.DoSetLinkStyle)
            {

                if (setLinkStyle.SetLineWidth)
                    diagramLink.LineWidth = setLinkStyle.LineWidth;

                if (setLinkStyle.SetColor)
                    diagramLink.LineColor = (setLinkStyle.Color.B * 256 + setLinkStyle.Color.G) * 256 + setLinkStyle.Color.R;

                if (setLinkStyle.SetLineStyle)
                    diagramLink.LineStyle = setLinkStyle.LineStyle;
            }

            diagramLink.Update();
        }

        public static EA.DiagramLink CreateDiagramLink(EA.Connector connector)
        {
            EA.DiagramLink diagramLink = CurrentDiagram.DiagramLinks.AddNew("", "");
            diagramLink.ConnectorID = connector.ConnectorID;
            diagramLink.Update();

            return diagramLink;
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

    }
}
