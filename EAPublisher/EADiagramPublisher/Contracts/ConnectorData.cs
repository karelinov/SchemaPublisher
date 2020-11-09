using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EADiagramPublisher.Enums;

namespace EADiagramPublisher.Contracts
{
    public class ConnectorData
    {
        public ConnectorData()
        {
            ConnectorID = 0;
            IsLibrary = false;
        }



        public int ConnectorID { get; set; }
        public EA.Connector Connector
        {
            get
            {
                if (ConnectorID != 0)
                    return Context.EARepository.GetConnectorByID(ConnectorID);
                else return null;
            }
            set
            {
                ConnectorID = value.ConnectorID;
            }
        }
        public bool IsLibrary { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public LinkType LinkType { get; set; }
        public string FlowID { get; set; }
        public string SegmentID { get; set; }

        public int SourceElementID { get; set; }
        public int TargetElementID { get; set; }

        public string NameForShow()
        {
            string result = Name;
            /*
            if (result == "" && IsLibrary)
            {
                result = "("+ LinkType.ToString()+")";
            }
            */
            return result;
        }

        public ConnectorData(EA.DiagramLink diagramLink) : base()
        {
            ConnectorID = diagramLink.ConnectorID;

            Name = Connector.Name;
            Notes = Connector.Notes;
            LinkType = LTHelper.GetConnectorType(Connector);
            FlowID = EATVHelper.GetTaggedValue(Connector, DAConst.DP_FlowIDTag);
            SegmentID = EATVHelper.GetTaggedValue(Connector, DAConst.DP_SegmentIDTag);

            SourceElementID = Connector.ClientID;
            TargetElementID = Connector.SupplierID;
        }

        public ConnectorData(EA.Connector connector)
        {
            Connector = connector;

            Name = Connector.Name;
            Notes = Connector.Notes;
            LinkType = LTHelper.GetConnectorType(connector);
            FlowID = EATVHelper.GetTaggedValue(connector, DAConst.DP_FlowIDTag);
            SegmentID = EATVHelper.GetTaggedValue(connector, DAConst.DP_SegmentIDTag);

            SourceElementID = connector.ClientID;
            TargetElementID = connector.SupplierID;
        }

    }
}
