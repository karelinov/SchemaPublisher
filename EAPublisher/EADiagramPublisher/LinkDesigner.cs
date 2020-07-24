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
                return Context.CurrentDiagram;
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
                ExecResult<CreateNewLinkData> createNewLinkData = new FCreateNewLink().Execute();
                if (createNewLinkData.code != 0) return result;


                // надо проверить, нет ли уже такого линка между элементами
                foreach (EA.Connector connector in firstElement.Connectors)
                {
                    if (connector.ClientID == secondElement.ElementID || connector.SupplierID == secondElement.ElementID) {
                        /// !!!!! В будущем надо проверять не только тип, но и и идентификатор !!!!!
                        if (EAHelper.IsLibrary(connector) && connector.TaggedValues.GetByName(DAConst.DP_LinkTypeTag) != null && ((EA.ConnectorTag)connector.TaggedValues.GetByName(DAConst.DP_LinkTypeTag)).Value == Enum.GetName(typeof(LinkType), createNewLinkData.value))
                        {
                            throw new Exception("Запрашиваемая связь уже существует");
                        }
                    }
                }


                // Создаём
                EA.Connector newConnector = LinkDesignerHelper.CreateConnector(createNewLinkData.value, firstDA, secondDA, true);

                CurrentDiagram.DiagramLinks.Refresh();
                EARepository.ReloadDiagram(CurrentDiagram.DiagramID);

                EAHelper.Out("Создан ", new EA.Connector[] { newConnector });

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }


        public ExecResult<Boolean> SetLinkVisibility()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();

            EAHelper.Out("");

            try
            {
                // Проверяем, что установлена текущая диаграмма
                if (CurrentDiagram == null)
                {
                    if (EARepository.GetCurrentDiagram() != null)
                    {
                        Context.CurrentDiagram = EARepository.GetCurrentDiagram();
                        EAHelper.Out("Установлена текущая диаграмма");
                    }
                    else
                    {
                        throw new Exception("Нет текущей диаграммы");
                    }

                }

                // запускаем форму
                ExecResult<LinkVisibilityData> selectLVResult = new FSetLinkVisibility().Execute();
                if (selectLVResult.code != 0) return result;


                // проходимся по элементам диаграммы
                foreach (EA.DiagramObject diagramObject in CurrentDiagram.DiagramObjects)
                {
                    // Получаем элемент
                    EA.Element diagramElement = EARepository.GetElementByID(diagramObject.ElementID);

                    // Получаем коннекторы элемента
                    foreach(EA.Connector connector in diagramElement.Connectors)
                    {
                        // проверяем, что коннектор может быть потенциально показан на диаграмме, т.е, что оба его элемента на диаграмме
                        EA.Element secondElement = EARepository.GetElementByID((connector.ClientID == diagramElement.ElementID) ? connector.SupplierID : connector.ClientID);
                        EA.DiagramObject secondElementDA = CurrentDiagram.GetDiagramObjectByID(secondElement.ElementID, "");
                        if (secondElementDA == null) continue;

                        // Теперь смотрим на настройки видимости коннектора
                        if (EAHelper.IsLibrary(connector)) {

                            LinkType linkType = LTHelper.GetConnectorType(connector);

                            // показ 
                            if (selectLVResult.value.showLinkType.Contains(linkType)) { 
                                EA.DiagramLink connectorLink = EAHelper.GetConnectorLink(connector);
                                if (connectorLink == null)
                                {
                                    connectorLink = LinkDesignerHelper.CreateLink(connector);
                                    connectorLink.Update();
                                }
                                if (connectorLink.IsHidden)
                                {
                                    connectorLink.IsHidden = false;
                                    connectorLink.Update();
                                }

                            }

                            // скрыть 
                            if (selectLVResult.value.hideLinkType.Contains(linkType))
                            {
                                EA.DiagramLink connectorLink = EAHelper.GetConnectorLink(connector);
                                if (connectorLink != null)
                                {
                                    connectorLink.IsHidden = true;
                                    connectorLink.Update();
                                }
                            }
                        }
                        else // для небиблиотечных
                        { 
                            EA.DiagramLink connectorLink = EAHelper.GetConnectorLink(connector);
                            if ((!connectorLink.IsHidden) != selectLVResult.value.showNotLibElements)
                            {
                                connectorLink.IsHidden = selectLVResult.value.showNotLibElements;
                                connectorLink.Update();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            CurrentDiagram.DiagramLinks.Refresh();
            EARepository.ReloadDiagram(CurrentDiagram.DiagramID);

            return result;
        }

        public void SetLinkVisibility(LinkType linkType, bool visibility = true)
        {
                // проходимся по элементам диаграммы
                foreach (EA.DiagramObject diagramObject in CurrentDiagram.DiagramObjects)
                {
                    // Получаем элемент
                    EA.Element diagramElement = EARepository.GetElementByID(diagramObject.ElementID);

                    // Получаем коннекторы элемента
                    foreach (EA.Connector connector in diagramElement.Connectors)
                    {
                        // Получаем тип коннектора
                        LinkType curLinkType = LTHelper.GetConnectorType(connector);
                        if (linkType != curLinkType) continue;



                    // проверяем, что коннектор может быть потенциально показан на диаграмме, т.е, что оба его элемента на диаграмме
                    EA.Element secondElement = EARepository.GetElementByID((connector.ClientID == diagramElement.ElementID) ? connector.SupplierID : connector.ClientID);
                        EA.DiagramObject secondElementDA = CurrentDiagram.GetDiagramObjectByID(secondElement.ElementID, "");
                        if (secondElementDA == null) continue;

                        // Теперь смотрим на настройки видимости коннектора
                        if (EAHelper.IsLibrary(connector))
                        {

                            LinkType linkType = LTHelper.GetConnectorType(connector);

                            // показ 
                            if (selectLVResult.value.showLinkType.Contains(linkType))
                            {
                                EA.DiagramLink connectorLink = EAHelper.GetConnectorLink(connector);
                                if (connectorLink == null)
                                {
                                    connectorLink = LinkDesignerHelper.CreateLink(connector);
                                    connectorLink.Update();
                                }
                                if (connectorLink.IsHidden)
                                {
                                    connectorLink.IsHidden = false;
                                    connectorLink.Update();
                                }

                            }

                            // скрыть 
                            if (selectLVResult.value.hideLinkType.Contains(linkType))
                            {
                                EA.DiagramLink connectorLink = EAHelper.GetConnectorLink(connector);
                                if (connectorLink != null)
                                {
                                    connectorLink.IsHidden = true;
                                    connectorLink.Update();
                                }
                            }
                        }
                        else // для небиблиотечных
                        {
                            EA.DiagramLink connectorLink = EAHelper.GetConnectorLink(connector);
                            if ((!connectorLink.IsHidden) != selectLVResult.value.showNotLibElements)
                            {
                                connectorLink.IsHidden = selectLVResult.value.showNotLibElements;
                                connectorLink.Update();
                            }
                        }
                    }
                }
        }




        /// <summary>
        /// Устанавливает видимость указанного типа коннекторов на диаграммме
        /// </summary>
        public static void SetConnectorVisibility(LinkType LinkType, bool visibility = true)
        {
            foreach (EA.DiagramLink diagramLink in Context.CurrentDiagram.DiagramLinks)
            {

                EA.Connector connector = Context.EARepository.GetConnectorByID(diagramLink.ConnectorID);

                switch (LinkType)
                {
                    case LinkType.Deploy:
                        if (connector.Type == "Dependency" && connector.Stereotype == "deploy")
                        {
                            EAHelper.SetConnectorVisibility(diagramLink, visibility);
                        }
                        break;


                }
            }
        }

    }
}

