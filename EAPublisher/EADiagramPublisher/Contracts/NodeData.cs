using EADiagramPublisher.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher.Contracts
{
    /// <summary>
    /// Класс для выбора элементов типа 
    /// </summary>
    public class NodeData
    {
        public EA.Element Element;
        public ComponentLevel ComponentLevel;
        public EA.Element Contour;
        public string[] GroupNames;

        public string ElementName
        {
            get
            {
                string result = "";

                if (Element != null)
                {
                    result = Element.Name;

                    if (Element.ClassfierID != 0)
                    {
                        EA.Element classifier = Context.EARepository.GetElementByID(Element.ClassfierID);
                        result += ":" + classifier.Name;
                    }
                }
                else if (Contour != null)
                {
                    result = Contour.Name;
                }
                else
                    result = ComponentLevel.ToString();

                return result;
            }
        }
    }
}
