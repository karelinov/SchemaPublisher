using EADiagramPublisher.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Contracts
{
    /// <summary>
    /// Класс со свойдной информацией об элементе, для верменного хранения без необходимости лазать по медленным COM-объектам
    /// </summary>
    public class ElementData : IDPContractWithID
    {
        public ElementData()
        {
            _ElementID = 0;
            ComponentLevel = null;
            NodeGroups = null;
        }

        public ElementData(EA.Element element)
        {
            _ElementID = element.ElementID;
            Name = element.Name;
            EAType = element.Type;

            if (element.ClassifierID != 0)
            {
                ClassifierID = element.ClassifierID;
                EA.Element classifier = Context.EARepository.GetElementByID((int)ClassifierID);
                ClassifierName = classifier.Name;
                ClassifierEAType = classifier.Type;
            }

            IsLibrary = LibraryHelper.IsLibrary(element);
            if (IsLibrary)
            {

                ComponentLevel =  CLHelper.GetComponentLevel(element);
                string ngTag = EATVHelper.GetTaggedValue(element, DAConst.DP_NodeGroupsTag);
                if (ngTag != null)
                    NodeGroups = ngTag.Split(',');
            }
        }


        public int _ElementID;
        public EA.Element Element
        {
            get
            {
                if (_ElementID != 0)
                    return Context.EARepository.GetElementByID(_ElementID);
                else return null;
            }
            set
            {
                _ElementID = value.ElementID;
            }
        }
        public string Name { get; set; }
        public string EAType { get; set; }
        public string Note { get; set; }
        public int? ClassifierID = null;
        public string ClassifierName { get; set; }
        public string ClassifierEAType { get; set; }


        // тэги
        public bool IsLibrary { get; set; }
        public ComponentLevel? ComponentLevel { get; set; }
        public string[] NodeGroups { get; set; }

        public string DisplayName
        {
            get
            {
                string result = Name;
                if (ClassifierID != null)
                {
                    result += ":" + ClassifierName;
                }


                return result;
            }
        }

        public int ID
        {
            get
            {
                return this._ElementID;
            }
        }

        /// <summary>
        /// Список идентификаторов коннекторов элемента
        /// </summary>
        public List<int> _ConnectorDataIDs = null;
        public ConnectorData[] ConnectorsData
        {
            get
            {
                if (_ConnectorDataIDs == null)
                    return null;
                else
                {
                    return Context.ConnectorData.Where(cd => _ConnectorDataIDs.Contains(cd.Key)).Select(cd => cd.Value).ToArray();
                }

            }

        }

        // Свойства для отчётов

        /// <summary>
        /// ID узла, в котором размещён компонент (для группировки компонентов в узлах)
        /// </summary>
        public int? RootDeployNodeID { get; set; }



    }
}
