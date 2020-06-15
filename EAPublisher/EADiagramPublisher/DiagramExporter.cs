using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EADiagramPublisher
{
    public static class DiagramExporter
    {
        public static ExecResult<bool> Export(string location)
        {
            var result = new ExecResult<bool>();

            try
            {
                EA.Diagram curDiagram = null;
                if (location == "Diagram" || location == "MainMenu")
                {
                    curDiagram = DPAddin.EARepository.GetCurrentDiagram();
                }
                else if (location == "TreeView")
                {
                    curDiagram = DPAddin.EARepository.GetTreeSelectedObject();
                }

                // Экспортируем PNG
                string pngSavePath = ExportPNG(curDiagram);

                // Экспортируем Текст
                string txtSavePath = ExportText(curDiagram);

                // Создаём .properties файл для публикации
                String propSavePath = Path.Combine(DPConfig.AppSettings["exchangePath"].Value, "export.properties");
                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(propSavePath))
                {
                    String guidStr = curDiagram.DiagramGUID.Replace("{", "").Replace("}", "");
                    streamWriter.WriteLine("OBJID="+ curDiagram.DiagramGUID.Replace("{","").Replace("}",""));
                    streamWriter.WriteLine("PARENTPAGE="+ DPConfig.AppSettings["parentPage"].Value);
                    streamWriter.WriteLine("PAGEGENAME="+ guidStr+" "+curDiagram.Name);
                    streamWriter.WriteLine("IMAGE=" + Path.GetFileName(pngSavePath));
                    streamWriter.WriteLine("TEXT=" + Path.GetFileName(txtSavePath));
                }

                // Запусаем java - публикатор
                Process process = new Process();
                process.StartInfo.FileName = Path.Combine(DPConfig.AppSettings["javaRuntime"].Value,"javaw.exe");
                process.StartInfo.Arguments = DPConfig.AppSettings["javaArguments"].Value+" "+ propSavePath+"";
                //process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.UseShellExecute = false;
                process.Start();

                process.WaitForExit();

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }



            return result;
        }

        private static string ExportText(EA.Diagram curDiagram)
        {
            String text = curDiagram.Notes;

            // Сохраняем на диск 
            string txtSavepath = Path.Combine(DPConfig.AppSettings["exchangePath"].Value, curDiagram.DiagramGUID.Replace("{", "").Replace("}", "") + ".txt");
            if (File.Exists(txtSavepath)) File.Delete(txtSavepath);
            System.IO.File.WriteAllLines(txtSavepath, new String[] { text });

            // пытаемся сразу отпустить файлы, а то были случаи
            GC.Collect();

            return txtSavepath;
        }

        private static string ExportPNG(EA.Diagram curDiagram)
        {
            // Сохраняем на диск метафайл
            string savePath = Path.Combine(DPConfig.AppSettings["exchangePath"].Value, curDiagram.DiagramGUID.Replace("{", "").Replace("}", "") + ".emf");
            if (File.Exists(savePath)) File.Delete(savePath);
            DPAddin.EARepository.GetProjectInterface().PutDiagramImageToFile(curDiagram.DiagramGUID, savePath, 0);

            // Загружаем метафайл и пересохраняем в PNG в рассчитанном масштабе
            Metafile metafile = new Metafile(savePath);
            Size newsize = CalculateScale(metafile.Size); // получаем новый размер

            Bitmap bitmap = new Bitmap(metafile, newsize);
            string pngSavePath = Path.ChangeExtension(savePath, ".png");
            if (File.Exists(pngSavePath)) File.Delete(pngSavePath);

            bitmap.Save(pngSavePath, ImageFormat.Png);

            // пытаемся сразу отпустить файлы, а то были случаи
            metafile.Dispose();
            bitmap.Dispose();
            GC.Collect();

            return pngSavePath;
        }


        private static Size CalculateScale(Size originalSize)
        {
            // Надо отмасштабировать картинку
            // стратегия: ширина: < 600 = 800, 600-1500 = 1000, в остальных случаях = 2000
            decimal scalefactor = 1;
            if (originalSize.Width < 600)
            {
                scalefactor = (decimal)800 / originalSize.Width;
            }
            else if(originalSize.Width >= 600 && originalSize.Width <=800)
            {
                scalefactor = (decimal)1000 / originalSize.Width;
            }
            else
            {
                scalefactor = (decimal)2000 /originalSize.Width;
            }

            return new Size(Decimal.ToInt32(Math.Round(originalSize.Width * scalefactor)), Decimal.ToInt32(Math.Round(originalSize.Height * scalefactor)));

        }


    }
}
