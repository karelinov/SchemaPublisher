using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Enums
{
    public enum ComponentLevel:int
    {
        SystemContour = 1,
        SystemComponent = 2,
        ContourContour = 3,
        ContourComponent= 4,
        Node = 5,
        Device = 6,
        ExecutionEnv = 7,
        Component = 8
    }
}
