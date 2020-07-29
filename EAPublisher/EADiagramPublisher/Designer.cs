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
    public class Designer
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
        public ExecResult<Boolean> PutElementOnDiagram()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                // Получаем текущий (библиотечный) элемент дерева
                EA.Element curElement = EARepository.GetTreeSelectedObject();
                if (curElement == null || !EAHelper.IsLibrary(curElement))
                {
                    throw new Exception("Не выделен библиотечный элемент");
                }

                // Помещаем элемент на диаграмму
                EA.DiagramObject diagramObject = PutElementOnDiagram(curElement);

                CurrentDiagram.DiagramLinks.Refresh();
                DPAddin.LinkDesigner.SetLinkTypeVisibility(LinkType.Deploy, false);
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
            DesignerHelper.CallLevel = 0;

            try
            {
                ExecResult<List<ComponentLevel>> displayLevelsResult = new FSelectHierarcyLevels().Execute();
                if (displayLevelsResult.code != 0) return result;


                // Получаем текущий (библиотечный) элемент дерева 
                EA.Element curElement = null;
                if (EARepository.GetTreeSelectedElements().Count > 0) curElement = EARepository.GetTreeSelectedElements().GetAt(0);
                EAHelper.Out("элемент:", new EA.Element[] { curElement });
                if (curElement == null || !EAHelper.IsLibrary(curElement))
                {
                    throw new Exception("Не выделен библиотечный элемент");
                }

                // Получаем список дочерних элементов контейнеров
                List<EA.Element> сhildrenDHierarchy = EAHelper.GetChildHierarchy(curElement);


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
                DPAddin.LinkDesigner.SetLinkTypeVisibility(LinkType.Deploy, false);
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
        public ExecResult<Boolean> PutChildrenDHierarchyOnElement()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            DesignerHelper.CallLevel = 0;

            try
            {
                ExecResult<List<ComponentLevel>> displayLevelsResult = new FSelectHierarcyLevels().Execute();
                if (displayLevelsResult.code != 0) return result;


                // Получаем текущий (библиотечный) элемент дерева 
                if (CurrentDiagram.SelectedObjects.Count == 0 || !EAHelper.IsLibrary(EARepository.GetElementByID(((EA.DiagramObject)CurrentDiagram.SelectedObjects.GetAt(0)).ElementID)))
                    throw new Exception("Не выделен библиотечный элемент на диаграмме");

                EA.Element curElement = EARepository.GetElementByID(((EA.DiagramObject)CurrentDiagram.SelectedObjects.GetAt(0)).ElementID);

                // Получаем список дочерних элементов контейнеров
                List<EA.Element> сhildrenDHierarchy = EAHelper.GetChildHierarchy(curElement);

                // Проходимся по иерархии и размещаем элементы на диаграмме
                for (int i = 0; i < сhildrenDHierarchy.Count; i++)
                {
                    // Размещаем элемент
                    EA.DiagramObject diagramObject = PutElementOnDiagram(сhildrenDHierarchy[i]);
                    diagramObject.Update();
                }

                CurrentDiagram.DiagramLinks.Refresh();
                DPAddin.LinkDesigner.SetLinkTypeVisibility(LinkType.Deploy, false);
                EARepository.ReloadDiagram(CurrentDiagram.DiagramID);

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        /// <summary>
        /// Размещает на диаграмме под элементом дерево компонентов, размещённых в нём
        /// </summary>
        /// <param name="onlyParent"></param>
        /// <returns></returns>
        public ExecResult<Boolean> PutChildrenDeployHierarchy(string location)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            DesignerHelper.CallLevel = 0;

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
                DPTreeNode<EA.Element> сhildrenDHierarchy = EAHelper.GetDeployChildrenHierarchy(selectedElement);

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
                        EAHelper.ApplyPointToDA(curDA, new Point(curDA.right + DAConst.border, levelStartPoint.Y));
                        int newLevelRight = curDA.right;
                        int newLevelBottom = curDA.bottom < levelEndPoint.Y ? curDA.bottom : levelEndPoint.Y;
                        levelEndPoint = new Point(newLevelRight, newLevelBottom);
                    }

                    // коллекуионируем список узлов уровнем ниже
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
        public ExecResult<Boolean> PutParentHierarchyOnDiagram(bool onlyParent = false)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            DesignerHelper.CallLevel = 0;

            try
            {
                ExecResult<List<ComponentLevel>> displayLevelsResult = new FSelectHierarcyLevels().Execute();
                if (displayLevelsResult.code != 0) return result;


                // Получаем текущий (библиотечный) элемент дерева 
                EA.Element curElement = EARepository.GetTreeSelectedObject();
                EAHelper.Out("элемент:", new EA.Element[] { curElement });
                if (curElement == null || !EAHelper.IsLibrary(curElement))
                {
                    throw new Exception("Не выделен библиотечный элемент");
                }

                // Получаем цепочку родительских контейнеров
                List<EA.Element> deployments = EAHelper.GetParentHierarchy(curElement);
                EAHelper.Out("цепочка deploy-родителей:", deployments.ToArray());

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
                DPAddin.LinkDesigner.SetLinkTypeVisibility(LinkType.Deploy, false);
                EARepository.ReloadDiagram(CurrentDiagram.DiagramID);

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        /// <summary>
        /// Функция при необходимости увеличиывает размеры родительского элемента, чтобы он вместил дочерний. 
        /// Дочерний элемент помещается на родительский (координатами)
        /// </summary>
        /// <param name="childElementDA"></param>
        /// <param name="parentElementDA"></param>
        private void FitElementInElement(EA.DiagramObject childElementDA, EA.DiagramObject parentElementDA)
        {
            DesignerHelper.CallLevel++;
            try
            {
                EAHelper.Out("", new EA.DiagramObject[] { childElementDA, parentElementDA });

                // получаем элемент, в котором находится родитель - чтобы при изменении размеров родителя перевписать его в "прародителя"
                EA.DiagramObject grandparentDA = EAHelper.GetContainerOfDA(parentElementDA);
                EAHelper.Out("Получен элемент прародителя ", new EA.DiagramObject[] { grandparentDA });

                // получаем список тех элементов, которые находятся внутри дочернего, чтобы при подвижке дочернего они из него не вывалились
                List<EA.DiagramObject> elementsInsideChild = EAHelper.GetContainedForDA(childElementDA);
                EAHelper.Out("Получен список вставленных в дочерний элемент ", elementsInsideChild.ToArray());

                // сохраняем начальную позицию child, чтобы когда он подвинется, подвинуть за ним вписанных в него
                Point originalchildPoint = new Point(childElementDA.left, childElementDA.top);

                // также получаем список непосредственно дочерних элементов родителя - чтобы понять, как лежат другие дочерние - конкуренты вписываемого
                List<EA.DiagramObject> elementsInsideParent = EAHelper.GetContainedForDA(parentElementDA);
                EAHelper.Out("Получен список вставленных в родительский элемент ", elementsInsideParent.ToArray());

                // Теперь делаем размеры объектов не диаграмме ненулевыми (а они такие могут быть)
                // Устанавливаем либо дефолтные размеры из тэгов либо (если там нет) просто дефолтные
                Size childSize = DesignerHelper.GetDefaultDASize(childElementDA);
                EAHelper.ApplySizeToDA(childElementDA, childSize);
                EAHelper.Out("Определены начальные координаты child ", new EA.DiagramObject[] { childElementDA });

                Point originalParentPoint = new Point(parentElementDA.left, parentElementDA.top);
                Size originalParentSize = DesignerHelper.GetSize(parentElementDA);
                Size parentSize = DesignerHelper.GetDefaultDASize(parentElementDA);
                EAHelper.ApplySizeToDA(parentElementDA, parentSize);
                EAHelper.Out("Определены начальные координаты parent ", new EA.DiagramObject[] { parentElementDA });

                // Определяем место на родителе для размещения child
                Point childStart;
                ExecResult<Point> childStartResult = DesignerHelper.GetPointForChild(parentElementDA, childElementDA, elementsInsideParent);
                if (childStartResult.code == 0)
                {
                    childStart = childStartResult.value;  // В parentDA найдено место для childDA
                    EAHelper.Out("Определены координаты для расмещения child ", new object[] { childStartResult.value });
                }
                else // если нет места на parentDA, рассчитываем, как его надо расширить
                {
                    Tuple<Size, Point> expandDAdata = DesignerHelper.GetExpandedDASizeForChild(parentElementDA, childElementDA, elementsInsideParent);
                    EAHelper.ApplySizeToDA(parentElementDA, expandDAdata.Item1); // расширяем родителя
                    childStart = new Point(expandDAdata.Item2.X + parentElementDA.left, expandDAdata.Item2.Y + parentElementDA.top); // координаты child корректируются с учётом позиции родителя, т.к. возвращены координты внутри родителя
                    EAHelper.Out("Определены и установлены новые координаты parent ", new EA.DiagramObject[] { parentElementDA });
                    EAHelper.Out("Определены координаты для расмещения child ", new object[] { childStart });
                }

                // Если координаты или размер parentElementDA изменились, его надо перевписать в его родителя 
                if ((!originalParentPoint.Equals(new Point(parentElementDA.left, parentElementDA.top))) || (!originalParentSize.Equals(DesignerHelper.GetSize(parentElementDA))))
                {
                    if (grandparentDA != null)
                    {
                        EAHelper.Out("Изменились размер или положение родительского DA, всписываем его в grandparent... ", new EA.DiagramObject[] { childElementDA });
                        Point newOriginalParentPoint = new Point(parentElementDA.left, parentElementDA.top);
                        FitElementInElement(parentElementDA, grandparentDA);

                        // После вписывания parent-а следует скорректировать позицию вписываемого элемента
                        Point moveParentVector = DesignerHelper.GetVector(newOriginalParentPoint, new Point(parentElementDA.left, parentElementDA.top));
                        childStart = new Point(childStart.X + moveParentVector.X, childStart.Y + moveParentVector.Y);
                        EAHelper.Out("Скорректирована позиция child ", new object[] { childStart });
                    }
                }

                // теперь подвигаем дочерний внутрь родительского
                EAHelper.ApplyPointToDA(childElementDA, childStart);
                /*
                childElementDA.left = childStart.X;// parentElementDA.left + border;
                childElementDA.right = childElementDA.left + childSize.Width;
                childElementDA.top = childStart.Y; // parentElementDA.top - border*2;
                childElementDA.bottom = childElementDA.top - childSize.Height;
                childElementDA.Update();
                */
                EAHelper.Out("установлены новые координаты child ", new EA.DiagramObject[] { childElementDA });

                // теперь вслед за дочерним, подвигаем элементы, которые были внутри дочернего
                Point moveVector = DesignerHelper.GetVector(originalchildPoint, childStart);
                if (elementsInsideChild.Count > 0)
                {
                    EAHelper.Out("инициируется подвижка вложенных в child...");
                    foreach (var elementInsideChild in elementsInsideChild)
                    {
                        EAHelper.MoveContainedHierarchy(elementInsideChild, moveVector);
                    }
                }




            }
            finally
            {
                DesignerHelper.CallLevel--;
            }

        }

        /// <summary>
        /// Функция добавляет или замещает элемент на диаграмме, если указано - внутри указанного элемента
        /// </summary>
        /// <param name="element"></param>
        /// <param name="parentElement"></param>
        private EA.DiagramObject PutElementOnDiagram(EA.Element element)
        {
            DesignerHelper.CallLevel++;
            EA.DiagramObject elementDA;

            try
            {

                EAHelper.Out(element.Name);
                EAHelper.Out("размещаем элемент ", new EA.Element[] { element });

                var diagramObjects = CurrentDiagram.DiagramObjects;

                elementDA = CurrentDiagram.GetDiagramObjectByID(element.ElementID, ""); // Получаем элемент на диаграмме
                if (elementDA == null) // если элемента нет - создаём
                {
                    EAHelper.Out("элемента нет на диаграмме, создаём ");


                    elementDA = CurrentDiagram.DiagramObjects.AddNew("", "");
                    elementDA.ElementID = element.ElementID;

                    // устанавливаем размер объекта 
                    Size elementSize = DesignerHelper.GetDefaultDASize(elementDA);
                    EAHelper.ApplySizeToDA(elementDA, elementSize, false);
                    int elementID = elementDA.ElementID;

                    Point newDAPoint = DesignerHelper.GetFirstFreePoinForDA(elementDA);
                    EAHelper.ApplyPointToDA(elementDA, newDAPoint, false);

                    elementDA.Update();
                    CurrentDiagram.Update();
                    CurrentDiagram.DiagramObjects.Refresh();

                    EAHelper.Out(element.Name + ":создан элемент ", new EA.DiagramObject[] { elementDA });
                }
                else
                {
                    EAHelper.Out("элемент уже на диаграмме", new EA.DiagramObject[] { elementDA });
                }

                // Подгоняем ZOrder
                //SetElementZorder(elementDA);

                /*
                // Проверяем наличие на диаграмме (непосредственных) элементов дочерней иерархии
                // Еесли есть такие - вписываем их в текущий элемент
                List<EA.DiagramObject> childDAList = EAHelper.GetNearestChildrenDA(element);
                EAHelper.Out("получен список присутсвующих на диагр. дочерних ", childDAList.ToArray());
                foreach (var childDA in childDAList)
                {
                    FitElementInElement(childDA, elementDA);
                }


                // Проверяем наличие на диаграмме элемента родительской иерархии
                // При наличии вписываем элемент в него
                List<EA.DiagramObject> parentDAList = EAHelper.GetDeployParentHierarchyDA(element);
                EAHelper.Out("получен список родительской иерархии на диаграмме", parentDAList.ToArray());
                if (parentDAList.Count > 0)
                {
                    var nearestParentDA = parentDAList[0];
                    FitElementInElement(elementDA, nearestParentDA);
                }
                */
            }
            finally
            {
                DesignerHelper.CallLevel--;
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
            List<EA.DiagramObject> childrenDAList = EAHelper.GetDeployChildrenHierarchyDA(element);

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
                EAHelper.Out("Zorder=" + curDA.Sequence, new EA.DiagramObject[] { curDA });
            }


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
                        if (!EAHelper.CheckCurrentDiagram())
                            throw new Exception("Не установлена или не открыта текущая диаграмма");

                        selectedLibElements = EAHelper.GetSelectedLibElement_Diagram();
                        break;
                    case "MainMenu":
                        if (CurrentDiagram != null)
                        {
                            if (EAHelper.CheckCurrentDiagram())
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
                            return DPAddin.LinkDesigner.SetConnectorTags(location);
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
                                EAHelper.TaggedValueRemove(curElement, curTagData.TagName);
                            }
                            else
                            {
                                EAHelper.TaggedValueSet(curElement, curTagData.TagName, curTagData.TagValue);
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
        /// Помещения на текущкю диаграмму выбранных узлов и устройств
        /// </summary>
        /// <returns></returns>
        public ExecResult<Boolean> PutNodesDevicesOnDiagram(string location)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                if (!EAHelper.CheckCurrentDiagram())
                    throw new Exception("Не установлена или не открыта текущая диаграмма");

                // получаем список библиотечных элементов нужного типа
                List<NodeData> nodeDataList = LibraryHelper.GetNodeData(new List<ComponentLevel>() { ComponentLevel.Device, ComponentLevel.Node });

                // показываем список на форме для отмечания
                ExecResult<List<NodeData>> ndSelectresult = new FSelectNodesAndDevices().Execute(nodeDataList);
                if (ndSelectresult.code != 0) return result;

                // что на форме наотмечали, помещаем на диаграмму
                foreach(NodeData nodeData in ndSelectresult.value)
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


    }
}
