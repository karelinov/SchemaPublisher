using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Enums
{
    public enum LinkSOperation: int
    {
        /// <summary>
        /// Сброс астроек вида - все линки чёрные толщина = 1
        /// </summary>
        ResetAll = 0,
        /// <summary>
        /// Установить указанные цвет и толщину
        /// </summary>
        SetStyle = 1,

    }
}
