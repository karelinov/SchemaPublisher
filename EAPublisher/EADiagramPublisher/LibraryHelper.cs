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
                if (LibraryHelper.IsLibrary(curParent) && new ComponentLevel[] { ComponentLevel.ContourComponent, ComponentLevel.ContourContour }.Contains(CLHelper.GetComponentLevel(curParent)))
                {
                    result = curParent;
                    break;
                }
            }

            return result;
        }

        public static DPTreeNode<String> GetLibComponentTree()
        {
            DPTreeNode<String> result = new DPTreeNode<String>(null);

            // Проходимся по дереву библиотеки и ищем компоненты.

            EA.Package libRootPackage = Context.CurrentLibrary;
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
                        if (LibraryHelper.IsLibrary(curElement))
                        {

                        }
                    }
                }




            } while (curLevelPackages.Count == 0); // пока не кончатся пакеты "уровнем ниже"





            return result;
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

            if (result == null)
                throw new Exception("Не обнаружена библиотека");

            return result;
        }

        /// <summary>
        /// Функция возвращает список данных элементов библиотеки, соответствующих переданному уровню
        /// Даные возвращаются в формате NodeData
        /// </summary>
        /// <param name="clList"></param>
        public static List<NodeData> GetNodeData(List<ComponentLevel> clList, bool onlyAppropriateForDiagram = true)
        {
            List<NodeData> result = new List<NodeData>();


            EA.Package LibRoot = Context.CurrentLibrary;

            List<EA.Package> curLevelPackages = new List<EA.Package>();
            curLevelPackages.Add(LibRoot);


            // Последовательно проходимся по уровням дерева пакетов
            while (curLevelPackages.Count > 0)
            {
                List<EA.Package> nextLevelPackages = new List<EA.Package>(); // список пакетов для перехода на следующий уровень

                foreach (EA.Package curPackage in curLevelPackages)
                {
                    // Выбираем из списка пакетов текущего уровня требуемые элементы
                    foreach (EA.Element curElement in curPackage.Elements)
                    {

                        if (IsLibrary(curElement) && clList.Contains(CLHelper.GetComponentLevel(curElement)) && (onlyAppropriateForDiagram ? AppropriateForDiagram(curElement) : true))
                        {
                            NodeData nodeData = new NodeData();
                            nodeData.Element = curElement;
                            nodeData.ComponentLevel = CLHelper.GetComponentLevel(curElement);
                            nodeData.Contour = GetElementContour(curElement);
                            nodeData.GroupNames = EAHelper.GetTaggedValue(curElement, DAConst.DP_NodeGroupsTag).Split(',');
                            result.Add(nodeData);
                        }
                    }

                    // строим список пакетов для перехода на следующий уровень
                    foreach (EA.Package nextPackage in curPackage.Packages)
                        nextLevelPackages.Add(nextPackage);

                }

                curLevelPackages = nextLevelPackages;
            }

            return result;
        }

        /// <summary>
        /// Вспомопгтельная функция определяет, предназанчен ли библиотечный элемнет для размещения на диаграмме
        /// Правила: Контуры = разрешён Classifier, остальные компоненты - только Instance
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool AppropriateForDiagram(EA.Element element)
        {
            bool result = false;

            ComponentLevel componentLevel = CLHelper.GetComponentLevel(element);

            switch (componentLevel)
            {
                case ComponentLevel.SystemContour:
                case ComponentLevel.ContourContour:

                    result = true;
                    break;

                case ComponentLevel.SystemComponent:
                case ComponentLevel.ContourComponent:
                case ComponentLevel.Node:
                case ComponentLevel.Device:
                case ComponentLevel.ExecutionEnv:
                case ComponentLevel.Component:

                    result = element.ClassfierID != 0;
                    break;

            }

            return result;
        }


        /// <summary>
        /// Проверяет, что элемент является библиотечным. Если это инстанс, проверяет также класс
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsLibrary(EA.Element element)
        {
            bool result = false;

            if (element.Type != "DeploymentSpecification")
            {

                EA.Collection taggedValues = EAHelper.GetTaggedValues(element);
                if (taggedValues != null && taggedValues.GetByName(DAConst.DP_LibraryTag) != null)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Проверяет является ли коннектор библиотечным
        /// </summary>
        /// <param name="connector"></param>
        /// <returns></returns>
        public static bool IsLibrary(EA.Connector connector)
        {
            bool result = false;

            if (connector.TaggedValues.GetByName(DAConst.DP_LibraryTag) != null)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Устанавливает текущую библиотеку
        /// </summary>
        /// <returns></returns>
        public static ExecResult<Boolean> SetCurrentLibrary()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();

            try
            {
                EA.Package libRoot = GetLibraryRootFromTreeSelection();
                if (libRoot == null)
                    throw new Exception("Не обнаружена библиотека ");


                Context.CurrentLibrary = libRoot;
                result.code = 0;
                result.value = true;
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        /// <summary>
        /// Возвращает корневой пакет библиотеки для первого найденного на текущей диаграмме библиотечного элемента
        /// </summary>
        /// <returns></returns>
        private static EA.Package GetLibraryRootFromDiagram()
        {
            if (CurrentDiagram == null) return null;

            // Пробегаемся по диаграмме в поисках библиотечного элемента
            foreach (EA.DiagramObject curDA in CurrentDiagram.DiagramObjects)
            {
                EA.Element curElement = EARepository.GetElementByID(curDA.ElementID);

                if (IsLibrary(curElement))
                {
                    return GetRootLibPackage(EARepository.GetPackageByID(curElement.PackageID));
                }
            }

            // Если через библиотечные элементы на диаграмме не получчилось, пытаемся найти от пакета диаграммы
            return GetRootLibPackage(EARepository.GetPackageByID(CurrentDiagram.PackageID));

        }

        /// <summary>
        /// Возвращает корневой пакет библиотеки для выделенного в дереве библиотечного элемента
        /// </summary>
        /// <returns></returns>
        private static EA.Package GetLibraryRootFromTreeSelection()
        {
            EA.Package libPackage = null;
            EA.Element libElement = null;

            // сначала проверяем "на библиотечность" текущий выделенный пакет
            EA.Package selectedPackage = EARepository.GetTreeSelectedPackage();
            if (selectedPackage != null)
            {
                if (IsLibrary(selectedPackage.Element))
                {
                    libPackage = selectedPackage;
                }
            }

            // потом проверяем "на библиотечность" выделенные элементы дерева
            if (libPackage == null)
            {
                EA.Collection curSelection = EARepository.GetTreeSelectedElements();
                foreach (EA.Element curSelectedElement in curSelection)
                {
                    if (IsLibrary(curSelectedElement))
                    {
                        libElement = curSelectedElement;
                        break;
                    }
                }
                if (libElement == null) return null;

                // Для элемента получаем пакет и отдаём в функцию поиска корня библиотеки по пакетам
                libPackage = EARepository.GetPackageByID(libElement.PackageID);
            }

            return GetRootLibPackage(libPackage);
        }



        /// <summary>
        /// Функция от указанного пакета лезет  вверх по дереву объектов репозитория и останавливается в одном из случаев:
        /// - когда по пути были библиотечные, но закончились (найден корень библиотеки)
        /// - когда по пути были библиотечных объектов не нашлось
        /// </summary>
        /// <param name="startPackage"></param>
        /// <returns>Корень библиотеки если найден</returns>
        private static EA.Package GetRootLibPackage(EA.Package startPackage)
        {
            EA.Package result = null;

            // лезем от элемента вверх по дереву пакетов, пока не достигнем верха либо не достигнем пакета без тэга DP_Library после нахождения такого тэга
            EA.Package curPackage = startPackage;
            if (IsLibrary(curPackage.Element)) result = curPackage;

            bool foundPackageAfterDPLibrary = false;
            bool foundDPLibraryPackage = false;

            while (curPackage != null && !foundPackageAfterDPLibrary)
            {
                if (curPackage.ParentID != 0)
                    curPackage = EARepository.GetPackageByID(curPackage.ParentID);
                else
                    curPackage = null;

                if (curPackage != null)
                {
                    if (curPackage.Element != null && IsLibrary(curPackage.Element))
                    {
                        foundDPLibraryPackage = true;
                        result = curPackage;
                    }

                    if ((curPackage.Element == null || !IsLibrary(curPackage.Element)) && foundDPLibraryPackage)
                        foundPackageAfterDPLibrary = true;
                }
            }


            return result;
        }

        /// <summary>
        /// Вычисляет (предлагаемый) ID инфопотока, исходящего из элемента
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string SuggestFlowIDName(EA.Element element)
        {
            string result = "";

            string flowID = EAHelper.GetTaggedValue(element, DAConst.DP_NameForFlowIDTag, false) ;

            // от данного элемента лезем вверх по его  свзям SoftwareClassification и склеиваем тэги DP_NameForFlowID
            List<EA.Element> connectedElements = EAHelper.GetConnectedElements(element, LinkType.SoftwareClassification, 1);
            // если у элемента нет таких зависимостей, но есть классификатор - ищем зависимости у классификатора
            if (connectedElements.Count == 0)
            {
                if(element.ClassifierID !=0)
                {
                    flowID = EAHelper.GetTaggedValue(element, DAConst.DP_NameForFlowIDTag, false);
                    connectedElements = EAHelper.GetConnectedElements(EARepository.GetElementByID(element.ClassifierID) , LinkType.SoftwareClassification, 1);
                }
            }

            while (connectedElements.Count >0)
            {
                string nextname = EAHelper.GetTaggedValue(connectedElements[0], DAConst.DP_NameForFlowIDTag, false);
                if (nextname != "")
                {
                    if (flowID == "")
                        flowID = nextname;
                    else
                        flowID = nextname +"."+ flowID;
                }

                connectedElements = EAHelper.GetConnectedElements(connectedElements[0], LinkType.SoftwareClassification, 1);
            }

            // Теперь ищем подбираем номер
            int segmentNumber = 1;

            string flowIDWithNumber = flowID + " " + segmentNumber.ToString();

            while (Context.ConnectorData[LinkType.SoftwareClassification].ContainsKey(flowIDWithNumber))
            {
                segmentNumber++;
                flowIDWithNumber = flowID + " " + segmentNumber.ToString();
            }

            result = flowIDWithNumber;

            return result;
        }


    }
}



