using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EADiagramPublisher.Forms
{
    public partial class FRunSQLQuery : Form
    {
        public FRunSQLQuery()
        {
            InitializeComponent();
        }


        public static ExecResult<bool> Execute()
        {
            var result = new ExecResult<bool>();

            try
            {


                var fRunSQLQuery = new FRunSQLQuery();
                DialogResult res = fRunSQLQuery.ShowDialog();

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string queryResult;
            try
            {
                queryResult = EAHelper.RunQuery(tbQuery.Text);
            }
            catch(Exception ex)
            {
                queryResult = ex.StackTrace;
            }

            tcQuery.SelectedTab = tpResults;

            var path = Path.GetTempPath();
            var fileName = Path.Combine(path, Guid.NewGuid().ToString() + ".xml");
            File.WriteAllText(fileName, queryResult); //xmlText is your xml string
            wbResults.Navigate(fileName); //"navigate" to the file webBrowser is your WebBrowser control
            //File.Delete(fileName);

        }
    }
}
