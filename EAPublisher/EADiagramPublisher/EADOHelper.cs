using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EADiagramPublisher
{
    /// <summary>
    /// Хэлпер с функциями манипулирования Объектами диаграмм (DiagramObject)
    /// </summary>
    class EADOHelper
    {
        /// <summary>
        /// Функция передвигает DA в указанную точку
        /// </summary>
        /// <param name="diagramObject"></param>
        /// <param name="newStart"></param>
        /// <param name="doUpdate"></param>
        public static void ApplyPointToDA(EA.DiagramObject diagramObject, Point newStart, bool doUpdate = true)
        {
            Size diagramObjectSize = ElementDesignerHelper.GetSize(diagramObject);

            diagramObject.left = newStart.X;
            diagramObject.right = diagramObject.left + diagramObjectSize.Width;
            diagramObject.top = newStart.Y;
            diagramObject.bottom = diagramObject.top - diagramObjectSize.Height;
            if (doUpdate) diagramObject.Update();
        }

        /// <summary>
        /// Функция применяет указанный размер к объекту DiagramObject
        /// </summary>
        /// <param name="diagramObject"></param>
        /// <param name="size"></param>
        /// <param name="doUpdate"></param>
        public static void ApplySizeToDA(EA.DiagramObject diagramObject, Size size, bool doUpdate = true)
        {
            diagramObject.right = diagramObject.left + size.Width;
            diagramObject.bottom = diagramObject.top - size.Height;
            if (doUpdate) diagramObject.Update();
        }

        /// <summary>
        /// Функция передвигает DA по указанному вектору (т.е. смещает на размеры точки)
        /// </summary>
        /// <param name="diagramObject"></param>
        /// <param name="newStart"></param>
        /// <param name="doUpdate"></param>
        public static void ApplyVectorToDA(EA.DiagramObject diagramObject, Point vector, bool doUpdate = true)
        {
            Size diagramObjectSize = ElementDesignerHelper.GetSize(diagramObject);

            diagramObject.left += vector.X;
            diagramObject.right += vector.X;
            diagramObject.top += vector.Y;
            diagramObject.bottom += vector.Y;
            if (doUpdate) diagramObject.Update();
        }


        /// <summary>
        /// Фунция возвращает список непосредственных вставленных DA для указанного
        /// Вставленные это:
        /// - С границами внутри указанного
        /// - С Z-Order "выше" указанного
        /// - Которые не могут быть названными вставленными другими da с Z-Order "выше" указанного 
        /// 
        /// </summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static List<EA.DiagramObject> GetContainedForDA(EA.DiagramObject da)
        {
            List<EA.DiagramObject> result = new List<EA.DiagramObject>();

            // Сначала проходимся по элементам диаграммы и смотрим кто укладывается в рамки и Z-order
            foreach (EA.DiagramObject curDA in Context.Designer.CurrentDiagram.DiagramObjects)
            {
                if (curDA.ElementID == da.ElementID) continue;
                // если такой элемент есть, проходимся по элементам даграммы ещё раз и смотрим, 
                // не может ли он принадлежать ещё кому-то с меньшим Z-order
                if (ElementDesignerHelper.DAFitInside(curDA, da) && da.Sequence > curDA.Sequence)
                {
                    bool hasAnotherParent = false;

                    foreach (EA.DiagramObject curDA1 in Context.Designer.CurrentDiagram.DiagramObjects)
                    {
                        if (curDA1.ElementID == da.ElementID) continue;
                        if (ElementDesignerHelper.DAFitInside(curDA, curDA1) && curDA1.Sequence < da.Sequence)
                        {
                            hasAnotherParent = true;
                            break;
                        }

                    }
                    if (!hasAnotherParent)
                        result.Add(curDA);

                }
                else
                {
                    //EAHelper.Out("Не Укладывается в рамки и Zorder: ", new EA.DiagramObject[] { curDA });
                }
            }


            return result;
        }

        /// <summary>
        /// Фунция возвращает DA, в который непосредственно вставлен указанный DA
        /// Вставленные это:
        /// - С границами внутри указанного
        /// - С Z-Order "выше" указанного
        /// - Которые не могут быть названными вставленными другими da с Z-Order "выше" указанного 
        /// 
        /// </summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static EA.DiagramObject GetContainerOfDA(EA.DiagramObject da)
        {
            EA.DiagramObject result = null;

            // Сначала проходимся по элементам диаграммы и смотрим кто укладывается в рамки и Z-order

            foreach (EA.DiagramObject curDA in Context.Designer.CurrentDiagram.DiagramObjects)
            {
                if (curDA.ElementID == da.ElementID) continue;

                if (ElementDesignerHelper.DAFitInside(da, curDA) && da.Sequence < curDA.Sequence)
                {
                    if (result == null)
                    {
                        result = curDA;
                    }
                    else
                    {
                        if (result.Sequence < curDA.Sequence) // если уже кого то отобрали, то новый становится "папой" только если у него больше Zorder
                        {
                            result = curDA;
                        }
                    }
                }
            }


            return result;
        }

    }
}
