using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Класс для установки значений TaggedValues
/// </summary>
namespace EADiagramPublisher.Contracts
{
    public class TagData
    {
        public bool TagState; // true = установлен false = не установлен
        public string TagName;
        public string TagValue;

        public int Count = 0; // количество элементов с данным тэгом
        public bool Ex = false; // признак, что Tag унаследован

        /// <summary>
        /// Вычисляемое своейство, говорит о том, когда Tags можно редактировать
        /// </summary>
        public bool Enabled 
        {
            get
            {
                return Ex == false;
            }
        }

    }
}
