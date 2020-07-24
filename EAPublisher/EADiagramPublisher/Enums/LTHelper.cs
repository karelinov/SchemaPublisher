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
            return (ConnectorType)Enum.Parse(typeof(ConnectorType), ((EA.ConnectorTag)connector.TaggedValues.GetByName(DAConst.DP_LinkTypeTag)).Value);
        }


    }
}
