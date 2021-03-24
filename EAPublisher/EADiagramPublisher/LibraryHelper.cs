using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;
using EADiagramPublisher.SQL;

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
        /// Вспомогательная функция определяет, предназанчен ли библиотечный элемнет для размещения на диаграмме
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
        /// Возвращает Список дочерних (deploy) объектов для указанного (дальнейшая иерархия не раскручивается)
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static List<EA.Element> GetDeployChildren(EA.Element element)
        {
            List<EA.Element> result = new List<EA.Element>();

            foreach (EA.Connector connector in element.Connectors)
            {
                if (ConnectorHelper.IsDeploymentLink(connector) && connector.SupplierID == element.ElementID)
                {
                    result.Add(EARepository.GetElementByID(connector.ClientID));
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает дерево дочерних (deploy) элементов для указанного элемента
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static DPTreeNode<EA.Element> GetDeployChildrenHierarchy(EA.Element element)
        {
            // Создаём корень
            DPTreeNode<EA.Element> result = new DPTreeNode<EA.Element>(element);

            // Получаем непосредственно размещённые элементы
            List<EA.Element> children = GetDeployChildren(element);

            // Проходимся по вложениям
            foreach (EA.Element child in children)
            {
                // получаем дочернее дерево и добавляем его к корневому узлу
                DPTreeNode<EA.Element> childTree = GetDeployChildrenHierarchy(child);
                result.AddChildNode(childTree);
            }

            return result;
        }

        /// <summary>
        /// Возвращает Список всех дочерних (deploy, всё дерево) элементов диаграммы, присутствующих на диаграмме
        /// </summary>
        /// <param name="eaElement"></param>
        /// <returns></returns>
        public static List<EA.DiagramObject> GetDeployChildrenHierarchyDA(EA.Element eaElement)
        {
            List<EA.DiagramObject> result = new List<EA.DiagramObject>();

            // Получаем собственно дочерних
            List<EA.Element> childrenElements = GetChildHierarchy(eaElement);

            // для каждого проверяем, нет ли его на диаграмме
            foreach (EA.Element childElement in childrenElements)
            {
                EA.DiagramObject childElementDA = Context.CurrentDiagram.GetDiagramObjectByID(childElement.ElementID, "");
                if (childElementDA != null) // если есть на диаграмме - добавляем его в результат
                {
                    result.Add(childElementDA);
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает если есть родительский элемент размещения
        /// </summary>
        public static EA.Element GetDeployParent(EA.Element eaElement)
        {
            EA.Element result = null;


            foreach (EA.Connector connector in eaElement.Connectors)
            {
                if (ConnectorHelper.IsDeploymentLink(connector) && connector.ClientID == eaElement.ElementID)
                {
                    result = EARepository.GetElementByID(connector.SupplierID);
                    break;
                }
            }


            return result;
        }

        /// <summary>
        /// Возвращает если есть родительский элемент размещения
        /// </summary>
        public static ElementData GetDeployParent(ElementData elementData)
        {
            ElementData result = null;

            // Инициируем обращение к спискам элементов и коннекторов, чтобы связать списки (если они ещё не инициированы и не связаны)
            var ed = Context.ElementData;
            var cd = Context.ConnectorData;

            foreach (ConnectorData connectorData in elementData.ConnectorsData)
            {
                if (connectorData.LinkType == LinkType.Deploy && connectorData.SourceElementID == elementData._ElementID)
                {
                    result = Context.ElementData[connectorData.TargetElementID];
                    break;
                }
            }


            return result;
        }

        /// <summary>
        /// Возвращает для элемента родительский элемент Componentlevel = Node/Device (если есть)
        /// То есть Узел, в котором он размещён
        /// Ограничение: считаем, что узлы внутри узлов не располагаются
        /// </summary>
        public static int? GetDeployComponentNode(int eaElementID)
        {
            int? result = null;

            ElementData elementData;
            if (Context.ElementData.ContainsKey(eaElementID))
                elementData = Context.ElementData[eaElementID];
            else // не библиотечный элемент
                return result;

            // Узел ищем только для компонентов и сред исполнения
            if (elementData.ComponentLevel == ComponentLevel.ExecutionEnv || elementData.ComponentLevel == ComponentLevel.Component)
            {
                ElementData parentElementData = GetDeployParent(elementData); 

                while (parentElementData != null)
                {
                    if (parentElementData.ComponentLevel == ComponentLevel.Device || parentElementData.ComponentLevel == ComponentLevel.Node)
                    {
                        result = parentElementData._ElementID;
                        break;
                    }

                    parentElementData = GetDeployParent(parentElementData);
                }
            }


            return result;
        }

        /// <summary>
        /// Возвращает Список элементов диаграммы, являющихся для него родительскими
        /// </summary>
        /// <param name="element"></param>
        /// <returns>Отсортированный в сооответствии с род. иерархией список DiagramObjects</returns>
        public static List<EA.DiagramObject> GetDeployParentHierarchyDA(EA.Element element)
        {
            List<EA.DiagramObject> result = new List<EA.DiagramObject>();

            // Получаем список родительской иерархии
            List<EA.Element> parentHierarchy = GetParentHierarchy(element);

            foreach (EA.Element curParent in parentHierarchy)
            {
                EA.DiagramObject curParentDA = Context.Designer.CurrentDiagram.GetDiagramObjectByID(curParent.ElementID, "");
                if (curParentDA != null)
                    result.Add(curParentDA);
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

            List<EA.Element> parentDeployHierrchy = GetParentHierarchy(curElement);

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

        /// <summary>
        /// Функция возвращает название ПО, которому принадлежит элемент
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetElementSoftwareName(EA.Element element)
        {
            string result = "";

            // пока тупо взвращаем ПО верхнего уровня

            // сначала пооучаем классификатор
            if (element.ClassifierID != 0)
            {
                EA.Element classifier = EARepository.GetElementByID(element.ClassifierID);
                // проверяем наличие связи SoftwareClassifier
                foreach (EA.Connector connector in classifier.Connectors)
                {
                    if (LTHelper.GetConnectorType(connector) == LinkType.SoftwareClassification && connector.ClientEnd == classifier)
                    {
                        result = classifier.Name;

                        // есть такая связь - ползём по связям по дереву вверх
                        // пока "сверху" есть следующий элемент, присваиваем результату имя текущего элемента
                        EA.Connector curConnector = connector;

                        while (curConnector != null)
                        {
                            EA.Element parent = EARepository.GetElementByID(curConnector.SupplierID);
                            EA.Connector nextConnector = null;

                            foreach (EA.Connector parentConnector in classifier.Connectors)
                            {
                                if (LTHelper.GetConnectorType(connector) == LinkType.SoftwareClassification && connector.ClientEnd == classifier)
                                {
                                    nextConnector = parentConnector;
                                    break;
                                }

                            }

                            if (nextConnector != null)
                            {
                                result = parent.Name;
                            }

                            curConnector = nextConnector;
                        }
                    }
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
                            nodeData.GroupNames = EATVHelper.GetTaggedValue(curElement, DAConst.DP_NodeGroupsTag).Split(',');
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
        /// Возвращает перечисление со списком возможных групп узлов из заведённого в библиотеке элемента
        /// </summary>
        /// <returns></returns>
        public static List<string> GetNodeGroupEnum()
        {
            List<string> result = new List<string>();

            EA.Element ngEnumElement = EARepository.GetElementByGuid(DAConst.DP_NodeGroupsEnumGUID);
            foreach (EA.Attribute attribute in ngEnumElement.Attributes)
            {
                result.Add(attribute.Name);
            }

            return result;
        }

        /// <summary>
        /// Возвращает Родительскую иерархию размещения объектов начиная от указанного
        /// </summary>
        /// <param name="eaElement"></param>
        /// <returns></returns>
        public static List<EA.Element> GetParentHierarchy(EA.Element eaElement)
        {
            List<EA.Element> result = new List<EA.Element>();

            EA.Element parentEAElement = eaElement;
            do
            {
                parentEAElement = GetDeployParent(parentEAElement);
                if (parentEAElement != null) result.Add(parentEAElement);

            } while (parentEAElement != null);

            return result;
        }

        /// <summary>
        /// Проверяет, что child размещён в parent
        /// т.е. есть библиотечный линк, направленный от child к parent , тип линка = Deploy
        /// </summary>
        /// <param name="childElement"></param>
        /// <param name="parentElement"></param>
        /// <returns></returns>
        public static bool IsDeployed(EA.Element childElement, EA.Element parentElement)
        {
            bool result = false;

            foreach (EA.Connector connector in childElement.Connectors)
            {
                if (ConnectorHelper.IsDeploymentLink(connector) && connector.ClientID == childElement.ElementID)
                {
                    result = true;
                    break;
                }
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

                EA.Collection taggedValues = EATVHelper.GetTaggedValues(element);
                try
                {

                    if (taggedValues != null && taggedValues.GetByName(DAConst.DP_LibraryTag) != null)
                    {
                        result = true;
                    }
                }
                catch
                {
                    result = false; // ибо обращение к несуществующему тэгу раизит ошибку, а проверить наличие можно только перебором тэгов, что неудобно
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

            try
            {

                if (connector.TaggedValues.GetByName(DAConst.DP_LibraryTag) != null)
                {
                    result = true;
                }
            }
            catch
            {
                result = false; // ибо обращение к несуществующему тэгу раизит ошибку, а проверить наличие можно только перебором тэгов, что неудобно
            }

            return result;
        }

        /// <summary>
        /// Устанавливает текущую библиотеку
        /// </summary>
        /// <returns></returns>
        public static ExecResult<Boolean> SetCurrentLibrary(string location)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                EA.Package libRoot = null;

                switch (location)
                {
                    case "Diagram":
                        libRoot = GetLibraryRootFromPackage(EARepository.GetPackageByID(EARepository.GetCurrentDiagram().PackageID));
                        break;
                    case "TreeView":
                    case "MainMenu":
                        libRoot = GetLibraryRootFromTreeSelection();
                        break;
                }
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
        /// Вычисляет (предлагаемый) ID инфопотока, исходящего из элемента
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string SuggestFlowIDName(EA.Element element)
        {
            string result = "";

            string flowID = EATVHelper.GetTaggedValue(element, DAConst.DP_NameForFlowIDTag, false);

            // от данного элемента лезем вверх по его  свзям SoftwareClassification и склеиваем тэги DP_NameForFlowID
            List<EA.Element> connectedElements = ConnectorHelper.GetConnectedElements(element, LinkType.SoftwareClassification, 1);
            // если у элемента нет таких зависимостей, но есть классификатор - ищем зависимости у классификатора
            if (connectedElements.Count == 0)
            {
                if (element.ClassifierID != 0)
                {
                    flowID = EATVHelper.GetTaggedValue(element, DAConst.DP_NameForFlowIDTag, false);
                    connectedElements = ConnectorHelper.GetConnectedElements(EARepository.GetElementByID(element.ClassifierID), LinkType.SoftwareClassification, 1);
                }
            }

            while (connectedElements.Count > 0)
            {
                string nextname = EATVHelper.GetTaggedValue(connectedElements[0], DAConst.DP_NameForFlowIDTag, false);
                if (nextname != "")
                {
                    if (flowID == "")
                        flowID = nextname;
                    else
                        flowID = nextname + "." + flowID;
                }

                connectedElements = ConnectorHelper.GetConnectedElements(connectedElements[0], LinkType.SoftwareClassification, 1);
            }

            // Теперь ищем подбираем номер
            int segmentNumber = 1;

            string flowIDWithNumber = flowID + " " + segmentNumber.ToString();

            throw new NotImplementedException();
            /*
            while (Context.ConnectorData[LinkType.SoftwareClassification].ContainsKey(flowIDWithNumber))
            {
                segmentNumber++;
                flowIDWithNumber = flowID + " " + segmentNumber.ToString();
            }
            */

            result = flowIDWithNumber;

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
                    return GetLibraryRootFromPackage(EARepository.GetPackageByID(curElement.PackageID));
                }
            }

            // Если через библиотечные элементы на диаграмме не получчилось, пытаемся найти от пакета диаграммы
            return GetLibraryRootFromPackage(EARepository.GetPackageByID(CurrentDiagram.PackageID));

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

            return GetLibraryRootFromPackage(libPackage);
        }



        /// <summary>
        /// Функция от указанного пакета лезет  вверх по дереву объектов репозитория и останавливается в одном из случаев:
        /// - когда по пути были библиотечные, но закончились (найден корень библиотеки)
        /// - когда по пути были библиотечных объектов не нашлось
        /// </summary>
        /// <param name="startPackage"></param>
        /// <returns>Корень библиотеки если найден</returns>
        public static EA.Package GetLibraryRootFromPackage(EA.Package startPackage)
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
        /// Возвращает Дочернюю иерархию размещения объектов начиная от указанного
        /// </summary>
        /// <param name="eaElement"></param>
        /// <returns></returns>
        public static List<EA.Element> GetChildHierarchy(EA.Element eaElement)
        {
            List<EA.Element> result = new List<EA.Element>();

            // Добавляем к результату непосредственных детей
            List<EA.Element> children = GetDeployChildren(eaElement);
            result.AddRange(children);

            // Проходимся по непосредственных детей и тянем из них рекурсией уже внуков
            foreach (EA.Element childElement in children)
            {
                List<EA.Element> grandСhildren = GetChildHierarchy(childElement);
                result.AddRange(grandСhildren);
            }

            return result;
        }

        /// <summary>
        /// Функция возвращает размер элемента, соответствующий размеру элемента на библиотечной диаграмме
        /// </summary>
        /// <returns></returns>
        public static ExecResult<Size> GetElementSizeOnLibDiagram(EA.Element element)
        {
            ExecResult<Size> result = new ExecResult<Size>() { code = -1 };

            try
            {
                // лезем от элемента вверх по дереву пакетов, пока не достигнем верха либо не достигнем пакета без тэга DP_Library после нахождения такого тэга
                EA.Package curPackage = EARepository.GetPackageByID(element.PackageID);
                bool foundPackageAfterDPLibrary = false;
                bool foundDPLibraryPackage = false;

                while (curPackage != null & !(foundPackageAfterDPLibrary))
                {
                    // Проходимся по диаграммам пакета
                    foreach (EA.Diagram curDiagram in curPackage.Diagrams)
                    {
                        // ... В в диаграмме - по объектам 
                        foreach (EA.DiagramObject diagramObject in curDiagram.DiagramObjects)
                        {
                            // если объект на диаграмме - наш объект, то срисовываем его размеры как дефолтные
                            if (diagramObject.ElementID == element.ElementID)
                            {
                                Size curSize = ElementDesignerHelper.GetSize(diagramObject);
                                result.value = curSize;
                                result.code = 0;
                                return result;
                            }
                        }

                        if (curPackage.ParentID != 0)
                            curPackage = EARepository.GetPackageByID(curPackage.ParentID);
                        else
                            curPackage = null;
                        if (LibraryHelper.IsLibrary(curPackage.Element))
                            foundDPLibraryPackage = true;
                        if (!LibraryHelper.IsLibrary(curPackage.Element) && foundDPLibraryPackage)
                            foundPackageAfterDPLibrary = true;
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
        /// Функция устанавливает размер элемента по умолчанию, соответствующий размеру элемента на библиотечной диаграмме
        /// </summary>
        /// <returns></returns>
        public static ExecResult<Boolean> SetElementDefaultSize()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();

            try
            {
                var obj = EARepository.GetContextObject();
                if (obj == null)
                {
                    throw new Exception("Нет текущего объекта");
                }
                if (!(obj is EA.Element) || !LibraryHelper.IsLibrary((EA.Element)obj))
                {
                    throw new Exception("Выделен не библиотечный элемент");
                }
                EA.Element curElement = (EA.Element)obj;

                // Ищем размер на библиотечных диаграммах
                ExecResult<Size> GetElementSizeOnLibDiagramResult = GetElementSizeOnLibDiagram(curElement);
                if (GetElementSizeOnLibDiagramResult.code != 0) throw new Exception(GetElementSizeOnLibDiagramResult.message);

                EATVHelper.TaggedValueSet(curElement, DAConst.defaultWidthTag, GetElementSizeOnLibDiagramResult.value.Width.ToString());
                EATVHelper.TaggedValueSet(curElement, DAConst.defaultHeightTag, GetElementSizeOnLibDiagramResult.value.Height.ToString());

                Logger.Out("Найден элемент диаграммы для установки размеров " + GetElementSizeOnLibDiagramResult.value.Width.ToString() + "x" + GetElementSizeOnLibDiagramResult.value.Height.ToString());

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }


        /// <summary>
        /// Возвращает список идентификаторов пакетов текущей библиотеки
        /// </summary>
        /// <returns></returns>
        public static int[] GetCurrentLibPackageIDs()
        {
            List<int> result = new List<int>();

            string[] args = new string[] { Context.CurrentLibrary.PackageGUID };
            XDocument sqlResultSet = SQLHelper.RunSQL("PackageHierarchy.sql", args);

            IEnumerable<XElement> rowNodes = sqlResultSet.Root.XPathSelectElements("/EADATA/Dataset_0/Data/Row");
            foreach (XElement rowNode in rowNodes)
            {
                int package_id = int.Parse(rowNode.Descendants("package_id").First().Value);
                result.Add(package_id);
            }

            return result.ToArray();

        }



    }
}



