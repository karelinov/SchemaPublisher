using EADiagramPublisher.Contracts;
using EADiagramPublisher.Forms;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EADiagramPublisher
{
    class ReportsHelper
    {
        private static EA.Repository EARepository
        {
            get
            {
                return Context.EARepository;
            }
        }

        public static ExecResult<bool> DiagramElementsReport(string location)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();

            try
            {
                // Получаем диаграмму
                EA.Diagram reportDiagram = null;

                switch (location)
                {
                    case "Diagram":
                        reportDiagram = EARepository.GetCurrentDiagram();
                        break;

                    case "MainMenu":
                    case "TreeView":
                        if (EARepository.GetTreeSelectedItemType() != EA.ObjectType.otDiagram)
                            throw new Exception("Не выбрана диаграмма в ProjectExplorer");

                        reportDiagram = EARepository.GetTreeSelectedObject();
                        break;
                }

                // Проверяем, что это диаграмма - библиотечная
                EA.Package diagramPackage = EARepository.GetPackageByID(reportDiagram.PackageID);
                EA.Package libPackage = LibraryHelper.GetLibraryRootFromPackage(diagramPackage);
                if (libPackage == null)
                    throw new Exception("Диаграмма не в библиотеке");

                // Устанавливаем текущую библиотеку
                if (Context.CurrentLibrary.PackageID != libPackage.PackageID)
                    Context.CurrentLibrary = libPackage;

                // Устанавливаем текущую диаграмму
                Context.CurrentDiagram = reportDiagram;


                // Подготавливаем имя RDLC - файла отчёта
                string fullReportName = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Reports", "LibDiagram.rdlc");
                if (!File.Exists(fullReportName)) throw new Exception("файл отчёта " + fullReportName + "не найден");

                // Подготавливаем источники данных для отчёта
                List<ReportDataSource> reportDataSources = new List<ReportDataSource>();

                List<ElementData> elementDataList = EAHelper.GetCurDiagramElementData().Values.ToList<ElementData>();
                foreach (var elementData in elementDataList) // Дообогащаем информацию об элементах данными узла размещения
                {
                    int? rootDeployNodeID = LibraryHelper.GetDeployComponentNode(elementData._ElementID);
                    if (rootDeployNodeID != null)
                        elementData.RootDeployNodeID = rootDeployNodeID;
                }

                ReportDataSource reportDataSource = new ReportDataSource("DS_ElementData", elementDataList);
                reportDataSources.Add(reportDataSource);

                List<ConnectorData> connectorDataList = EAHelper.GetCurDiagramConnectors();
                ReportDataSource reportDataSource1 = new ReportDataSource("DS_ConnectorData", connectorDataList);
                reportDataSources.Add(reportDataSource1);


                // Подготавливаем параметры отчёта
                List<ReportParameter> reportParameters = new List<ReportParameter>();
                // Картинка диаграммы
                string SavedDiagramImagePath = DiagramExporter.ExportPNG(reportDiagram);
                //string base64Image = Convert.ToBase64String(File.ReadAllBytes(SavedDiagramImagePath));
                reportParameters.Add(new ReportParameter("paramDiagramImage", "file:///" + SavedDiagramImagePath));


                // запускаем форму
                ExecResult<bool> fReportResult = FReport.Execute(fullReportName, reportDataSources, reportParameters);
                if (fReportResult.code != 0)
                    throw new Exception(fReportResult.message);
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;



        }


    }
}
