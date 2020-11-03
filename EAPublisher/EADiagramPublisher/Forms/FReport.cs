using Microsoft.Reporting.WinForms;
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
    public partial class FReport : Form
    {
        public FReport()
        {
            InitializeComponent();
        }

        public static ExecResult<bool> Execute(string rdlcFile, List<ReportDataSource> reportDataSources, List<ReportParameter> reportParameters)
        {
            var result = new ExecResult<bool>();
            try
            {
                var form = new FReport();


                // устанавливаем файл отчёта
                form.reportViewer1.LocalReport.EnableExternalImages = true;
                form.reportViewer1.LocalReport.ReportPath = rdlcFile;

                // Подключаем источники данных
                form.reportViewer1.LocalReport.DataSources.Clear();
                foreach (ReportDataSource reportDataSource in reportDataSources)
                {
                    form.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                }
                // Передаём параметры
                form.reportViewer1.LocalReport.SetParameters(reportParameters);

                // Запускаем отчёт
                form.reportViewer1.RefreshReport();

                DialogResult dr = form.ShowDialog();
                result.code = 0;
                result.value = true;

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }



    }
}
