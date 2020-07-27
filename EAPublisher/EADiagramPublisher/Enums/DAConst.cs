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
        public static int border = 20;
        public static int defaultHeight = 50;
        public static String defaultHeightTag = "DP_DefaultHeight";
        public static int defaultWidth = 100;
        public static String defaultWidthTag = "DP_DefaultWidth";

        public static string DP_ComponentLevelTag = "DP_ComponentLevel";

        public static string DP_FlowIDTag = "DP_FlowID";
        public static string DP_LibraryTag = "DP_Library";
        // Константы LinkType
        public static string DP_LinkTypeTag = "DP_LinkType";
        public static string DP_SegmentIDTag = "DP_FlowSegmentID";
        public static string DP_TempLinkDiagramIDTag = "DP_TempLinkDiagramID";
        public static string DP_TempLinkTag = "DP_TempLink";
        // Идентификатор диаграммы, на которой следует показать этот линк


        public static string[] StandardTags
        {
            get
            {
                return new string[] { DP_LibraryTag, DP_ComponentLevelTag, defaultWidthTag, defaultHeightTag, DP_LinkTypeTag, DP_FlowIDTag, DP_SegmentIDTag };
            }
        }

    }
}
