﻿using EADiagramPublisher.Enums;
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


        public int _ElementID { get; set; }
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


    }
}
