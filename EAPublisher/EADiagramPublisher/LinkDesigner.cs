using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EADiagramPublisher.Contracts;
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
                if (!EAHelper.CheckCurrentDiagram())
                    throw new Exception("Не установлена или не открыта текущая диаграмма");


                // На диаграмме должны быть выделены 2 (библиотечных) элемента 
                var selectedObjects = CurrentDiagram.SelectedObjects;
                if (selectedObjects.Count != 2)
                    throw new Exception("Должно быть выделено 2 элемента");

                EA.DiagramObject firstDA = selectedObjects.GetAt(0);
                EA.Element firstElement = EARepository.GetElementByID(firstDA.ElementID);
                EA.DiagramObject secondDA = selectedObjects.GetAt(1);
                EA.Element secondElement = EARepository.GetElementByID(secondDA.ElementID);

                if (!LibraryHelper.IsLibrary(firstElement) || !LibraryHelper.IsLibrary(secondElement))
                    throw new Exception("Должны быть выделены библиотечные элементы");

                EAHelper.Out("Выделенные элементы: ", new EA.Element[] { firstElement, secondElement });

                // запускаем форму
                ExecResult<ConnectorData> createNewLinkData = new FCreateNewLink().Execute(firstDA, secondDA);
                if (createNewLinkData.code != 0) return result;


                // надо проверить, нет ли уже такого линка между элементами
                foreach (EA.Connector connector in firstElement.Connectors)
                {
                    if (connector.ClientID == secondElement.ElementID || connector.SupplierID == secondElement.ElementID)
                    {
                        if (LibraryHelper.IsLibrary(connector))
                        {
                            LinkType connectorLinkType = LTHelper.GetConnectorType(connector);
                            if (createNewLinkData.value.LinkType == connectorLinkType)
                            {
                                if (EAHelper.GetTaggedValue(connector, DAConst.DP_FlowIDTag) == createNewLinkData.value.FlowID && EAHelper.GetTaggedValue(connector, DAConst.DP_SegmentIDTag) == createNewLinkData.value.SegmentID)
                                {
                                    throw new Exception("Запрашиваемая связь уже существует");
                                }
                            }
                        }
                    }
                }


                // Создаём
                EA.Connector newConnector = LinkDesignerHelper.CreateConnector(createNewLinkData.value, true);

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


        public ExecResult<Boolean> SetConnectorVisibility()
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

                // Обрабатываем результаты
                foreach (var curLinkType in selectLVResult.value.showLinkType)
                {
                    SetConnectorVisibility(curLinkType, true);
                }

                foreach (var curLinkType in selectLVResult.value.hideLinkType)
                {
                    SetConnectorVisibility(curLinkType, false);
                }

                SetConnectorVisibility_Untyped(selectLVResult.value.showNotLibElements);

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            CurrentDiagram.DiagramLinks.Refresh();
            EARepository.ReloadDiagram(CurrentDiagram.DiagramID);

            return result;
        }

        /// <summary>
        /// Включает/выключает показ коннекторов указанного типа на диаграмме
        /// </summary>
        /// <param name="linkType"></param>
        /// <param name="visibility"></param>
        public void SetConnectorVisibility(LinkType linkType, bool visibility = true)
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
                    try
                    {
                        LinkType curLinkType = LTHelper.GetConnectorType(connector);
                        if (linkType != curLinkType) continue;
                    }
                    catch (Exception ex)
                    {
                        EAHelper.OutA("Не удалось определить тип коннектора " + ex.StackTrace, new EA.Connector[] { connector });
                        continue;
                    }

                    // проверяем, что коннектор может быть потенциально показан на диаграмме, т.е, что оба его элемента на диаграмме
                    EA.Element secondElement = EARepository.GetElementByID((connector.ClientID == diagramElement.ElementID) ? connector.SupplierID : connector.ClientID);
                    EA.DiagramObject secondElementDA = CurrentDiagram.GetDiagramObjectByID(secondElement.ElementID, "");
                    if (secondElementDA == null) continue;

                    // Теперь смотрим на настройки видимости коннектора
                    if (LibraryHelper.IsLibrary(connector))
                    {

                        LinkType connectorlinkType = LTHelper.GetConnectorType(connector);
                        if (linkType == connectorlinkType)
                        {
                            EA.DiagramLink connectorLink = EAHelper.GetConnectorLink(connector);
                            if (connectorLink == null)
                            {
                                connectorLink = LinkDesignerHelper.CreateLink(connector);
                                connectorLink.Update();
                            }

                            EAHelper.SetDiagramLinkVisibility(connectorLink, visibility);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Устанавливает видимость указанного типа коннекторов на диаграммме
        /// </summary>
        public void SetLinkTypeVisibility(LinkType LinkType, bool visibility = true)
        {
            foreach (EA.DiagramLink diagramLink in Context.CurrentDiagram.DiagramLinks)
            {

                EA.Connector connector = Context.EARepository.GetConnectorByID(diagramLink.ConnectorID);
                LinkType connectorLinkType = LTHelper.GetConnectorType(connector);

                switch (LinkType)
                {
                    case LinkType.Deploy:

                        if (connectorLinkType == LinkType.Deploy)
                        {
                            EAHelper.SetDiagramLinkVisibility(diagramLink, visibility);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Устанавливает видимость для нетипизированных коннекторов на диаграмме
        /// </summary>
        public void SetConnectorVisibility_Untyped(bool visibility = true)
        {
            // проходимся по элементам диаграммы
            foreach (EA.DiagramObject diagramObject in CurrentDiagram.DiagramObjects)
            {
                // Получаем элемент
                EA.Element diagramElement = EARepository.GetElementByID(diagramObject.ElementID);

                // Получаем коннекторы элемента
                foreach (EA.Connector connector in diagramElement.Connectors)
                {
                    // Проверяем тип коннектора
                    if (LibraryHelper.IsLibrary(connector)) continue;

                    // проверяем, что коннектор может быть потенциально показан на диаграмме, т.е, что оба его элемента на диаграмме
                    EA.Element secondElement = EARepository.GetElementByID((connector.ClientID == diagramElement.ElementID) ? connector.SupplierID : connector.ClientID);
                    EA.DiagramObject secondElementDA = CurrentDiagram.GetDiagramObjectByID(secondElement.ElementID, "");
                    if (secondElementDA == null) continue;

                    // Получаем линк коннектора на диаграмме
                    EA.DiagramLink connectorLink = EAHelper.GetConnectorLink(connector);
                    if (connectorLink == null) continue;

                    // Устанавливаем видимость 
                    EAHelper.SetDiagramLinkVisibility(connectorLink, visibility);
                }
            }
        }

        /// <summary>
        /// Установка Tags для выделенного коннектора
        /// </summary>
        /// <returns></returns>
        public ExecResult<Boolean> SetConnectorTags(string location)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                // Сначала получаем список выделеннеых библиотечных элементов
                EA.Connector selectedConnector = EAHelper.GetSelectedLibConnector_Diagram(false);

                if (selectedConnector == null)
                    throw new Exception("Не выделены библиотечные элементы");


                // Конструируем данные тэгов для формы
                List<TagData> curTagDataList = new List<TagData>();

                foreach (EA.ConnectorTag taggedValue in selectedConnector.TaggedValues)
                {
                    string tagName = taggedValue.Name;

                    TagData curTagData = new TagData() { TagName = tagName, TagValue = taggedValue.Value };
                    curTagData.TagState = true;
                    curTagData.Ex = false;
                    curTagData.Count = 1;
                    curTagDataList.Add(curTagData);
                }

                // Открываем форму для установки Tags
                ExecResult<List<TagData>> setTagsResult = new FSetTags().Execute(curTagDataList);
                if (setTagsResult.code != 0) return result;

                // Прописываем в элементах что наустанавливали на форме
                foreach (TagData curTagData in setTagsResult.value)
                {
                    if (curTagData.Enabled) // записываем только для Tags, в котоорые разрешено
                    {

                        if (curTagData.TagState == false)
                        {
                            EAHelper.TaggedValueRemove(selectedConnector, curTagData.TagName);
                        }
                        else
                        {
                            EAHelper.TaggedValueSet(selectedConnector, curTagData.TagName, curTagData.TagValue);
                        }
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
        /// Установка Tags для выделеного коннектора и других видимых на диаграмме линков того же типа
        /// </summary>
        /// <returns></returns>
        public ExecResult<Boolean> SetSimilarLinksTags(string location)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                // Сначала получаем список выделеннеых библиотечных элементов
                EA.Connector selectedConnector = EAHelper.GetSelectedLibConnector_Diagram();

                if (selectedConnector == null)
                    throw new Exception("Не выделены библиотечные элементы");

                // Создаём список коннекторов и добавляем к нему выделенный
                List<EA.Connector> connectorList = new List<EA.Connector>();

                // Ищем на диаграмме другие линки такого же типа
                foreach (EA.DiagramLink curDL in CurrentDiagram.DiagramLinks)
                {
                    EA.Connector curConnector = EARepository.GetConnectorByID(curDL.ConnectorID);
                    if (!curDL.IsHidden && curConnector.Type == selectedConnector.Type)
                    {
                        connectorList.Add(curConnector);
                    }
                }


                // Конструируем данные тэгов для формы
                List<TagData> curTagDataList = new List<TagData>();

                foreach (EA.Connector connector in connectorList)
                {
                    foreach (EA.ConnectorTag taggedValue in selectedConnector.TaggedValues)
                    {
                        string tagName = taggedValue.Name;

                        TagData curTagData;
                        curTagData = curTagDataList.FirstOrDefault(item => (item.TagName == tagName));
                        if (curTagData == null)
                        {
                            curTagData = new TagData() { TagName = tagName, TagValue = taggedValue.Value };
                            curTagDataList.Add(curTagData);
                        }
                        curTagData.TagState = true;
                        curTagData.Ex = false;
                        curTagData.Count++;
                    }
                }

                // Открываем форму для установки Tags
                ExecResult<List<TagData>> setTagsResult = new FSetTags().Execute(curTagDataList);
                if (setTagsResult.code != 0) return result;

                // Прописываем в элементах что наустанавливали на форме
                foreach (EA.Connector connector in connectorList)
                {
                    foreach (TagData curTagData in setTagsResult.value)
                    {
                        if (curTagData.Enabled) // записываем только для Tags, в котоорые разрешено
                        {

                            if (curTagData.TagState == false)
                            {
                                EAHelper.TaggedValueRemove(connector, curTagData.TagName);
                            }
                            else
                            {
                                EAHelper.TaggedValueSet(connector, curTagData.TagName, curTagData.TagValue);
                            }
                        }
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
        /// Установка Выделения цветом, толщиной... указанных коннекторов
        /// </summary>
        /// <returns></returns>
        public ExecResult<Boolean> LinksSelection(string location)
        {

            ExecResult<Boolean> result = new ExecResult<bool>();

            try
            {

                // Открываем форму для установки свойств линков
                ExecResult<LinksOperationData> linksOperationDataResult = new FLinkSelection().Execute(Context.ConnectorData);

                if (linksOperationDataResult.code != 0) return result;

                // Выполняем операцию
                //...


            }
            catch (Exception ex)
            {
                result.setException(ex);
            }
            return result;

        }


    }
}

