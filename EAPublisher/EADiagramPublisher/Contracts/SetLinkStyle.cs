﻿using EADiagramPublisher.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Contracts
{
    public class SetLinkStyle
    {
        public bool DoSetLinkStyle;

        public bool SetLineWidth;
        public int LineWidth;

        public bool SetColor;
        public Color Color;

        public bool SetLineStyle;
        public EA.LinkLineStyle LineStyle;
    }
}
