using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EADiagramPublisher.Enums;

namespace EADiagramPublisher.Contracts
{
    public class ConnectorData
    {
        private int _ConnectorID = 0;
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
        public string Name;
        public LinkType LinkType;
        public string FlowID;
        public string SegmentID;

        public int SourceElementID;
        public int DestinationElementID;
    }
}
