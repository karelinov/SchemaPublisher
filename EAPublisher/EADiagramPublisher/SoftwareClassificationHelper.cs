using EADiagramPublisher.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher
{
    public class SoftwareClassificationHelper
    {
        public static DPTreeNode<ElementData> GetSoftwareClassification()
        {
            List<DPTreeNode<ElementData>> result = new List<DPTreeNode<ElementData>>();


            // Перебираем все коннекторы SoftwareClassification
            foreach (ConnectorData connectorData in Context.ConnectorData.Values.Where(cd => cd.IsLibrary && cd.LinkType == Enums.LinkType.SoftwareClassification))
            {
                // получаем описатель элемента начала и конца коннектора
                ElementData sourceElementData = Context.ElementData[connectorData.SourceElementID];
                ElementData targetElementData = Context.ElementData[connectorData.TargetElementID];

                // строим узелы в дереве
                AddDPNodeToList(result, sourceElementData, targetElementData);
            }

            // в конце в списке должен остаться только один корневой элемент, если это не так - ошибка метаданных (или бага плагина)
            if (result.Count > 1)
            {
                string rootElements = "";
                foreach(var node in result)
                {
                    rootElements += node.Value._ElementID.ToString() + " " + node.Value.DisplayName +"("+ node.AllNodes.Count() +"), ";
                }
                throw new Exception("В классификации ПО обнаружено несколько корневых узлов: "+ rootElements);
            }


            return result[0];
        }

        // Вспомогательная функция добавления элемента в структуру строящегося дерева
        private static void AddDPNodeToList(List<DPTreeNode<ElementData>> dpTreeNodeList, ElementData sourceElementData, ElementData targetElementData)
        {
            // сначала ищем, нет ли уже элемента в структуре
            DPTreeNode<ElementData> sourceElementNode = null;
            DPTreeNode<ElementData> sourceElementNodeRoot = null;
            DPTreeNode<ElementData> targetElementNode = null;
            DPTreeNode<ElementData> targetElementNodeRoot = null;

            foreach (DPTreeNode<ElementData> curNode in dpTreeNodeList)
            {
                if (curNode.AllNodes.ContainsKey(sourceElementData.ID))
                {
                    sourceElementNode = curNode.AllNodes[sourceElementData.ID];
                    sourceElementNodeRoot = curNode;
                }
                if (curNode.AllNodes.ContainsKey(targetElementData.ID))
                {
                    targetElementNode = curNode.AllNodes[targetElementData.ID];
                    targetElementNodeRoot = curNode;
                }

                if (sourceElementNode != null && targetElementNode != null)
                    break;
            }

            // Варианты: 
            // оба узла найдены и находятся в одной иерархии - ничего делать не надо
            if (sourceElementNode != null && targetElementNode != null && sourceElementNodeRoot == targetElementNodeRoot)
            {
                // Ничего не делаем
            }
            // оба узла найдены и находятся разных иерархиях - надо объъединить эти ветки
            if (sourceElementNode != null && targetElementNode != null && sourceElementNodeRoot != targetElementNodeRoot)
            {
                targetElementNode.AddChildNode(sourceElementNode);
                dpTreeNodeList.Remove(sourceElementNode);
            }
            // найден только дочерний узел - создаём в этой же ветке родительский и перевешиваем на него дочерний
            // такая ситуация возможна только если дочерний дежит на самом верху (временно является рутом данной ветки)
            else if (sourceElementNode != null && targetElementNode == null)
            {
                targetElementNode = new DPTreeNode<ElementData>(targetElementData, true);
                targetElementNode.AddChildNode(sourceElementNode);

                dpTreeNodeList.Remove(sourceElementNode);
                dpTreeNodeList.Add(targetElementNode);
            }
            // найден только родительский узел - создаём в этой же ветке  дочерний
            // такая ситуация возможна только если родительский лежит в самом низу ветки (временно является одним из листовых узлов)
            else if (sourceElementNode == null && targetElementNode != null)
            {
                sourceElementNode = new DPTreeNode<ElementData>(sourceElementData, false);
                targetElementNode.AddChildNode(sourceElementNode);
            }
            // оба не найдены - создаём оба узла в новой ветке
            else if (sourceElementNode == null && targetElementNode == null)
            {
                sourceElementNode = new DPTreeNode<ElementData>(sourceElementData, false);
                targetElementNode = new DPTreeNode<ElementData>(targetElementData, true);
                targetElementNode.AddChildNode(sourceElementNode);
                dpTreeNodeList.Add(targetElementNode);
            }
        }

        /// <summary>
        /// Функция вычисляет, принадлежит ли переданный в функцию элемент указанному классификатору ПО текущей 
        /// </summary>
        /// <param name="elementData"></param>
        /// <param name="softwareClassificationData"></param>
        /// <returns></returns>
        public static bool ISBelongsToSoftware(ElementData elementData, ElementData softwareClassificationData)
        {
            bool result = false;

            // Считаем, что проверяется экземпляр класса, участвующего в классификации ПО
            if (elementData.ClassifierID != null)
            {
                // ищем класс элемента среди элементов классификации ПО
                if (Context.SoftwareClassification.AllNodes.ContainsKey(elementData.ClassifierID))
                {

                    // получаем класс проверяемого элемента
                    DPTreeNode<ElementData> classifierDataNode = Context.SoftwareClassification.AllNodes[elementData.ClassifierID];

                    // Проходимся от полученного элемента классификации вверх, Проверяем , не являются ли они требуемыми
                    while (classifierDataNode != null)
                    {
                        // Сверяем
                        if (classifierDataNode.Value._ElementID == softwareClassificationData._ElementID)
                        {
                            result = true;
                            break;
                        }
                        classifierDataNode = classifierDataNode.Parent; // переходим к родителю
                    }
                }
            }

            return result;
        }


    }
}
