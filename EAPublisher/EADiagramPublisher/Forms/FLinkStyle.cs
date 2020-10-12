using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;

namespace EADiagramPublisher.Forms
{
    public partial class FLinkStyle : Form
    {
        public FLinkStyle()
        {
            InitializeComponent();

            cbLineStyle.SelectedIndex = 0;

        }

        public ExecResult<SetLinkStyle> Execute()
        {
            ExecResult<SetLinkStyle> result = new ExecResult<SetLinkStyle>();
            try
            {

                DialogResult res = this.ShowDialog();
                if (res == DialogResult.OK)
                {
                    result.code = 0;
                    result.value = new SetLinkStyle();

                    result.value.DoSetLinkStyle = chkSetLinkStyle.Checked;
                    // Линия и Цвет

                    result.value.SetLineWidth = chkSetLineSize.Checked;
                    result.value.LineWidth = (int)nudLineSize.Value;

                    result.value.SetColor = chkSetColor.Checked;
                    result.value.Color = pbColor.BackColor;

                    result.value.SetLineStyle = chkSetLineStyle.Checked;
                    if (cbLineStyle.SelectedIndex == 0)
                        result.value.LineStyle = EA.LinkLineStyle.LineStyleOrthogonalRounded;
                    else
                        result.value.LineStyle = EA.LinkLineStyle.LineStyleDirect; 


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




        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            DialogResult dialogResult = colorDialog.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                pbColor.BackColor = colorDialog.Color;
            }
        }

        private void cbSetShowStyle_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSetLinkStyle.Checked)
            {
                chkSetColor.Enabled = true;
                chkSetLineSize.Enabled = true;
                chkSetLineStyle.Enabled = true;
            }
            else
            {
                chkSetColor.Enabled = false;
                chkSetLineSize.Enabled = false;
                chkSetLineStyle.Enabled = false;

            }
        }
    }
}
