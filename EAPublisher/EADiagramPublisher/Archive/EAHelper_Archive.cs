using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Archive
{
    class EAHelper_Archive
    {
        /// <summary>
        /// Подвигает DA и вписанные в него DA на указанный вектор
        /// </summary>
        /// <param name="diagramObject"></param>
        /// <param name="vector"></param>
        public static void MoveContainedHierarchy(EA.DiagramObject diagramObject, Point vector)
        {
            // Сначала получаем и подывигаем детей
            List<EA.DiagramObject> containedDAList = EADOHelper.GetContainedForDA(diagramObject);
            foreach (var curContainedDA in containedDAList)
            {
                MoveContainedHierarchy(curContainedDA, vector);
            }

            // потом сам объект
            EADOHelper.ApplyVectorToDA(diagramObject, vector);
        }

        /// <summary>
        /// Возвращает Список первых из имеющейся иерархии дочерних объектов, присутствующих на диаграмме
        /// </summary>
        /// <param name="eaElement"></param>
        /// <returns></returns>
        public static List<EA.DiagramObject> GetNearestChildrenDA(EA.Element eaElement)
        {
            List<EA.DiagramObject> result = new List<EA.DiagramObject>();

            // Получаем собственно дочерних
            List<EA.Element> childrenElements = LibraryHelper.GetDeployChildren(eaElement);
            // для каждого проверяем, нет ли его на диаграмме
            foreach (EA.Element childElement in childrenElements)
            {
                // Проверяем, что элемент на диаграмме
                EA.DiagramObject childElementDA = Context.CurrentDiagram.GetDiagramObjectByID(childElement.ElementID, "");

                if (childElementDA != null) // ... и если есть - добавляем в результат
                {
                    result.Add(childElementDA);
                }
                else // а если нет - надо проверить дочерних для эьтих дочерних - вдруг они на диаграмме
                {
                    List<EA.DiagramObject> grandchildrenDA = GetNearestChildrenDA(childElement);
                    result.AddRange(grandchildrenDA);
                }

            }

            return result;
        }


    }
}
