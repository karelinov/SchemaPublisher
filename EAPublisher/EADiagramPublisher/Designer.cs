using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using EADiagramPublisher.Enums;
using EADiagramPublisher.Forms;

namespace EADiagramPublisher
{
    /// <summary>
    /// Класс для решения задач дизайна схем (бизнес-функции)
    /// </summary>
    public class Designer
    {
        public static int defaultWidth = 100;
        public static int defaultHeight = 50;

        public static String defaultWidthTag = "DP_DefaultWidth";
        public static String defaultHeightTag = "DP_DefaultHeight";

        public static int border = 20;

        public EA.Repository EARepository
        {
            get
            {
                return EAHelper.EARepository;
            }
            set
            {
                EAHelper.EARepository = value;
            }
        }

        public Designer(EA.Repository eaRepository)
        {
            this.EARepository = eaRepository;
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
        /// Идентификатор текущей диаграммы
        /// </summary>
        /// 
        private EA.Diagram _CurrentDiagram { get; set; }
        public EA.Diagram CurrentDiagram
        {
            get
            {
                if (_CurrentDiagram == null)
                {
                    var currentDiagram = EARepository.GetCurrentDiagram();
                    if (currentDiagram == null)
                        throw new Exception("Нет активной диаграммы");
                    else
                        _CurrentDiagram = currentDiagram;
                }



                return _CurrentDiagram;
            }
            set
            {
                _CurrentDiagram = value;
            }
        }

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

                SetConnectorVisibility(ConnectorType.Deploy, false);
                CurrentDiagram.DiagramLinks.Refresh();
                EARepository.ReloadDiagram(CurrentDiagram.DiagramID);

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
        private EA.DiagramObject PutElementOnDiagram(EA.Element element)
        {
            DesignerHelper.CallLevel++;
            EA.DiagramObject elementDA;

            try
            {

                EAHelper.Out(element.Name);
                EAHelper.Out("размещаем элемент ", new EA.Element[] { element });

                var diagramObjects = CurrentDiagram.DiagramObjects;

                elementDA = EAHelper.GetDiagramObject(element); // Получаем элемент на диаграмме
                if (elementDA == null) // если элемента нет - создаём
                {
                    EAHelper.Out("элемента нет на диаграмме, создаём ");

                    elementDA = CurrentDiagram.DiagramObjects.AddNew("", "");
                    elementDA.ElementID = element.ElementID;
                    elementDA.Update();
                    CurrentDiagram.Update();
                    CurrentDiagram.DiagramObjects.Refresh();
                    DesignerHelper.SetDefaultDASize(elementDA);
                    int elementID = elementDA.ElementID;
                    EAHelper.Out(element.Name + ":создан элемент ", new EA.DiagramObject[] { elementDA });
                }
                else
                {
                    EAHelper.Out("элемент уже на диаграмме", new EA.DiagramObject[] { elementDA });
                }

                // Проверяем наличие на диаграмме элементов дочерней иерархии, если есть такие - вписываем их в текущий элемент
                List<EA.Element> childDeployments = EAHelper.GetDeployChildrenDA(element);
                EAHelper.Out(element.Name + ":получен список присутсвующих на диагр. дочерних ", childDeployments.ToArray());


                // Подгоняем ZOrder
                SetElementZorder(elementDA);

                // Проверяем наличие на диаграмме элемента родительской иерархии
                EA.DiagramObject parentElementDA = null;
                List<EA.Element> deployments = EAHelper.GetParentHierarchy(element);
                EAHelper.Out("получен список родительской иерархии ", deployments.ToArray());

 
                bool putInparent = false;
                for (int i = 0; i < deployments.Count - 1; i++) // проходимся по родительской иерархии
                {
                    parentElementDA = EAHelper.GetDiagramObject(deployments[i]);
                    if (parentElementDA != null && !putInparent) // Если на диаграмме есть контейнер из родительской иерархии - вписываем элемент в данный контейнер
                    {
                        FitElementInElement(elementDA, parentElementDA);
                        putInparent = true;
                    }
                }
            }
            finally
            {
                DesignerHelper.CallLevel--;
            }

            return elementDA;
        }

        /// <summary>
        /// Функция при необходимости увеличиывает размеры родительского элемента, чтобы он вместил дочерний 
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

                // Сначала получаем список тех элементов, которые находятся внутри дочернего, чтобы при подвижке дочернего они из него не вывалились
                List<EA.DiagramObject> elementsInsideChild = EAHelper.GetChildrenDA(childElementDA);
                EAHelper.Out("Получен список вставленных в дочерний элемент ", elementsInsideChild.ToArray());

                // также получаем список непосредственно дочерних элементов родителя - чтобы понять, как лежат другие дочерние - конкуренты вписываемого
                List<EA.DiagramObject> elementsInsideParent = EAHelper.GetChildrenDA(parentElementDA);
                EAHelper.Out("Получен список дочерних элементов родителя ", elementsInsideParent.ToArray());

                // Теперь делаем размеры объектов не диаграмме ненулевыми (а они такие могут быть)
                // Устанавливаем либо дефолтные размеры из тэгов либо (если там нет) просто дефолтные
                Size childSize = DesignerHelper.SetDefaultDASize(childElementDA);
                EAHelper.Out("Определены начальные координаты child ", new EA.DiagramObject[] { childElementDA });

                Size parentSize = DesignerHelper.SetDefaultDASize(parentElementDA);
                EAHelper.Out("Определены начальные координаты parent ", new EA.DiagramObject[] { parentElementDA });

                // Определяем место на родителе для размещения child
                // место для вписываемого элемента - справа от правого/снизу от нижнего/ справа от нижнего - выбирается минимальная сумма координат
                Point childStart;
                ExecResult<Point> childStartResult = DesignerHelper.GetPointForChild(parentElementDA, childElementDA, elementsInsideParent);
                if (childStartResult.code == 0)
                {
                    childStart = childStartResult.value;  // В parentDA найдено место для childDA
                }
                else // если нет места на parentDA, рассчитываем, как его надо расширить
                {
                    Tuple<Size, Point> expandDAdata = DesignerHelper.ExpandedDASizeForChild(parentElementDA, childElementDA, elementsInsideParent);
                    DesignerHelper.ApplySizeToDA(parentElementDA, expandDAdata.Item1);
                    EAHelper.Out("Определены и установлены новые координаты parent ", new EA.DiagramObject[] { parentElementDA });
                    childStart = expandDAdata.Item2;
                    // корректируем позицию child с учётом подвижки родителя
                    childStart.X = parentElementDA.left + childStart.X;
                    childStart.Y = parentElementDA.top + childStart.Y;
                }
                EAHelper.Out("Вычислены координаты childDA ", new Object[] { childStart });

                // теперь подвигаем дочерний внутрь родительского
                childElementDA.left = childStart.X;// parentElementDA.left + border;
                childElementDA.right = childElementDA.left + childSize.Width;
                childElementDA.top = childStart.Y; // parentElementDA.top - border*2;
                childElementDA.bottom = childElementDA.top - childSize.Height;
                childElementDA.Update();
                EAHelper.Out("установлены новые координаты child ", new EA.DiagramObject[] { childElementDA });


                // теперь вслед за дочерним, подвигаем элементы, которые были внутри дочернего
                if (elementsInsideChild.Count > 0)
                {
                    EAHelper.Out("инициируется подвижка вложенных в child...");
                    foreach (var elementInsideChild in elementsInsideChild)
                    {
                        FitElementInElement(elementInsideChild, childElementDA);
                    }
                }

            }
            finally
            {
                DesignerHelper.CallLevel--;
            }

        }

        /// <summary>
        /// Устанавливает видимость указанного типо коннекторов
        /// </summary>
        private void SetConnectorVisibility(ConnectorType connectorType, bool visibility = true)
        {
            foreach (EA.DiagramLink diagramLink in CurrentDiagram.DiagramLinks)
            {

                EA.Connector connector = EARepository.GetConnectorByID(diagramLink.ConnectorID);

                switch (connectorType)
                {
                    case ConnectorType.Deploy:
                        if (connector.Type == "Dependency" && connector.Stereotype == "deploy")
                        {
                            EAHelper.setConnectorVisibility(diagramLink, visibility);
                        }
                        break;


                }
            }
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
            foreach(var childDA in childrenDAList)
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


    }
}
