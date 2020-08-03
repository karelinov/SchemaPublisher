using EADiagramPublisher.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Contracts
{
    public class LinksOperationData
    {
        public LinkSOperation Operation;
        public Color Color;
        public int LineSize;

        public List<ConnectorData> Connectors;
    }
}
