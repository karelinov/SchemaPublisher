using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EADiagramPublisher.Enums;
using EADiagramPublisher.Forms;

namespace EADiagramPublisher
{
    /// <summary>
    /// Класс для управления библиотечными связями элементов
    /// </summary>
    public class LinkDesigner
    {
        /// <summary>
        /// Shortcut до глобальной переменной с EA.Diagram + логика установки
        /// </summary>
        public EA.Diagram CurrentDiagram
        {
            get
            {
                return EARepository.GetCurrentDiagram();
                /*
                if (Context.CurrentDiagram == null)
                {
                    var currentDiagram = EARepository.GetCurrentDiagram();
                    if (currentDiagram == null)
                        throw new Exception("Нет активной диаграммы");
                    else
                        Context.CurrentDiagram = currentDiagram;
                }
                return Context.CurrentDiagram;
                */
            }

        }

        /// <summary>
        /// Shortcut до глобальной переменной с EA.Repository
        /// </summary>
        private EA.Repository EARepository
        {
            get
            {
                return Context.EARepository;
            }
        }

        public ExecResult<Boolean> CreateLink()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();

            EAHelper.Out("");

            try
            {
                // На диаграмме должны быть выделены 2 (библиотечных) элемента 
                var selectedObjects = EARepository.GetCurrentDiagram().SelectedObjects;// CurrentDiagram.SelectedObjects;
                if (selectedObjects.Count != 2)
                    throw new Exception("Должно быть выделено 2 элемента");

                EA.DiagramObject firstDA = selectedObjects.GetAt(0);
                EA.Element firstElement = EARepository.GetElementByID(firstDA.ElementID);
                EA.DiagramObject secondDA = selectedObjects.GetAt(1);
                EA.Element secondElement = EARepository.GetElementByID(secondDA.ElementID);

                if (!EAHelper.IsLibrary(firstElement) || !EAHelper.IsLibrary(secondElement))
                    throw new Exception("Должны быть выделены библиотечные элементы");

                EAHelper.Out("Выделенные элементы: ", new EA.Element[] { firstElement, secondElement });

                // запускаем форму
                ExecResult<LinkType> selectLinkTypeResult = new FCreateNewLink().Execute();
                if (selectLinkTypeResult.code != 0) return result;


                // надо проверить, нет ли уже такого линка между элементами
                foreach (EA.Connector connector in firstElement.Connectors)
                {
                    if (connector.ClientID == secondElement.ElementID || connector.SupplierID == secondElement.ElementID) {
                        /// !!!!! В будущем надо проверять не только тип, но и и идентификатор !!!!!
                        if (EAHelper.IsLibrary(connector) && connector.TaggedValues.GetByName(DAConst.DP_LinkTypeTag) != null && ((EA.ConnectorTag)connector.TaggedValues.GetByName(DAConst.DP_LinkTypeTag)).Value == Enum.GetName(typeof(LinkType), selectLinkTypeResult.value))
                        {
                            throw new Exception("Запрашиваемая связь уже существует");
                        }
                    }
                }


                // Создаём
                EA.Connector newConnector = CreateLink(selectLinkTypeResult.value, firstDA, secondDA);

                EAHelper.Out("Создан ", new EA.Connector[] { newConnector });

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        private EA.Connector CreateLink(LinkType linkType, EA.DiagramObject firstDA, EA.DiagramObject secondDA, string flowID = null, string segmentID = null)
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


            // Помещаем на диаграмму
            EA.DiagramLink diagramLink = CurrentDiagram.DiagramLinks.AddNew("", "");
            diagramLink.ConnectorID = newConnector.ConnectorID;
            diagramLink.Update();

            CurrentDiagram.DiagramLinks.Refresh();
            //SetConnectorVisibility(ConnectorType.Deploy, false);
            EARepository.ReloadDiagram(CurrentDiagram.DiagramID);


            return newConnector;
        }

    }
}

