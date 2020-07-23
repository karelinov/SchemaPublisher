using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Enums
{
    /// <summary>
    /// Константы плагина
    /// </summary>
    public class DAConst
    {
        public static string DP_LibraryTag = "DP_Library";


        public static int defaultWidth = 100;
        public static int defaultHeight = 50;

        public static String defaultWidthTag = "DP_DefaultWidth";
        public static String defaultHeightTag = "DP_DefaultHeight";

        public static int border = 20;


        // Константы LinkType
        public static string DP_LinkTypeTag = "DP_LinkType";
        public static string DP_FlowIDTag = "DP_FlowID";
        public static string DP_SegmentIDTag = "DP_FlowSegmentID";
        public static string DP_TempLinkTag = "DP_TempLink";
        public static string DP_TempLinkDiagramIDTag = "DP_TempLinkDiagramID"; // Идентификатор диаграммы, на которой следует показать этот линк
    }
}
