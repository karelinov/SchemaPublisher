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
    public partial class FInputString : Form
    {
        public FInputString()
        {
            InitializeComponent();
        }

        public ExecResult<string> Execute(string defaultValue)
        {
            ExecResult<string> result = new ExecResult<string>();
            try
            {
                tbText.Text = defaultValue;

                DialogResult res = this.ShowDialog();
                if (res == DialogResult.OK)
                {
                    result.code = 0;
                    result.value = tbText.Text;
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
}
