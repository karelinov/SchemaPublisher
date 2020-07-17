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
    public partial class FCreateNewLink : Form
    {
        public FCreateNewLink()
        {
            InitializeComponent();


            this.clbLinkType.Items.Clear();
            this.clbLinkType.Items.Add(LinkType.Deploy, false);
            this.clbLinkType.Items.Add(LinkType.Communication, false);
        }


        public ExecResult<LinkType> Execute()
        {
            ExecResult<LinkType> result = new ExecResult<LinkType>();
            try
            {
                DialogResult res = this.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    if(clbLinkType.CheckedItems.Count == 0)
                    {
                        result.code = (int)DialogResult.Cancel;
                    }
                    else
                    {
                        result.value = ((LinkType)clbLinkType.CheckedItems[0]);
                    }
                }

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        private void clbLinkType_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index == 0)
            {
                if (e.NewValue == CheckState.Checked)
                    ChangeAllCheckBoxValues(true);
                else
                    ChangeAllCheckBoxValues(false);
            }
        }

        private void ChangeAllCheckBoxValues(bool value)
        {
            for (int i = 1; i < clbLinkType.Items.Count; i++)
            {
                clbLinkType.SetItemChecked(i, value);
            }
        }
    }
}
