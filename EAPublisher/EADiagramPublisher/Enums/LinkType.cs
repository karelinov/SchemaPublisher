using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Enums
{
    public enum LinkType : int
    {
        /// <summary>
        /// Размещение компонента внутри другого компонента
        /// </summary>
        Deploy = 0,
        /// <summary>
        /// Коммуникация копонентов (узлов, неспецифицированный обмен данными)
        /// </summary>
        Communication = 1,
        /// <summary>
        /// Инфопоток (може также дополнительно характеризоваться номером инфопотока и сегмента)
        /// </summary>
        InformationFlow = 2,
        /// <summary>
        /// Классификация ПО (по программным комплексам и компонентам)
        /// </summary>
        SoftwareClassification = 3
    }
}
