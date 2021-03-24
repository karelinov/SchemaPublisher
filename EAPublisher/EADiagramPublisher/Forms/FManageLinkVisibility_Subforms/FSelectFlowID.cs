using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EADiagramPublisher.Forms
{
    public partial class FSelectFlowID : Form
    {
        public FSelectFlowID()
        {
            InitializeComponent();

            clbFlowIDs.Items.Clear();
            clbFlowIDs.Items.AddRange(ConnectorHelper.GetCurrentFlowIDs());
        }

        public static ExecResult<string[]> Execute()
        {
            var result = new ExecResult<string[]>();
            try
            {
                var form = new FSelectFlowID();
                DialogResult res = form.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    List<string> selectedFlowIDs = new List<string>();
                    foreach (string selectedFlowID in form.clbFlowIDs.SelectedItems)
                    {
                        selectedFlowIDs.Add(selectedFlowID);
                    }

                    result.value = selectedFlowIDs.ToArray();
                }
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbFlowIDs.Items.Count; i++)
            {
                clbFlowIDs.SetItemChecked(i, true);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbFlowIDs.Items.Count; i++)
            {
                clbFlowIDs.SetItemChecked(i, false);
            }
        }
    }


}
