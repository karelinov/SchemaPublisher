using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher
{
    class Context
    {
        /// <summary>
        /// Открытый плагином EA.Repository
        /// </summary>
        public static EA.Repository EARepository { get; set; }

        /// <summary>
        /// Установленная текущая рабочая диаграмма
        /// </summary>
        public static EA.Diagram CurrentDiagram { get; set; }

    }



}
