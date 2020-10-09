using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Enums
{
    class LTHelper
    {
        public static LinkType GetConnectorType(EA.Connector connector)
        {
            try
            {
                return (LinkType)Enum.Parse(typeof(LinkType), ((EA.ConnectorTag)connector.TaggedValues.GetByName(DAConst.DP_LinkTypeTag)).Value);
            }
            catch (Exception ex)
            {
                Logger.Out("Ошибка в определении типа коннектора " + ex.StackTrace, new EA.Connector[] { connector });
                throw ex;
            }

        }
    }
}
