using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EADiagramPublisher.Enums;

namespace EADiagramPublisher.Forms
{
    public partial class FSetLinkVisibility : Form
    {
        public FSetLinkVisibility()
        {
            InitializeComponent();

            this.clbShowLinkType.Items.Clear();
            foreach (LinkType linkType in (LinkType[])Enum.GetValues(typeof(LinkType)))
            {
                this.clbShowLinkType.Items.Add(linkType, false);
            }


            this.clbHideLinkType.Items.Clear();
            foreach (LinkType linkType in (LinkType[])Enum.GetValues(typeof(LinkType)))
            {
                this.clbHideLinkType.Items.Add(linkType, false);
            }


        }

        public ExecResult<LinkVisibilityData> Execute()
        {
            ExecResult<LinkVisibilityData> result = new ExecResult<LinkVisibilityData>() { value = new LinkVisibilityData() };
            try
            {
                DialogResult res = this.ShowDialog();
                if (res == DialogResult.OK)
                {
                    result.code = 0;

                    foreach (var checkedItem in clbShowLinkType.CheckedItems)
                    {
                        result.value.showLinkType.Add((LinkType)checkedItem);
                    }
                    result.value.showNotLibElements = cbShowNotLibConnectors.Checked;

                    foreach (var checkedItem in clbHideLinkType.CheckedItems)
                    {
                        result.value.hideLinkType.Add((LinkType)checkedItem);
                    }
                    result.value.hideTempDiagramLinks = cbHideTempDiagramLinks.Checked;
                }
                else
                {
                    result.code = (int)DialogResult.Cancel;
                }

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

    }

    /// <summary>
    /// Класс с выбранными опциями показа линков
    /// </summary>
    public class LinkVisibilityData
    {
        public List<LinkType> showLinkType = new List<LinkType>(); // список типов линков , которые надо показать
        public bool showNotLibElements = false; // показывать небиблиотечные линки

        public List<LinkType> hideLinkType = new List<LinkType>(); // список типов линков , которые надо скрыть
        public bool hideTempDiagramLinks = false; // скрывать временые линки других диаграмм
    }


}
