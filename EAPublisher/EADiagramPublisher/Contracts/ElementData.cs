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
    class ElementData
    {
        public int _ElementID = 0;
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
        public string Name;
        public string EAType;
        public string Note;
        public int? ClassifierID = null;
        public string ClassifierName;
        public string ClassifierEAType;


        // тэги
        public bool IsLibrary;
        public ComponentLevel? ComponentLevel = null;
        public string[] NodeGroups = null;

        public string DisplayName
        {
            get
            {
                string result = Name;
                if (ClassifierID != 0)
                {
                    result = ":" + ClassifierName;
                }


                return result;
            }
        }

    }
}
