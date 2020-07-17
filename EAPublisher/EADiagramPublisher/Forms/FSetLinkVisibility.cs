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

        public ExecResult<SelectedLinkVisibility> Execute()
        {
            ExecResult<SelectedLinkVisibility> result = new ExecResult<SelectedLinkVisibility>() { value = new SelectedLinkVisibility() };
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
                    foreach (var checkedItem in clbHideLinkType.CheckedItems)
                    {
                        result.value.hideLinkType.Add((LinkType)checkedItem);
                    }

                    result.value.showNotLibElements = cbShowNotLibConnectors.Checked;
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
    public class SelectedLinkVisibility
    {
        public List<LinkType> showLinkType = new List<LinkType>(); // список типов линков , которые надо показать
        public List<LinkType> hideLinkType = new List<LinkType>(); // список типов линков , которые надо скрыть
        public bool showNotLibElements = false; // показывать небиблиотечные линки
    }


}
