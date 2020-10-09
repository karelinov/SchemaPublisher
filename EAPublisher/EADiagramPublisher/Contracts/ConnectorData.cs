using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EADiagramPublisher.Enums;

namespace EADiagramPublisher.Contracts
{
    public class ConnectorData
    {
        public int _ConnectorID = 0;
        public EA.Connector Connector
        {
            get
            {
                if (_ConnectorID != 0)
                    return Context.EARepository.GetConnectorByID(_ConnectorID);
                else return null;
            }
            set
            {
                _ConnectorID = value.ConnectorID;
            }
        }
        public bool IsLibrary = false;
        public string Name;
        public LinkType LinkType;
        public string FlowID;
        public string SegmentID;

        public int SourceElementID;
        public int TargetElementID;

        public string NameForShow()
        {
            string result = Name;
            if (result == "")
            {
                result = LinkType.ToString();
            }


            if (SourceElementID != 0)
            {
                result += " " + Logger.DumpObject(Context.EARepository.GetElementByID(SourceElementID));
            }
            if (TargetElementID != 0)
            {
                result += "-" + Logger.DumpObject(Context.EARepository.GetElementByID(TargetElementID));
            }

            return result;
        }

        public ConnectorData(EA.DiagramLink diagramLink)
        {
            _ConnectorID = diagramLink.ConnectorID;

            Name = Connector.Name;
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
            LinkType = LTHelper.GetConnectorType(connector);
            FlowID = EATVHelper.GetTaggedValue(connector, DAConst.DP_FlowIDTag);
            SegmentID = EATVHelper.GetTaggedValue(connector, DAConst.DP_SegmentIDTag);

            SourceElementID = connector.ClientID;
            TargetElementID = connector.SupplierID;
        }

        public ConnectorData()
        {

        }

    }
}
