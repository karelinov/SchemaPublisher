using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;

namespace EADiagramPublisher
{

    /// <summary>
    /// Хэлпер для работы с библиотекой
    /// </summary>
    public class LibraryHelper
    {
        /// <summary>
        /// Shortcut до глобальной переменной с EA.Diagram
        /// </summary>
        public static EA.Diagram CurrentDiagram
        {
            get
            {
                return Context.CurrentDiagram;
            }
        }
        /// <summary>
        /// Shortcut до глобальной переменной с EA.Repository
        /// </summary>
        private static EA.Repository EARepository
        {
            get
            {
                return Context.EARepository;
            }
        }

        /// <summary>
        /// Возвращает корневой пакет библиотеки для первого найденного на текущей диаграмме библиотечного элемента
        /// </summary>
        /// <returns></returns>
        public static EA.Package GetLibraryRootFromDiagram()
        {
            if (CurrentDiagram == null) return null;

            // Пробегаемся по диаграмме в поисках библиотечного элемента
            foreach (EA.DiagramObject curDA in CurrentDiagram.DiagramObjects)
            {
                EA.Element curElement = EARepository.GetElementByID(curDA.ElementID);

                if (EAHelper.IsLibrary(curElement))
                {
                    return EAHelper.GetRootLibPackage(EARepository.GetPackageByID(curElement.PackageID));
                }
            }

            // Если через библиотечные элементы на диаграмме не получчилось, пытаемся найти от пакета диаграммы
            return EAHelper.GetRootLibPackage(EARepository.GetPackageByID(CurrentDiagram.PackageID));

        }

        /// <summary>
        /// Возвращает корневой пакет библиотеки для выделенного в дереве библиотечного элемента
        /// </summary>
        /// <returns></returns>
        public static EA.Package GetLibraryRootFromTreeSelection()
        {
            EA.Element libElement = null;

            EA.Collection curSelection = EARepository.GetTreeSelectedElements();
            foreach (EA.Element curSelectedElement in curSelection)
            {
                if (EAHelper.IsLibrary(curSelectedElement))
                {
                    libElement = curSelectedElement;
                    break;
                }
            }
            if (libElement == null) return null;

            // Пробегаемся по диаграмме в поисках библиотечного элемента
            foreach (EA.Element curElement in CurrentDiagram.DiagramObjects)
            {
                if (EAHelper.IsLibrary(curElement))
                {
                    libElement = curElement;
                    break;
                }
            }
            if (libElement == null) return null; // облом, нет среди выделенного ничего библиотечного
            EA.Package curpackage = EARepository.GetPackageByID(libElement.PackageID);

            return EAHelper.GetRootLibPackage(curpackage);
        }


        /// <summary>
        /// Возвращает корневой пакет библиотеки по данным выделеанного в дереве объекта или элементам текущей диаграммы
        /// </summary>
        /// <returns></returns>
        public static EA.Package GetLibraryRoot()
        {
            EA.Package result = null;

            result = GetLibraryRootFromTreeSelection();

            if (result == null)
                result = GetLibraryRootFromDiagram();

            return result;
        }


        public static DPTreeNode<String> GetLibComponentTree()
        {
            DPTreeNode<String> result = new DPTreeNode<String>(null);

            // Проходимся по дереву библиотеки и ищем компоненты.

            EA.Package libRootPackage = GetLibraryRoot();
            if (libRootPackage == null)
                throw new Exception("Не обнаружена библиотека компоннетов");

            List<EA.Package> curLevelPackages = new List<EA.Package>();
            curLevelPackages.Add(libRootPackage);

            do
            {
                foreach (EA.Package curPackage in curLevelPackages)
                {
                    foreach (EA.Element curElement in curPackage.Elements)
                    {
                        if (EAHelper.IsLibrary(curElement))
                        {

                        }
                    }
                }




            } while (curLevelPackages.Count == 0); // пока не кончатся пакеты "уровнем ниже"





            return result;
        }

        /// <summary>
        /// Функция возвращает список данных элементов библиотеки, соответствующих переданному уровню
        /// Даные возвращаются в формате NodeData
        /// </summary>
        /// <param name="clList"></param>
        public static List<NodeData> GetNodeData(List<ComponentLevel> clList)
        {
            List<NodeData> result = new List<NodeData>();


            EA.Package LibRoot = GetLibraryRoot();

            List<EA.Package> curLevelPackages = new List<EA.Package>();
            curLevelPackages.Add(LibRoot);

            // Последовательно проходимся по уровням дерева пакетов
            while (curLevelPackages.Count > 0)
            {
                foreach (EA.Package curPackage in curLevelPackages)
                {
                    foreach (EA.Element curElement in curPackage.Elements)
                    {
                        if (EAHelper.IsLibrary(curElement) && curElement.ClassfierID != 0 && clList.Contains(CLHelper.GetComponentLevel(curElement)))
                        {
                            NodeData nodeData = new NodeData();
                            nodeData.Element = curElement;
                            nodeData.ComponentLevel = CLHelper.GetComponentLevel(curElement);
                            nodeData.Contour = GetElementContour(curElement);
                            nodeData.GroupNames = EAHelper.GetTaggedValue(curElement, DAConst.DP_NodeGroupsTag).Split();
                            result.Add(nodeData);
                        }
                    }

                }

                // строим список пакетов для перехода на следующий уровень
                List<EA.Package> nextLevelPackages = new List<EA.Package>();
                foreach (EA.Package curPackage in curLevelPackages)
                {
                    foreach (EA.Package nextPackage in curPackage.Packages)
                        nextLevelPackages.Add(nextPackage);
                }
                curLevelPackages = nextLevelPackages;
            }

            return result;
        }


        /// <summary>
        /// Возвращает, если есть, название (ближайшего) контура, в который включен компонент
        /// </summary>
        /// <param name="curElement"></param>
        /// <returns></returns>
        public static EA.Element GetElementContour(EA.Element curElement)
        {
            EA.Element result = null;

            List<EA.Element> parentDeployHierrchy = EAHelper.GetParentHierarchy(curElement);

            foreach (EA.Element curParent in parentDeployHierrchy)
            {
                if (EAHelper.IsLibrary(curParent) && new ComponentLevel[] { ComponentLevel.ContourComponent, ComponentLevel.ContourContour }.Contains(CLHelper.GetComponentLevel(curParent)))
                {
                    result = curParent;
                    break;
                }
            }

            return result;
        }




    }



}



