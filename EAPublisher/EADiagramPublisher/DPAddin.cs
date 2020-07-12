using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace EADiagramPublisher
{
    public class DPAddin
    {
        public static Designer Designer { get; set; }
        public static String logpath = null;

        // define menu constants
        const string menuDPAddin = "-&DPAddin";

        const string menuExportDiagram = "&ExportDiagram";

        const string menuDesign = "-&Design";
        //const string menuSetCurrentLibrary = "&SetCurrentLibrary";
        //const string menuSetCurrentDiagram = "&SetCurrentDiagram";
        const string menuPutLibElementOnDiagram = "&PutLibElementOnDiagram";
        const string menuPutDeploymentHierarchyOnDiagram = "&PutParentHierarchyOnDiagram";
        const string menuUtils = "-&Utils";
        const string menuSetDefaultSize = "&SetDefaultSize";

        const string menuTest = "-&Test";
        const string menuTest1 = "&Test1";
        const string menuTest2 = "&Test2";
        const string menuTest3 = "&Test3";

        ///
        /// Called Before EA starts to check Add-In Exists
        /// Nothing is done here.
        /// This operation needs to exists for the addin to work
        ///
        /// <param name="Repository" />the EA repository
        /// a string
        public String EA_Connect(EA.Repository repository)
        {
            Context.EARepository = repository;

            Designer = new Designer();
            logpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "EADiagramPublisher.log");


            return "";
        }

        ///
        /// Called when user Clicks Add-Ins Menu item from within EA.
        /// Populates the Menu with our desired selections.
        /// Location can be "TreeView" "MainMenu" or "Diagram".
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        ///
        public object EA_GetMenuItems(EA.Repository repository, string location, string menuName)
        {
            string[] subMenus = null;

            switch (menuName)
            {
                // defines the top level menu option
                case "":
                    return menuDPAddin;
                case menuDPAddin:
                    subMenus = new string[] { menuExportDiagram, menuDesign, menuUtils, menuTest };
                    return subMenus;
                case menuDesign:
                    subMenus = new string[] { /*menuSetCurrentLibrary, menuSetCurrentDiagram,*/ menuPutLibElementOnDiagram, menuPutDeploymentHierarchyOnDiagram };
                    return subMenus;
                case menuUtils:
                    subMenus = new string[] { menuSetDefaultSize };
                    return subMenus;
                case menuTest:
                    subMenus = new string[] { menuTest1 , menuTest2, menuTest3 };
                    return subMenus;
            }

            return "";
        }

        ///
        /// returns true if a project is currently opened
        ///
        /// <param name="Repository" />the repository
        /// true if a project is opened in EA
        bool IsProjectOpen(EA.Repository repository)
        {
            try
            {
                EA.Collection c = repository.Models;
                return true;
            }
            catch
            {
                return false;
            }
        }

        ///
        /// Called once Menu has been opened to see what menu items should active.
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the menu item
        /// <param name="IsEnabled" />boolean indicating whethe the menu item is enabled
        /// <param name="IsChecked" />boolean indicating whether the menu is checked
        public void EA_GetMenuState(EA.Repository repository, string location, string menuName, string ItemName, ref bool isEnabled, ref bool isChecked)
        {
            if (IsProjectOpen(repository))
            {
                switch (ItemName)
                {
                    case menuExportDiagram:
                        isEnabled = true;
                        break;
                    case menuDesign:
                    /*
                    case menuSetCurrentLibrary:
                    case menuSetCurrentDiagram:
                    */
                    case menuPutLibElementOnDiagram:
                    case menuPutDeploymentHierarchyOnDiagram:
                        isEnabled = true;
                        break;

                    case menuUtils:
                    case menuSetDefaultSize:
                        isEnabled = true;
                        break;

                    case menuTest:
                    case menuTest1:
                    case menuTest2:
                    case menuTest3:
                        isEnabled = true;
                        break;

                    // there shouldn't be any other, but just in case disable it.
                    default:
                        isEnabled = false;
                        break;
                }
            }
            else
            {
                // If no open project, disable all menu options
                isEnabled = false;
            }

        }

        ///
        /// Called when user makes a selection in the menu.
        /// This is your main exit point to the rest of your Add-in
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the selected menu item
        public void EA_MenuClick(EA.Repository repository, string location, string menuName, string itemName)
        {
            switch (itemName)
            {
                case menuExportDiagram:
                    var exportResult = DiagramExporter.Export(location);
                    break;
                /*
                case menuSetCurrentLibrary:
                    var setCurrentLibraryResult = Designer.SetCurrentLibrary();
                    OutExecResult(setCurrentLibraryResult);
                    break;
                case menuSetCurrentDiagram:
                    Designer.CurrentDiagram = EARepository.GetCurrentDiagram();
                    EAHelper.OutA("Установлена текущая диаграмма = " + Designer.CurrentDiagram.Name);
                    break;
                */
                case menuPutLibElementOnDiagram:
                    var putLibElementResult = Designer.PutElementOnDiagram();
                    OutExecResult(putLibElementResult);
                    break;
                case menuPutDeploymentHierarchyOnDiagram:
                    var putDeploymentHierarchyResult = Designer.PutParentHierarchyOnDiagram();
                    OutExecResult(putDeploymentHierarchyResult);
                    break;

                case menuSetDefaultSize:
                    var SetElementDefaultSizeResult = EAHelper.SetElementDefaultSize();
                    OutExecResult(SetElementDefaultSizeResult);
                    break;
                case menuTest1:
                    var test1Result = Test1();
                    OutExecResult(test1Result);
                    break;
                case menuTest2:
                    var test2Result = Test2();
                    OutExecResult(test2Result);
                    break;
                case menuTest3:
                    var test3Result = Test3();
                    OutExecResult(test3Result);
                    break;



            }
        }

        ///
        /// EA calls this operation when it exists. Can be used to do some cleanup work.
        ///
        public void EA_Disconnect()
        {
            Designer = null;
            Context.EARepository = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }



        private void OutExecResult<T>(ExecResult<T> result)
        {
            Context.EARepository.WriteOutput("System", "result code=" + result.code + " " + result.message, 0);

        }


        private static int test1Flag = 0;
        private ExecResult<Boolean> Test1()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                // Открываем и чистим тестовую диаграмму 
                EA.Diagram testDiagram = Context.EARepository.GetDiagramByGuid("{0093407F-0187-42a8-93DC-B97E8FA79EED}");
                if (test1Flag == 0)
                {

                    while (testDiagram.DiagramObjects.Count > 0)
                    {
                        testDiagram.DiagramObjects.DeleteAt(0, true);
                    }
                    testDiagram.Update();
                    testDiagram.DiagramObjects.Refresh();
                    Context.EARepository.ReloadDiagram(testDiagram.DiagramID);
                }

                Context.EARepository.OpenDiagram(testDiagram.DiagramID);
                Context.EARepository.ActivateDiagram(testDiagram.DiagramID);
                Context.CurrentDiagram = testDiagram;

                // Выделяем элемент
                EA.Element element = Context.EARepository.GetElementByGuid("{83142BDB-7EE4-48e7-B788-0011E0E2E343}");
                Context.EARepository.ShowInProjectView(element);

                // Запускаем формирование иерархии элементов диаграммы
                var putDeploymentHierarchyResult = Designer.PutParentHierarchyOnDiagram();
                OutExecResult(putDeploymentHierarchyResult);


                test1Flag = 1;
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;

        }

        private ExecResult<Boolean> Test2()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                // Открываем и чистим тестовую диаграмму 
                EA.Diagram testDiagram = Context.EARepository.GetDiagramByGuid("{0093407F-0187-42a8-93DC-B97E8FA79EED}");
                if (test1Flag == 0)
                {

                    while (testDiagram.DiagramObjects.Count > 0)
                    {
                        testDiagram.DiagramObjects.DeleteAt(0, true);
                    }
                    testDiagram.Update();
                    testDiagram.DiagramObjects.Refresh();
                    Context.EARepository.ReloadDiagram(testDiagram.DiagramID);
                }

                Context.EARepository.OpenDiagram(testDiagram.DiagramID);
                Context.EARepository.ActivateDiagram(testDiagram.DiagramID);
                Context.CurrentDiagram = testDiagram;

                // Выделяем элемент
                EA.Element element = Context.EARepository.GetElementByGuid("{83142BDB-7EE4-48e7-B788-0011E0E2E343}");
                Context.EARepository.ShowInProjectView(element);
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;

        }

        private ExecResult<Boolean> Test3()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();
            try
            {
                // Открываем и чистим тестовую диаграмму 
                EA.Diagram testDiagram = Context.EARepository.GetDiagramByGuid("{0093407F-0187-42a8-93DC-B97E8FA79EED}");
                if (test1Flag == 0)
                {

                    while (testDiagram.DiagramObjects.Count > 0)
                    {
                        testDiagram.DiagramObjects.DeleteAt(0, true);
                    }
                    testDiagram.Update();
                    testDiagram.DiagramObjects.Refresh();
                    Context.EARepository.ReloadDiagram(testDiagram.DiagramID);
                }

                Context.EARepository.OpenDiagram(testDiagram.DiagramID);
                Context.EARepository.ActivateDiagram(testDiagram.DiagramID);
                Context.CurrentDiagram = testDiagram;

                // Выделяем элемент Сервер1
                EA.Element element = Context.EARepository.GetElementByGuid("{A4A9C875-BF19-4fde-848D-4329FF477A02}");
                Context.EARepository.ShowInProjectView(element);

                EA.DiagramObject elementDA = testDiagram.DiagramObjects.AddNew("", "");
                elementDA.ElementID = element.ElementID;
                elementDA.Update();
                testDiagram.Update();
                testDiagram.DiagramObjects.Refresh();
                int elementID = elementDA.ElementID;
                EAHelper.OutA("создан элемент ", new EA.DiagramObject[] { elementDA });

                testDiagram.DiagramObjects.Refresh();
                EAHelper.OutA("after  DiagramObjects.Refresh ", new EA.DiagramObject[] { elementDA });


                testDiagram.DiagramObjects.Refresh();
                Context.EARepository.ReloadDiagram(testDiagram.DiagramID);
                testDiagram.DiagramObjects.Refresh();
                EAHelper.OutA("after RRR", new EA.DiagramObject[] { elementDA });

                elementDA.bottom = elementDA.bottom - 1;
                elementDA.Update();
                testDiagram.DiagramObjects.Refresh();
                Context.EARepository.ReloadDiagram(testDiagram.DiagramID);
                testDiagram.DiagramObjects.Refresh();
                EAHelper.OutA("after MRRR", new EA.DiagramObject[] { elementDA });



            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;

        }


    }
}