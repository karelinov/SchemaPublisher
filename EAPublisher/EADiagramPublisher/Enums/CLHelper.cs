using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Enums
{
    class CLHelper
    {
        public static string DP_ComponentLevel = "DP_ComponentLevel";
        public static string Component_System_Stereotype = "система";
        public static string Component_Contour_Stereotype = "контур";


        /// <summary>
        /// Функция возвращает уровень библиотечного компонента, которому соответствует элемент
        /// </summary>
        /// <param name="eaElement"></param>
        /// <returns></returns>
        public static ComponentLevel GetComponentLevel(EA.Element eaElement)
        {
            try
            {

                if (!EAHelper.IsLibrary(eaElement)) throw new Exception("компонент не является библиотечным");

                if (eaElement.Type == "Boundary")
                {
                    if (EAHelper.GetTaggedValues(eaElement).GetByName(DP_ComponentLevel) == null || EAHelper.GetTaggedValues(eaElement).GetByName(DP_ComponentLevel).Value == "")
                        throw new Exception("Не определён уровень компонента контура ");

                    //EA.TaggedValue taggedValue = GetTaggedValues(eaElement).GetByName(DP_ComponentLevel);
                    return (ComponentLevel)Enum.Parse(typeof(ComponentLevel), EAHelper.GetTaggedValues(eaElement).GetByName(DP_ComponentLevel).Value);
                }
                else if (EAHelper.GetTaggedValues(eaElement).GetByName(DP_ComponentLevel) != null)
                {
                    return Enum.Parse(typeof(ComponentLevel), EAHelper.GetTaggedValues(eaElement).GetByName(DP_ComponentLevel).Value);
                }
                else if (eaElement.Type == "Component")
                {
                    if (eaElement.Stereotype == Component_System_Stereotype)
                        return ComponentLevel.SystemComponent;
                    else if (eaElement.Stereotype == Component_Contour_Stereotype)
                        return ComponentLevel.ContourComponent;
                    else
                        return ComponentLevel.Component;
                }
                else if (eaElement.Type == "Node")
                {
                    return ComponentLevel.Node;
                }
                else if (eaElement.Type == "Device")
                {
                    return ComponentLevel.Device;
                }
                else if (eaElement.Type == "ExecutionEnvironment")
                {
                    return ComponentLevel.ExecutionEnv;
                }
                else
                    throw new Exception("неизветсный тип элемента");


            }
            catch (Exception ex)
            {
                EAHelper.Out(ex.ToString(), new EA.Element[] { eaElement });
                throw ex;
            }

        }

    }
}
