using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using EADiagramPublisher.Enums;

namespace EADiagramPublisher
{
    /// <summary>
    /// Хэлпер для функций дизайна схем.
    /// Здесь помещаются вспомогательные функции
    /// </summary>

    class DesignerHelper
    {
        public static int CallLevel { get; set; }

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
        /// Вспомогательная функция, определяет, помещаютcя ли координаты innerDA внутри outerDA
        /// </summary>
        /// <param name="outerDA"></param>
        /// <param name="innerDA"></param>
        /// <returns></returns>
        public static bool DAFitInside(EA.DiagramObject innerDA, EA.DiagramObject outerDA)
        {
            bool result = false;

            if (outerDA.top > innerDA.top && outerDA.bottom < innerDA.bottom && outerDA.left < innerDA.left && outerDA.right > innerDA.right)
                result = true;

            return result;
        }

        /// <summary>
        /// Функция вычисляет размеры элемента по умолчанию, если они нулевые
        /// Если есть настройки размера в элементе - используются настройки в элементе
        /// Если нет - используются общие дефолтные константы
        /// </summary>
        /// <param name="da"></param>
        /// <returns>размер объекта</returns>
        public static Size GetDefaultDASize(EA.DiagramObject diagramObject)
        {
            Size result = new Size(diagramObject.right - diagramObject.left, diagramObject.top - diagramObject.bottom);
            EA.Element element = EARepository.GetElementByID(diagramObject.ElementID);

            // Для начала пытаемся установить размер из тэгов
            if (result.Width == 0 && EAHelper.GetTaggedValues(element).GetByName(DAConst.defaultWidthTag) != null)
            {
                result.Width = int.Parse(EAHelper.GetTaggedValues(element).GetByName(DAConst.defaultWidthTag).Value);
            }
            if (result.Height == 0 && EAHelper.GetTaggedValues(element).GetByName(DAConst.defaultHeightTag) != null)
            {
                result.Height = int.Parse(EAHelper.GetTaggedValues(element).GetByName(DAConst.defaultHeightTag).Value);
            }

            // Если из тэгов не установили, пытаемся вычислить на лету по библиотечной диаграмме
            if (result.Height == 0 || result.Width == 0)
            {
                ExecResult<Size> GetElementSizeOnLibDiagramResult = EAHelper.GetElementSizeOnLibDiagram(element);
                if (GetElementSizeOnLibDiagramResult.code == 0)
                {
                    result = GetElementSizeOnLibDiagramResult.value;
                }
            }

            // Ну если совсем ничего, то просто ставим цифры по умолчанию
            if (result.Height == 0)
                result.Height = DAConst.defaultHeight;
            if (result.Width == 0)
                result.Width = DAConst.defaultWidth;

            return result;
        }

        /// <summary>
        /// Функция вычисляет требуемое расширение родительского элемента для вписывания в него childDA
        /// </summary>
        /// <param name="parentDA"></param>
        /// <param name="childDA"></param>
        /// <returns>Новый размер parentDA, позиция для child-а внутри parentDA</returns>
        public static Tuple<Size, Point> GetExpandedDASizeForChild(EA.DiagramObject parentDA, EA.DiagramObject childDA, List<EA.DiagramObject> childDAList)
        {
            Point resultPoint = new Point();


            Size parentSize = GetSize(parentDA);
            Size childSize = GetSize(childDA);

            // получаем точку, от которой в элементе начинается свободное место
            var childDAList1 = childDAList.Where(da => da.ElementID != childDA.ElementID); // убираем из списка "внтури родительского" дочерний
            Point parentDAFreeBorders = GetDAFreeBorders(parentDA, childDAList); // получаем точку начала свободного места


            // далее упрощённо - если ширина больше высоты - увеличиваем высоту
            // иначе - увеличиваем ширину
            if (parentSize.Width > parentSize.Height)
            {
                // Увеличиваем parentDA вниз
                parentSize.Height = parentSize.Height + childSize.Height + 3 * DAConst.border - (parentSize.Height - Math.Abs(parentDAFreeBorders.Y));
                if (parentSize.Width < (childSize.Width + 2 * DAConst.border)) parentSize.Width = childSize.Width + 2 * DAConst.border;

                // размещение childDA = у левой и нижней границы
                resultPoint.X = DAConst.border;
                resultPoint.Y = -parentSize.Height + DAConst.border + childSize.Height;
            }
            else
            {
                // Увеличиваем parentDA вправо
                parentSize.Width = parentSize.Width + childSize.Width + 2 * DAConst.border - (parentSize.Width - parentDAFreeBorders.X);
                if (parentSize.Height < (childSize.Height + 3 * DAConst.border)) parentSize.Height = childSize.Height + 3 * DAConst.border;

                // размещение childDA = у правой верхней границы
                resultPoint.X = parentSize.Width - DAConst.border - childSize.Width;
                resultPoint.Y = -2*DAConst.border;
            }

            return new Tuple<Size, Point>(parentSize, resultPoint);
        }

        /// <summary>
        /// Функция возвращает свободное место на диаграмме для размещения DA
        /// Точку в левом / правом верхнем углу
        /// </summary>
        /// <returns></returns>
        public static Point GetFirstFreePoinForDA(EA.DiagramObject diagramObject)
        {
            Size daSize = GetSize(diagramObject);

            Point result = new Point(DAConst.border, -DAConst.border); // начальная точка - возле левого верхнего угла
            Point result1 = new Point(DAConst.border, -DAConst.border);


            // Проходимся по элементам, одновременно решая, пересекается ли элемент с другими 
            // и вычисляя минимальную правую незанятую точку на тот случай если пересекается
            bool intersects = false;

            foreach (EA.DiagramObject curDA in Context.CurrentDiagram.DiagramObjects)
            {
                if (curDA.ElementID == diagramObject.ElementID) continue; // вообще то он ещё не проапдейчен и его нет, но на всякий случай....

                if (!intersects && Intersects(diagramObject, curDA))
                    intersects = true;

                if ((curDA.right + DAConst.border) >= result1.X)
                    result1.X = curDA.right + DAConst.border;
            }


            if (!intersects)
                return result;
            else
                return result1;
        }

        /// <summary>
        /// Функция ищет оптимальное место для размещения внутри родительского контейнера
        /// </summary>
        /// <param name="parentDA"></param> 
        /// <param name="childDA"></param>
        /// <param name="childDAList">список других элементов внутри родительского, чтобы не наступить на один из них</param>
        /// <returns></returns>
        public static ExecResult<Point> GetPointForChild(EA.DiagramObject parentDA, EA.DiagramObject childDA, List<EA.DiagramObject> childDAList)
        {
            ExecResult<Point> result = new ExecResult<Point>() { code = -1 };

            // Сначала просто пытаемся поместить childDA возле левого верхнего угла
            Rectangle requiredRectangle = new Rectangle(parentDA.left + DAConst.border, parentDA.top - 2 * DAConst.border, childDA.right - childDA.left, childDA.top - childDA.bottom);
            bool intersectsWithOther = false;
            if (Contain(parentDA, requiredRectangle))
            {
                foreach (var otherChildDA in childDAList)
                {
                    if (otherChildDA.ElementID == childDA.ElementID) continue;
                    if (Intersects(otherChildDA, requiredRectangle))
                    {
                        intersectsWithOther = true;
                        break;
                    }
                }
                if (!intersectsWithOther)
                {
                    result.value = new Point(requiredRectangle.X, requiredRectangle.Y);
                    result.code = 0;
                    return result;
                }
            }

            // если мы здесь, значит тупо вписаться не удалось, начинаем шарить вокруг вписанных в родительский элементов, пытаясь найти место
            foreach (var otherChildDA in childDAList)
            {
                if (otherChildDA.ElementID == childDA.ElementID) continue;

                Point point = FindSpaceAround(childDA, otherChildDA, parentDA, childDAList);
                if (!point.IsEmpty)
                {
                    result.value = point;
                    result.code = 0;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Вспомогательная функция получения Size DA
        /// </summary>
        /// <param name="diagramObject"></param>
        /// <returns></returns>
        public static Size GetSize(EA.DiagramObject diagramObject)
        {
            return new Size()
            {
                Width = diagramObject.right - diagramObject.left,
                Height = diagramObject.top - diagramObject.bottom
            };
        }

        /// <summary>
        /// Вспомогательная функция вычисления смещения от одной точки к второй
        /// </summary>
        /// <param name="fromPoint"></param>
        /// <param name="toPoint"></param>
        /// <returns></returns>
        public static Point GetVector(Point fromPoint, Point toPoint)
        {
            return new Point()
            {
                X = toPoint.X - fromPoint.X,
                Y = toPoint.Y - fromPoint.Y,
            };
        }


        /// <summary>
        /// Функция определяет, находится ли второй прямоугольник (полностью) внутри первого
        /// </summary>
        /// <param name="firstRectangle"></param>
        /// <param name="secondRectangle"></param>
        /// <returns></returns>
        private static bool Contain(Rectangle firstRectangle, Rectangle secondRectangle)
        {
            //return firstRectangle.Contains(secondRectangle); ЭТО НЕ КАТИТ, У НАС НЕКЛАССИЧЕСКАЯ СИСТЕМА КООРДИНАТ И РАЗМЕРОВ

            // верхняя сторона второго должна иметь координыты ниже верхней стороны первого
            if (secondRectangle.Y >= firstRectangle.X)
                return false;

            // правая сторона второго должна иметь координыты левее первого
            if (secondRectangle.X + secondRectangle.Width >= firstRectangle.X + firstRectangle.Width)
                return false;

            // нижняя сторона второго должна иметь координыты выше первого
            if (secondRectangle.Y - secondRectangle.Height <= firstRectangle.Y - firstRectangle.Height)
                return false;

            // левая сторона второго должна иметь координыты правее первого
            if (secondRectangle.X <= firstRectangle.X)
                return false;


            return true;
        }

        private static bool Contain(EA.DiagramObject firstDA, EA.DiagramObject secondDA)
        {
            Rectangle firstRectangle = GetRectangle(firstDA);
            Rectangle secondRectangle = GetRectangle(secondDA);
            return Contain(firstRectangle, secondRectangle);
        }

        private static bool Contain(EA.DiagramObject firstDA, Rectangle secondRectangle)
        {
            Rectangle firstRectangle = GetRectangle(firstDA);
            return Contain(firstRectangle, secondRectangle);
        }

        private static Point FindSpaceAround(EA.DiagramObject firstDA, EA.DiagramObject secondDA, EA.DiagramObject parentDA, List<EA.DiagramObject> childDAList)
        {
            Point result = new Point();

            Size parentDASize = GetSize(parentDA);
            Size firstDASize = GetSize(firstDA);
            Size secondDASize = GetSize(secondDA);

            // Конструируем прямоугольник, очерчивающий внутреннюю область родителя, в которую разрешено вписываться
            // т.е урезаем границы
            Rectangle requiredParentRectangle = new Rectangle();
            requiredParentRectangle.X = parentDA.left + DAConst.border;
            requiredParentRectangle.Y = parentDA.top - 2 * DAConst.border;
            requiredParentRectangle.Width = parentDASize.Width - 2 * DAConst.border;
            requiredParentRectangle.Height = parentDASize.Height - 3 * DAConst.border;

            // Конструируем прямоугольник, очерчивающий первый+второй DA
            Rectangle requiredRectangle = new Rectangle();

            // Слева
            requiredRectangle.X = firstDA.left - DAConst.border - secondDASize.Width;
            requiredRectangle.Y = firstDA.top;
            requiredRectangle.Width = firstDASize.Width + DAConst.border + secondDASize.Width;
            requiredRectangle.Height = (firstDASize.Height > secondDASize.Height) ? firstDASize.Height : secondDASize.Height;

            if (Contain(requiredParentRectangle, requiredRectangle)) // если вписываемся в парента
            {
                bool intersectsWithOthers = false;
                foreach (var otherChildDA in childDAList)
                {
                    if (otherChildDA.ElementID == firstDA.ElementID || otherChildDA.ElementID == secondDA.ElementID)
                    {
                        if (Intersects(otherChildDA, requiredRectangle))
                        {
                            intersectsWithOthers = true;
                            break;
                        }
                    }
                }
                if (!intersectsWithOthers) // если ни с кем другим не пересекаемся, то 
                {
                    result.X = requiredRectangle.X;
                    result.Y = requiredRectangle.Y;
                    return result;
                }
            }

            // Вверху
            requiredRectangle.X = firstDA.left;
            requiredRectangle.Y = firstDA.top + DAConst.border + secondDASize.Height;
            requiredRectangle.Width = (firstDASize.Width > secondDASize.Width) ? firstDASize.Width : secondDASize.Width;
            requiredRectangle.Height = firstDASize.Height + DAConst.border + secondDASize.Height;

            if (Contain(requiredParentRectangle, requiredRectangle)) // если вписываемся в парента
            {
                bool intersectsWithOthers = false;
                foreach (var otherChildDA in childDAList)
                {
                    if (otherChildDA.ElementID == firstDA.ElementID || otherChildDA.ElementID == secondDA.ElementID)
                    {
                        if (Intersects(otherChildDA, requiredRectangle))
                        {
                            intersectsWithOthers = true;
                            break;
                        }
                    }
                }
                if (!intersectsWithOthers) // если ни с кем другим не пересекаемся, то 
                {
                    result.X = requiredRectangle.X;
                    result.Y = requiredRectangle.Y;
                    return result;
                }
            }

            // Справа
            requiredRectangle.X = firstDA.left;
            requiredRectangle.Y = firstDA.top;
            requiredRectangle.Width = firstDASize.Width + DAConst.border + secondDASize.Width;
            requiredRectangle.Height = (firstDASize.Height > secondDASize.Height) ? firstDASize.Height : secondDASize.Height;

            if (Contain(requiredParentRectangle, requiredRectangle)) // если вписываемся в парента
            {
                bool intersectsWithOthers = false;
                foreach (var otherChildDA in childDAList)
                {
                    if (otherChildDA.ElementID == firstDA.ElementID || otherChildDA.ElementID == secondDA.ElementID)
                    {
                        if (Intersects(otherChildDA, requiredRectangle))
                        {
                            intersectsWithOthers = true;
                            break;
                        }
                    }
                }
                if (!intersectsWithOthers) // если ни с кем другим не пересекаемся, то 
                {
                    result.X = requiredRectangle.X;
                    result.Y = requiredRectangle.Y;
                    return result;
                }
            }

            // внизу
            requiredRectangle.X = firstDA.left;
            requiredRectangle.Y = firstDA.top - DAConst.border - secondDASize.Height;
            requiredRectangle.Width = (firstDASize.Width > secondDASize.Width) ? firstDASize.Width : secondDASize.Width;
            requiredRectangle.Height = firstDASize.Height + DAConst.border + secondDASize.Height;

            if (Contain(requiredParentRectangle, requiredRectangle)) // если вписываемся в парента
            {
                bool intersectsWithOthers = false;
                foreach (var otherChildDA in childDAList)
                {
                    if (otherChildDA.ElementID == firstDA.ElementID || otherChildDA.ElementID == secondDA.ElementID)
                    {
                        if (Intersects(otherChildDA, requiredRectangle))
                        {
                            intersectsWithOthers = true;
                            break;
                        }
                    }
                }
                if (!intersectsWithOthers) // если ни с кем другим не пересекаемся, то 
                {
                    result.X = requiredRectangle.X;
                    result.Y = requiredRectangle.Y;
                    return result;
                }
            }



            return result;

        }

        /// <summary>
        /// Функция возвращает точку внутри DA (координаты отсчитываются от DA), правее и ниже которой нет дочерних элементов
        /// </summary>
        /// <param name="DA">объект, куда вписываем</param>
        /// <param name="childDAList">прочие объекты внутри DA</param>
        /// <returns></returns>
        private static Point GetDAFreeBorders(EA.DiagramObject diagramObject, List<EA.DiagramObject> childDAList)
        {
            Point result = new Point(diagramObject.left, diagramObject.top);

            foreach (var childDA in childDAList)
            {
                if (result.X < childDA.right) result.X = childDA.right;
                if (result.Y > childDA.bottom) result.Y = childDA.bottom;
            }

            // Делаем координаты относительно diagramObject
            result.X = result.X - diagramObject.left;
            result.Y = result.Y - diagramObject.top;

            return result;
        }

        /// <summary>
        /// Вспомогательная функция получения Rectangle DA
        /// </summary>
        /// <param name="diagramObject"></param>
        /// <returns></returns>
        private static Rectangle GetRectangle(EA.DiagramObject diagramObject)
        {
            return new Rectangle()
            {
                X = diagramObject.left,
                Y = diagramObject.top,
                Width = diagramObject.right - diagramObject.left,
                Height = diagramObject.top - diagramObject.bottom
            };
        }
        /// <summary>
        /// Функция определяет, пересекаются ли два прямоугольника (совпадение границ - не пересечение)
        /// </summary>
        /// <param name="firstRectangle"></param>
        /// <param name="secondRectangle"></param>
        /// <returns></returns>
        private static bool Intersects(Rectangle firstRectangle, Rectangle secondRectangle)
        {
            return firstRectangle.IntersectsWith(secondRectangle);
        }
        private static bool Intersects(EA.DiagramObject firstDA, EA.DiagramObject secondDA)
        {
            Rectangle firstRectangle = GetRectangle(firstDA);
            Rectangle secondRectangle = GetRectangle(secondDA);
            return Intersects(firstRectangle, secondRectangle);
        }
        private static bool Intersects(EA.DiagramObject firstDA, Rectangle secondRectangle)
        {
            Rectangle firstRectangle = GetRectangle(firstDA);
            return Intersects(firstRectangle, secondRectangle);
        }
    }
}
