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
    /// Класс для решения задач дизайна схем (бизнес-функции)
    /// </summary>
    public class ElementDesigner
    {
        /// <summary>
        /// Shortcut до глобальной переменной с EA.Diagram + логика установки
        /// </summary>
        public EA.Diagram CurrentDiagram
        {
            get
            {
                return Context.CurrentDiagram;

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

        /*
        /// <summary>
        /// Идентификатор текущей библиотеки
        /// </summary>
        private int _CurrentLibraryID;
        public int CurrentLibraryID
        {
            get
            {
                return _CurrentLibraryID;
            }
            set
            {
                _CurrentLibraryID = value;
            }
        }
        */
        /*
        public ExecResult<Boolean> SetCurrentLibrary()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                var selection = EARepository.GetTreeSelectedPackage();
                if (selection == null || !EAHelper.IsLibrary(selection.Element))
                {
                    throw new Exception("Не выделен библиотечный пакет");
                }
                CurrentLibraryID = selection.PackageID;
                result.message = "установлен ID библиотеки=" + CurrentLibraryID;


            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;

        }
        */
        /// <summary>
        /// Размещает на диаграмме под элементом дерево компонентов, размещённых в нём
        /// </summary>
        /// <param name="onlyParent"></param>
        /// <returns></returns>
        public ExecResult<Boolean> PutChildrenDeployHierarchy(string location)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            ElementDesignerHelper.CallLevel = 0;

            try
            {
                EA.Element selectedElement = null;



                switch (location)
                {
                    case "TreeView":
                        throw new Exception("Доступно только для диаграммы");
                    case "Diagram":
                    case "MainMenu":
                        if ((EARepository.GetCurrentDiagram() == null) || (EARepository.GetCurrentDiagram() != null && EARepository.GetCurrentDiagram().DiagramID != CurrentDiagram.DiagramID))
                            throw new Exception("Текущая диаграмма должны быть открыта");

                        List<EA.Element> selectedElementList = EAHelper.GetSelectedLibElement_Diagram();
                        if (selectedElementList.Count == 0)
                            throw new Exception("На текщей диаграммме нет выделенных библиотечных элементов");

                        selectedElement = selectedElementList[0];
                        break;
                }

                // Получаем дерево дочерних элементов контейнеров
                DPTreeNode<EA.Element> сhildrenDHierarchy = LibraryHelper.GetDeployChildrenHierarchy(selectedElement);

                // Для начала размещаем на диаграмме корневой элемент
                EA.DiagramObject rootDA = PutElementOnDiagram(selectedElement);

                // Проходимся по иерархии и размещаем элементы на диаграмме
                List<DPTreeNode<EA.Element>> currentLevelNodes = new List<DPTreeNode<EA.Element>>();
                currentLevelNodes.Add(сhildrenDHierarchy);

                List<DPTreeNode<EA.Element>> childLevelNodes = сhildrenDHierarchy.Children.ToList<DPTreeNode<EA.Element>>();

                Point levelStartPoint = new Point(rootDA.left, rootDA.bottom - DAConst.border);
                Point levelEndPoint = new Point(levelStartPoint.X, levelStartPoint.Y);

                while (childLevelNodes.Count > 0)
                {
                    foreach (DPTreeNode<EA.Element> childLevelNode in childLevelNodes)
                    {
                        // Размещаем элемент на диаграмме
                        EA.DiagramObject curDA = PutElementOnDiagram(childLevelNode.Value);
                        // Подвигаем элемент на отведённым ему уровень

                        EADOHelper.ApplyPointToDA(curDA, new Point(levelEndPoint.X, levelStartPoint.Y));
                        int newLevelRight = curDA.right + DAConst.border;
                        int newLevelBottom = curDA.bottom < levelEndPoint.Y ? curDA.bottom : levelEndPoint.Y;
                        levelEndPoint = new Point(newLevelRight, newLevelBottom);
                    }

                    // коллекционируем список узлов уровнем ниже
                    List<DPTreeNode<EA.Element>> grandchildLevelNodes = new List<DPTreeNode<EA.Element>>();
                    foreach (DPTreeNode<EA.Element> childlevelNode in childLevelNodes)
                    {
                        grandchildLevelNodes.AddRange(childlevelNode.Children);
                    }
                    childLevelNodes = grandchildLevelNodes; // делаем список узлов уровнем ниже - текущим

                    // смещаем координаты размещения следующего уровня компонентов
                    levelStartPoint = new Point(levelStartPoint.X, levelEndPoint.Y - DAConst.border);
                    levelEndPoint = new Point(levelStartPoint.X, levelStartPoint.Y - DAConst.border);
                }


                //CurrentDiagram.DiagramLinks.Refresh();
                //DPAddin.LinkDesigner.SetLinkTypeVisibility(LinkType.Deploy, false);
                EARepository.ReloadDiagram(CurrentDiagram.DiagramID);

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        /// <summary>
        /// Размещает на диаграмме укзаанный элемент и иерархию его контейнеров
        /// </summary>
        /// <param name="onlyParent"></param>
        /// <returns></returns>
        public ExecResult<Boolean> PutChildrenDHierarchyOnDiagram()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            ElementDesignerHelper.CallLevel = 0;

            try
            {
                ExecResult<List<ComponentLevel>> displayLevelsResult = FSelectComponentLevels.Execute();
                if (displayLevelsResult.code != 0) return result;


                // Получаем текущий (библиотечный) элемент дерева 
                EA.Element curElement = null;
                if (EARepository.GetTreeSelectedElements().Count > 0) curElement = EARepository.GetTreeSelectedElements().GetAt(0);
                Logger.Out("элемент:", new EA.Element[] { curElement });
                if (curElement == null || !LibraryHelper.IsLibrary(curElement))
                {
                    throw new Exception("Не выделен библиотечный элемент");
                }

                // Получаем список дочерних элементов контейнеров
                List<EA.Element> сhildrenDHierarchy = LibraryHelper.GetChildHierarchy(curElement);


                // Размещаем на диаграмме элемент
                PutElementOnDiagram(curElement);

                // Проходимся по иерархии и размещаем элементы на диаграмме
                for (int i = 0; i < сhildrenDHierarchy.Count; i++)
                {
                    // размещает только элементы выбранных уровней
                    ComponentLevel componentLevel = CLHelper.GetComponentLevel(сhildrenDHierarchy[i]);
                    if (!displayLevelsResult.value.Contains(componentLevel)) continue;
                    // Размещаем элемент
                    EA.DiagramObject diagramObject = PutElementOnDiagram(сhildrenDHierarchy[i]);
                    diagramObject.Update();
                }

                CurrentDiagram.DiagramLinks.Refresh();
                Context.LinkDesigner.SetLinkTypeVisibility(LinkType.Deploy, false);
                EARepository.ReloadDiagram(CurrentDiagram.DiagramID);

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }


        /// <summary>
        /// Помещения на текущкю диаграмму выбранных узлов и устройств
        /// </summary>
        /// <returns></returns>
        public ExecResult<Boolean> PutContourContourOnDiagram(string location)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                if (location == "TreeView" || EARepository.GetCurrentDiagram() == null)
                    throw new Exception("Операция разрешена только для диаграммы");


                if (!Context.CheckCurrentDiagram())
                    throw new Exception("Не установлена или не открыта текущая диаграмма");

                // получаем список библиотечных элементов нужного типа
                List<NodeData> nodeDataList = LibraryHelper.GetNodeData(new List<ComponentLevel>() { ComponentLevel.ContourContour, ComponentLevel.SystemContour });

                // показываем список на форме для отмечания
                ExecResult<List<NodeData>> ndSelectresult = new FSelectContourContour().Execute(nodeDataList);
                if (ndSelectresult.code != 0) return result;

                // что на форме наотмечали, помещаем на диаграмму
                foreach (NodeData nodeData in ndSelectresult.value)
                {
                    PutElementOnDiagram(nodeData.Element);
                }

                EARepository.ReloadDiagram(CurrentDiagram.DiagramID);
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        public ExecResult<Boolean> PutElementOnDiagram()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                // Получаем текущий (библиотечный) элемент дерева
                EA.Element curElement = EARepository.GetTreeSelectedObject();
                if (curElement == null || !LibraryHelper.IsLibrary(curElement))
                {
                    throw new Exception("Не выделен библиотечный элемент");
                }

                // Помещаем элемент на диаграмму
                EA.DiagramObject diagramObject = PutElementOnDiagram(curElement);

                CurrentDiagram.DiagramLinks.Refresh();
                Context.LinkDesigner.SetLinkTypeVisibility(LinkType.Deploy, false);
                EARepository.ReloadDiagram(CurrentDiagram.DiagramID);

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        /// <summary>
        /// Размещает на диаграмме укзаанный элемент и иерархию его контейнеров
        /// </summary>
        /// <param name="onlyParent"></param>
        /// <returns></returns>
        public ExecResult<Boolean> PutParentHierarchyOnDiagram(bool onlyParent = false)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            ElementDesignerHelper.CallLevel = 0;

            try
            {
                ExecResult<List<ComponentLevel>> displayLevelsResult = FSelectComponentLevels.Execute();
                if (displayLevelsResult.code != 0) return result;


                // Получаем текущий (библиотечный) элемент дерева 
                EA.Element curElement = EARepository.GetTreeSelectedObject();
                Logger.Out("элемент:", new EA.Element[] { curElement });
                if (curElement == null || !LibraryHelper.IsLibrary(curElement))
                {
                    throw new Exception("Не выделен библиотечный элемент");
                }

                // Получаем цепочку родительских контейнеров
                List<EA.Element> deployments = LibraryHelper.GetParentHierarchy(curElement);
                Logger.Out("цепочка deploy-родителей:", deployments.ToArray());

                // Размещаем на диаграмме элемент
                PutElementOnDiagram(curElement);

                // Проходимся по иерархии и размещаем элементы на диаграмме
                EA.Element prevElement = null;
                for (int i = 0; i < deployments.Count; i++)
                {
                    // размещает только элементы выбранных уровней
                    ComponentLevel componentLevel = CLHelper.GetComponentLevel(deployments[i]);
                    if (!displayLevelsResult.value.Contains(componentLevel)) continue;


                    EA.DiagramObject diagramObject = PutElementOnDiagram(deployments[i]);
                    //diagramObject.Sequence = 1000 - (deployments.Count + i); // Это надо нормально сделать
                    diagramObject.Update();
                    prevElement = deployments[i];
                }

                CurrentDiagram.DiagramLinks.Refresh();
                Context.LinkDesigner.SetLinkTypeVisibility(LinkType.Deploy, false);
                EARepository.ReloadDiagram(CurrentDiagram.DiagramID);

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        /// <summary>
        /// Установка Tags для выделенных элементов
        /// </summary>
        /// <returns></returns>
        public ExecResult<Boolean> SetElementTags(string location)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                // Сначала получаем список выделеннеых библиотечных элементов
                List<EA.Element> selectedLibElements = new List<EA.Element>();

                switch (location)
                {
                    case "TreeView":
                        selectedLibElements = EAHelper.GetSelectedLibElement_Tree();
                        break;
                    case "Diagram":
                        if (!Context.CheckCurrentDiagram())
                            throw new Exception("Не установлена или не открыта текущая диаграмма");

                        selectedLibElements = EAHelper.GetSelectedLibElement_Diagram();
                        break;
                    case "MainMenu":
                        if (CurrentDiagram != null)
                        {
                            if (Context.CheckCurrentDiagram())
                                selectedLibElements = EAHelper.GetSelectedLibElement_Diagram();
                            else
                                selectedLibElements = EAHelper.GetSelectedLibElement_Tree();
                        }
                        else
                        {
                            selectedLibElements = EAHelper.GetSelectedLibElement_Tree();
                        }
                        break;
                }

                if (selectedLibElements.Count == 0) // если не выделены элементы  - пытаемся найти выделенные коннекторы
                {
                    if (location == "Diagram" || location == "MainMenu")
                    {
                        EA.Connector selectedConnector = EAHelper.GetSelectedLibConnector_Diagram();
                        if (selectedConnector != null)
                            return Context.LinkDesigner.SetConnectorTags(location);
                    }
                    else
                    {
                        throw new Exception("Не выделены библиотечные элементы");
                    }
                }


                // Конструируем данные тэгов для формы
                List<TagData> curTagDataList = new List<TagData>();
                foreach (EA.Element curElement in selectedLibElements)
                {
                    foreach (EA.TaggedValue taggedValue in curElement.TaggedValuesEx)
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
                        curTagData.Count++;
                        if (taggedValue.ElementID != curElement.ElementID)
                        {
                            curTagData.Ex = true;
                        }

                    }
                }
                // Открываем форму для установки Tags
                ExecResult<List<TagData>> setTagsResult = new FSetTags().Execute(curTagDataList);
                if (setTagsResult.code != 0) return result;

                // Прописываем в элементах что наустанавливали на форме
                foreach (EA.Element curElement in selectedLibElements)
                {
                    foreach (TagData curTagData in setTagsResult.value)
                    {
                        if (curTagData.Enabled) // записываем только для Tags, в котоорые разрешено
                        {

                            if (curTagData.TagState == false)
                            {
                                EATVHelper.TaggedValueRemove(curElement, curTagData.TagName);
                            }
                            else
                            {
                                EATVHelper.TaggedValueSet(curElement, curTagData.TagName, curTagData.TagValue);
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
        /// Функция добавляет или замещает элемент на диаграмме, если указано - внутри указанного элемента
        /// </summary>
        /// <param name="element"></param>
        /// <param name="parentElement"></param>
        public EA.DiagramObject PutElementOnDiagram(EA.Element element)
        {
            ElementDesignerHelper.CallLevel++;
            EA.DiagramObject elementDA;

            try
            {

                Logger.Out(element.Name);
                Logger.Out("размещаем элемент ", new EA.Element[] { element });

                var diagramObjects = CurrentDiagram.DiagramObjects;

                elementDA = CurrentDiagram.GetDiagramObjectByID(element.ElementID, ""); // Получаем элемент на диаграмме
                if (elementDA == null) // если элемента нет - создаём
                {
                    Logger.Out("элемента нет на диаграмме, создаём ");


                    elementDA = CurrentDiagram.DiagramObjects.AddNew("", "");
                    elementDA.ElementID = element.ElementID;

                    // устанавливаем размер объекта 
                    Size elementSize = ElementDesignerHelper.GetDefaultDASize(elementDA);
                    EADOHelper.ApplySizeToDA(elementDA, elementSize, false);
                    int elementID = elementDA.ElementID;

                    Point newDAPoint = ElementDesignerHelper.GetFirstFreePoinForDA(elementDA);
                    EADOHelper.ApplyPointToDA(elementDA, newDAPoint, false);

                    elementDA.Update();
                    CurrentDiagram.Update();
                    CurrentDiagram.DiagramObjects.Refresh();

                    Logger.Out(element.Name + ":создан элемент ", new EA.DiagramObject[] { elementDA });
                }
                else
                {
                    Logger.Out("элемент уже на диаграмме", new EA.DiagramObject[] { elementDA });
                }

                // Подгоняем ZOrder
                //SetElementZorder(elementDA);

                /*
                // Проверяем наличие на диаграмме (непосредственных) элементов дочерней иерархии
                // Еесли есть такие - вписываем их в текущий элемент
                List<EA.DiagramObject> childDAList = EAHelper.GetNearestChildrenDA(element);
                Logger.Out("получен список присутсвующих на диагр. дочерних ", childDAList.ToArray());
                foreach (var childDA in childDAList)
                {
                    FitElementInElement(childDA, elementDA);
                }


                // Проверяем наличие на диаграмме элемента родительской иерархии
                // При наличии вписываем элемент в него
                List<EA.DiagramObject> parentDAList = EAHelper.GetDeployParentHierarchyDA(element);
                Logger.Out("получен список родительской иерархии на диаграмме", parentDAList.ToArray());
                if (parentDAList.Count > 0)
                {
                    var nearestParentDA = parentDAList[0];
                    FitElementInElement(elementDA, nearestParentDA);
                }
                */
            }
            finally
            {
                ElementDesignerHelper.CallLevel--;
            }

            return elementDA;
        }

        /// <summary>
        /// Устанавливает Zorder для нового элемента
        /// </summary>
        private void SetElementZorder(EA.DiagramObject diagramObject)
        {
            /*
            План такой:
            - на входе считаем, что ZOrder у всех нормальный кроме устанавливаемого

            - есть дерево дочерних (deploy) - обрабатываемый элемент  надо положить "под них"
            - есть прочие элементы, ZOrder которых был больше (ниже) дочерних выше - их всех надо "притопить" на единичку, т.к. под дочерними вставлен обрабатываемый
            - все остальные не трогаем

            */

            EA.Element element = EARepository.GetElementByID(diagramObject.ElementID);

            // Получаем список родителей на диаграммме
            //List<EA.DiagramObject> parentDAList = EAHelper.GetDeployParentHierarchyDA(element);

            // Получаем список детей на диаграмме
            List<EA.DiagramObject> childrenDAList = LibraryHelper.GetDeployChildrenHierarchyDA(element);

            // наличие на диаграмме элементов дочерней иерархии, если есть такие - подкладываем элемент под них
            int elementZOrder = 0;
            foreach (var childDA in childrenDAList)
            {
                if (childDA.Sequence >= elementZOrder)
                    elementZOrder = childDA.Sequence + 1;
            }

            foreach (EA.DiagramObject curDA in CurrentDiagram.DiagramObjects)
            {
                if (curDA.ElementID == diagramObject.ElementID)
                {
                    diagramObject.Sequence = elementZOrder;
                    diagramObject.Update();
                }
                else if (curDA.Sequence >= elementZOrder)
                {
                    curDA.Sequence = curDA.Sequence + 1;
                    curDA.Update();
                }

            }

            foreach (EA.DiagramObject curDA in CurrentDiagram.DiagramObjects)
            {
                Logger.Out("Zorder=" + curDA.Sequence, new EA.DiagramObject[] { curDA });
            }


        }
    }
}
