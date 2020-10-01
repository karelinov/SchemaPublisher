using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace EADiagramPublisher
{
    public class Logger
    {
        /// <summary>
        /// Shortcut до глобальной переменной с EA.Repository
        /// </summary>
        private static EA.Repository EARepository
        {
            get
            {
                return Context.EARepository;
            }
        }



        private static bool writelog = true;

        /// <summary>
        /// OUT в консоль спаркса
        /// </summary>        
        private static bool writeout = false;



        /// <summary>
        /// Выводит в лог /+ консоль сообщение+список объектов
        /// </summary>
        /// <param name="outStr"></param>
        /// <param name="objectsToOUT"></param>
        /// <param name="tabname"></param>
        /// <param name="addCallername"></param>
        public static void Out(string outStr, Object[] objectsToOUT = null, string tabname = "System", bool addCallername = true)
        {
            string result = DateTime.Now.ToString("HH:mm:sss.fff ");
            string resultLevel = new StringBuilder(DesignerHelper.CallLevel).Insert(0, " ", DesignerHelper.CallLevel).ToString();

            if (writeout || writelog)
            {
                if (objectsToOUT != null)
                    result += DumpObjects(objectsToOUT);

                if (addCallername)
                    result = new StackFrame(1, true).GetMethod().Name + ":" + outStr + " " + result;
            }

            result = resultLevel + result;

            if (writeout)
            {
                EARepository.WriteOutput("System", resultLevel + result, 0);
            }

            if (writelog)
                using (StreamWriter outputFile = new StreamWriter(DPAddin.logpath, true))
                    outputFile.WriteLine(result);
        }

        /// <summary>
        /// Выводит во все возможные файлы, лог /+ консоль сообщение+список объектов
        /// </summary>
        /// <param name="outStr"></param>
        /// <param name="objectsToOUT"></param>
        /// <param name="tabname"></param>
        /// <param name="addCallername"></param>
        public static void OutA(string outStr, Object[] objectsToOUT = null, string tabname = "System", bool addCallername = true)
        {

            bool oldwritelog = writelog;
            bool oldwriteout = writeout;

            writelog = true;
            writeout = true;

            Out(outStr, objectsToOUT, tabname, addCallername);

            writelog = oldwritelog;
            writeout = oldwriteout;

        }

        /// <summary>
        /// Преобразует объекты в строку. Для объектов EA визуализирует значения важных свойств
        /// </summary>        
        private static string DumpObjects(object[] objectsToOUT)
        {
            if (objectsToOUT == null || objectsToOUT.Length == 0)
            {
                return "null";
            }

            string result;
            if (objectsToOUT.Length == 1)
                result = ": ";
            else
                result = "(" + objectsToOUT.Length.ToString() + "):";

            if ((objectsToOUT as EA.Element[]) != null)
            {
                foreach (EA.Element curElement in (objectsToOUT as EA.Element[]))
                    result += DumpObject(curElement) + ";";
            }
            else if ((objectsToOUT as EA.DiagramObject[]) != null)
            {
                foreach (EA.DiagramObject curDA in (objectsToOUT as EA.DiagramObject[]))
                    result += DumpObject(curDA) + ";";
            }
            else if ((objectsToOUT as EA.Package[]) != null)
            {
                foreach (EA.Package curPackage in (objectsToOUT as EA.Package[]))
                    result += DumpObject(curPackage) + ";";
            }
            else if ((objectsToOUT as EA.Connector[]) != null)
            {
                foreach (EA.Connector curConnector in (objectsToOUT as EA.Connector[]))
                    result += DumpObject(curConnector) + ";";
            }
            else if ((objectsToOUT[0].GetType() == typeof(Point)))
            {
                foreach (Point point in objectsToOUT)
                    result += DumpObject(point) + ";";
            }
            else if ((objectsToOUT[0].GetType() == typeof(Size)))
            {
                foreach (Size size in objectsToOUT)
                    result += DumpObject(size) + ";";

            }
            else
            {
                foreach (var obj in (objectsToOUT))
                    result += DumpObject(obj) + ";";
            }

            return result;
        }


        /// <summary>
        /// Преобразует объект в строку. Для объектов EA визуализирует значения важных свойств
        /// </summary>        
        public static string DumpObject(object objectToOUT)
        {
            if (objectToOUT == null)
            {
                return "null";
            }

            string result = ":";

            if ((objectToOUT as EA.Element) != null)
            {
                EA.Element curElement = objectToOUT as EA.Element;
                {
                    if (curElement.ClassfierID != 0)
                    {
                        result += "instance(" + curElement.Type;
                        EA.Element classifier = EARepository.GetElementByID(curElement.ClassfierID);
                        result += "," + curElement.Name + "/" + classifier.Name + "," + classifier.Stereotype + ")";
                    }
                    else
                    {
                        result += "classifier(" + curElement.Type;
                        result += "," + curElement.Name + "," + curElement.Stereotype + ")";
                    }
                }
            }
            else if ((objectToOUT as EA.DiagramObject) != null)
            {
                EA.DiagramObject curDA = objectToOUT as EA.DiagramObject;
                {
                    result += "da(" + (curDA.right - curDA.left).ToString() + "x" + Math.Abs(curDA.top - curDA.bottom).ToString() + ")(" + curDA.left.ToString() + "," + curDA.right.ToString() + "," + curDA.top.ToString() + "," + curDA.bottom.ToString() + ")";
                    EA.Element curElement = EARepository.GetElementByID(curDA.ElementID);
                    if (curElement.ClassfierID != 0)
                    {
                        result += "instance(" + curElement.Type;
                        EA.Element classifier = EARepository.GetElementByID(curElement.ClassfierID);
                        result += "," + curElement.Name + "," + classifier.Name + "," + classifier.Stereotype + ")";
                    }
                    else
                    {
                        result += "classifier(" + curElement.Type;
                        result += "," + curElement.Name + "," + curElement.Stereotype + ")";
                    }
                }
            }
            else if ((objectToOUT as EA.Package) != null)
            {
                EA.Package curPackage = objectToOUT as EA.Package;
                result += "package(" + curPackage.Name + ")";
            }
            else if ((objectToOUT as EA.Connector) != null)
            {
                EA.Connector curConnector = objectToOUT as EA.Connector;
                EA.Element elementFrom = EARepository.GetElementByID(curConnector.ClientID);
                string elementFromDump = DumpObject(elementFrom);
                EA.Element elementTo = EARepository.GetElementByID(curConnector.SupplierID);
                string elementToDump = DumpObject(elementTo);

                result += "connector(" + curConnector.Name + "," + curConnector.Type + ") from-to " + elementFromDump + "-" + elementToDump;
            }
            else if (objectToOUT.GetType() == typeof(Point))
            {
                Point point = (Point)objectToOUT;
                result += "(X=" + point.X.ToString() + ", Y=" + point.Y.ToString() + ");";
            }
            else if (objectToOUT.GetType() == typeof(Size))
            {
                Size size = (Size)objectToOUT;
                result += "(Width=" + size.Width.ToString() + ", Height=" + size.Height.ToString() + ");";
            }
            else
            {
                result += objectToOUT.ToString() + ";";
            }

            return result;
        }



    }
}
