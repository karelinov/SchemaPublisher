using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using EADiagramPublisher.Enums;
using EADiagramPublisher.Forms;
using EADiagramPublisher.Contracts;

namespace EADiagramPublisher.Archive
{
    class Designer_Archive
    {
        /// <summary>
        /// Функция при необходимости увеличиывает размеры родительского элемента, чтобы он вместил дочерний. 
        /// Дочерний элемент помещается на родительский (координатами)
        /// </summary>
        /// <param name="childElementDA"></param>
        /// <param name="parentElementDA"></param>
        private void FitElementInElement(EA.DiagramObject childElementDA, EA.DiagramObject parentElementDA)
        {
            ElementDesignerHelper.CallLevel++;
            try
            {
                Logger.Out("", new EA.DiagramObject[] { childElementDA, parentElementDA });

                // получаем элемент, в котором находится родитель - чтобы при изменении размеров родителя перевписать его в "прародителя"
                EA.DiagramObject grandparentDA = EADOHelper.GetContainerOfDA(parentElementDA);
                Logger.Out("Получен элемент прародителя ", new EA.DiagramObject[] { grandparentDA });

                // получаем список тех элементов, которые находятся внутри дочернего, чтобы при подвижке дочернего они из него не вывалились
                List<EA.DiagramObject> elementsInsideChild = EADOHelper.GetContainedForDA(childElementDA);
                Logger.Out("Получен список вставленных в дочерний элемент ", elementsInsideChild.ToArray());

                // сохраняем начальную позицию child, чтобы когда он подвинется, подвинуть за ним вписанных в него
                Point originalchildPoint = new Point(childElementDA.left, childElementDA.top);

                // также получаем список непосредственно дочерних элементов родителя - чтобы понять, как лежат другие дочерние - конкуренты вписываемого
                List<EA.DiagramObject> elementsInsideParent = EADOHelper.GetContainedForDA(parentElementDA);
                Logger.Out("Получен список вставленных в родительский элемент ", elementsInsideParent.ToArray());

                // Теперь делаем размеры объектов не диаграмме ненулевыми (а они такие могут быть)
                // Устанавливаем либо дефолтные размеры из тэгов либо (если там нет) просто дефолтные
                Size childSize = ElementDesignerHelper.GetDefaultDASize(childElementDA);
                EADOHelper.ApplySizeToDA(childElementDA, childSize);
                Logger.Out("Определены начальные координаты child ", new EA.DiagramObject[] { childElementDA });

                Point originalParentPoint = new Point(parentElementDA.left, parentElementDA.top);
                Size originalParentSize = ElementDesignerHelper.GetSize(parentElementDA);
                Size parentSize = ElementDesignerHelper.GetDefaultDASize(parentElementDA);
                EADOHelper.ApplySizeToDA(parentElementDA, parentSize);
                Logger.Out("Определены начальные координаты parent ", new EA.DiagramObject[] { parentElementDA });

                // Определяем место на родителе для размещения child
                Point childStart;
                ExecResult<Point> childStartResult = ElementDesignerHelper.GetPointForChild(parentElementDA, childElementDA, elementsInsideParent);
                if (childStartResult.code == 0)
                {
                    childStart = childStartResult.value;  // В parentDA найдено место для childDA
                    Logger.Out("Определены координаты для расмещения child ", new object[] { childStartResult.value });
                }
                else // если нет места на parentDA, рассчитываем, как его надо расширить
                {
                    Tuple<Size, Point> expandDAdata = ElementDesignerHelper.GetExpandedDASizeForChild(parentElementDA, childElementDA, elementsInsideParent);
                    EADOHelper.ApplySizeToDA(parentElementDA, expandDAdata.Item1); // расширяем родителя
                    childStart = new Point(expandDAdata.Item2.X + parentElementDA.left, expandDAdata.Item2.Y + parentElementDA.top); // координаты child корректируются с учётом позиции родителя, т.к. возвращены координты внутри родителя
                    Logger.Out("Определены и установлены новые координаты parent ", new EA.DiagramObject[] { parentElementDA });
                    Logger.Out("Определены координаты для расмещения child ", new object[] { childStart });
                }

                // Если координаты или размер parentElementDA изменились, его надо перевписать в его родителя 
                if ((!originalParentPoint.Equals(new Point(parentElementDA.left, parentElementDA.top))) || (!originalParentSize.Equals(ElementDesignerHelper.GetSize(parentElementDA))))
                {
                    if (grandparentDA != null)
                    {
                        Logger.Out("Изменились размер или положение родительского DA, всписываем его в grandparent... ", new EA.DiagramObject[] { childElementDA });
                        Point newOriginalParentPoint = new Point(parentElementDA.left, parentElementDA.top);
                        FitElementInElement(parentElementDA, grandparentDA);

                        // После вписывания parent-а следует скорректировать позицию вписываемого элемента
                        Point moveParentVector = ElementDesignerHelper.GetVector(newOriginalParentPoint, new Point(parentElementDA.left, parentElementDA.top));
                        childStart = new Point(childStart.X + moveParentVector.X, childStart.Y + moveParentVector.Y);
                        Logger.Out("Скорректирована позиция child ", new object[] { childStart });
                    }
                }

                // теперь подвигаем дочерний внутрь родительского
                EADOHelper.ApplyPointToDA(childElementDA, childStart);
                /*
                childElementDA.left = childStart.X;// parentElementDA.left + border;
                childElementDA.right = childElementDA.left + childSize.Width;
                childElementDA.top = childStart.Y; // parentElementDA.top - border*2;
                childElementDA.bottom = childElementDA.top - childSize.Height;
                childElementDA.Update();
                */
                Logger.Out("установлены новые координаты child ", new EA.DiagramObject[] { childElementDA });

                // теперь вслед за дочерним, подвигаем элементы, которые были внутри дочернего
                Point moveVector = ElementDesignerHelper.GetVector(originalchildPoint, childStart);
                if (elementsInsideChild.Count > 0)
                {
                    Logger.Out("инициируется подвижка вложенных в child...");
                    foreach (var elementInsideChild in elementsInsideChild)
                    {
                        EAHelper_Archive.MoveContainedHierarchy(elementInsideChild, moveVector);
                    }
                }




            }
            finally
            {
                ElementDesignerHelper.CallLevel--;
            }

        }

        /// <summary>
        /// Размещает на диаграмме укзаанный элемент и иерархию его контейнеров
        /// </summary>
        /// <param name="onlyParent"></param>
        /// <returns></returns>
        public ExecResult<Boolean> PutChildrenDHierarchyOnElement()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            ElementDesignerHelper.CallLevel = 0;

            try
            {
                ExecResult<List<ComponentLevel>> displayLevelsResult = new FSelectHierarcyLevels().Execute();
                if (displayLevelsResult.code != 0) return result;


                // Получаем текущий (библиотечный) элемент дерева 
                if (Context.CurrentDiagram.SelectedObjects.Count == 0 || !LibraryHelper.IsLibrary(Context.EARepository.GetElementByID(((EA.DiagramObject)Context.CurrentDiagram.SelectedObjects.GetAt(0)).ElementID)))
                    throw new Exception("Не выделен библиотечный элемент на диаграмме");

                EA.Element curElement = Context.EARepository.GetElementByID(((EA.DiagramObject)Context.CurrentDiagram.SelectedObjects.GetAt(0)).ElementID);

                // Получаем список дочерних элементов контейнеров
                List<EA.Element> сhildrenDHierarchy = LibraryHelper.GetChildHierarchy(curElement);

                // Проходимся по иерархии и размещаем элементы на диаграмме
                for (int i = 0; i < сhildrenDHierarchy.Count; i++)
                {
                    // Размещаем элемент
                    EA.DiagramObject diagramObject = Context.Designer.PutElementOnDiagram(сhildrenDHierarchy[i]);
                    diagramObject.Update();
                }

                Context.CurrentDiagram.DiagramLinks.Refresh();
                Context.LinkDesigner.SetLinkTypeVisibility(LinkType.Deploy, false);
                Context.EARepository.ReloadDiagram(Context.CurrentDiagram.DiagramID);

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
                if (!Context.CheckCurrentDiagram())
                    throw new Exception("Не установлена или не открыта текущая диаграмма");

                // получаем список библиотечных элементов нужного типа
                List<NodeData> nodeDataList = LibraryHelper.GetNodeData(new List<ComponentLevel>() { ComponentLevel.Device, ComponentLevel.Node });

                // показываем список на форме для отмечания
                ExecResult<List<NodeData>> ndSelectresult = new FSelectNodesAndDevices().Execute(nodeDataList);
                if (ndSelectresult.code != 0) return result;

                // что на форме наотмечали, помещаем на диаграмму
                foreach (NodeData nodeData in ndSelectresult.value)
                {
                    Context.Designer.PutElementOnDiagram(nodeData.Element);
                }

                Context.EARepository.ReloadDiagram(Context.CurrentDiagram.DiagramID);
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }


    }
}
