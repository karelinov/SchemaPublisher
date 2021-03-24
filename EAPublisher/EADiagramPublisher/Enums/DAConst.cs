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

        public static string DP_LibraryTag = "DP_Library"; // Идентифицирует объект как библиотечный
        public static string DP_ComponentLevelTag = "DP_ComponentLevel"; // Уровень библиотечного элемента

        
        public static string DP_LinkTypeTag = "DP_LinkType"; // Тип связи (LinkType) 
        public static string DP_FlowIDTag = "DP_FlowID"; // Идентифифкатор инфопотока
        public static string DP_SegmentIDTag = "DP_FlowSegmentID"; // Номер сегомента инфопотока
        public static string DP_NameForFlowIDTag = "DP_NameForFlowID"; // Элемент имени (в добавление к, например, имени ПК), которое будет предложено для инфопотока для данного компонента

        public static string DP_NodeGroupsTag = "DP_NodeGroups"; // Список групп, в которые входит узел
        public static string DP_NodeGroupsEnumGUID = "{C30DA53D-A2AC-4b4c-BB20-31681C9E2671}"; // Идентификатор перечисления, в котором лежит список испольуземых групп

        public static string DP_TempLinkDiagramIDTag = "DP_TempLinkDiagramID"; // Идентификатор диаграммы, на которой следует показать этот линк
        public static string DP_TempLinkTag = "DP_TempLink"; // признак временной связи (созданной чтобы показать инфопоток отсутствующих на диаграмме элементов)

        public static string DP_SoftwareIDTag = "DP_SoftwareID"; // (уникальный только в комплексе всей цепочки идентификаторов ПО) Идентификатор ПО в ПК (декларирует, что компонент является классификатором ПО (используется в связке с LinkType = SoftwareClassification))


        public static string[] StandardTags
        {
            get
            {
                return new string[] { DP_LibraryTag, DP_ComponentLevelTag, defaultWidthTag, defaultHeightTag, DP_LinkTypeTag, DP_FlowIDTag, DP_SegmentIDTag, DP_NodeGroupsTag, DP_TempLinkDiagramIDTag, DP_TempLinkTag, DP_SoftwareIDTag };
            }
        }

    }
}
