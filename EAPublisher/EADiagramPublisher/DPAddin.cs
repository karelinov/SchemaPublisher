using EADiagramPublisher.Enums;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using EADiagramPublisher.Forms;

namespace EADiagramPublisher
{
    public class DPAddin
    {
        public static String logpath = null;
        public bool startup_done = false;

        // define menu constants
        const string menuDPAddin = "-&DPAddin";

        const string menuExportDiagram = "&ExportDiagram";

        const string menuDesign = "-&Design";
        const string menuPutElement = "-&PutElement";
        const string menuPutLibElementOnDiagram = "&PutLibElementOnDiagram";
        const string menuPutContourContour = "&PutContourContour";
        const string menuPutParentDHierarchyOnDiagram = "&PutParentDHierarchyOnDiagram";
        const string menuPutChildrenDHierarchyOnDiagram = "&PutChildrenDHierarchyOnDiagram";
        //const string menuPutChildrenDHierarchyOnElement = "&PutChildrenDHierarchyOnElement";
        const string menuPutChildrenDeployHierarchy = "&PutChildrenDeployHierarchy";
        //const string menuPutNodes = "&PutNodes";
        const string menuSetElementTags = "&SetElementTags";

        const string menuDesignLinks = "-&DesignLinks";
        const string menuCreateLink = "&CreateLink";
        const string menuSetLinkVisibility = "&SetLinkVisibility";
        const string menuSetConnectorTags = "&SetConnectorTags";
        const string menuSetSimilarLinksTags = "&SetSimilarLinksTags";
        const string menuLinksSelection = "&LinksSelection";

        const string menuUtils = "-&Utils";
        const string menuSetCurrentDiagram = "&SetCurrentDiagram";
        const string menuSetCurrentLibrary = "&SetCurrentLibrary";
        const string menuSetDPLibratyTag = "&SetDPLibratyTag";
        //const string menuSetDefaultSize = "&SetDefaultSize";
        const string menuReloadConnectorData = "&ReloadConnectorData";
        const string menuDoOnConnectActions = "&DoOnConnectActions";
        const string menuRunSQLQuery = "&RunSQLQuery";

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

            Context.Designer = new Designer();
            Context.LinkDesigner = new LinkDesigner();
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
                    subMenus = new string[] { menuExportDiagram, menuDesign, menuDesignLinks, menuUtils, menuTest };
                    return subMenus;

                case menuDesign:
                    subMenus = new string[] { menuPutElement,  menuSetElementTags };
                    return subMenus;

                case menuPutElement:
                    subMenus = new string[] { menuPutContourContour, menuPutParentDHierarchyOnDiagram, menuPutChildrenDeployHierarchy};
                    return subMenus;

                case menuDesignLinks:
                    subMenus = new string[] { menuCreateLink, menuSetLinkVisibility, menuSetConnectorTags, menuSetSimilarLinksTags, menuLinksSelection };
                    return subMenus;

                case menuUtils:
                    subMenus = new string[] { menuSetCurrentDiagram, menuSetCurrentLibrary, menuSetDPLibratyTag, menuReloadConnectorData, menuDoOnConnectActions, menuRunSQLQuery};
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
                    case menuPutLibElementOnDiagram:
                    case menuPutContourContour:
                    case menuPutParentDHierarchyOnDiagram:
                    case menuPutChildrenDeployHierarchy:
                    case menuSetElementTags:
                        isEnabled = true;
                        break;
                    
                    case menuDesignLinks:
                    case menuCreateLink:
                    case menuSetLinkVisibility:
                    case menuSetConnectorTags:
                    case menuSetSimilarLinksTags:
                    case menuLinksSelection:
                        isEnabled = true;
                        break;

                    case menuUtils:
                    case menuSetCurrentDiagram:
                    case menuSetCurrentLibrary:
                    case menuSetDPLibratyTag:
                    case menuReloadConnectorData:
                    case menuDoOnConnectActions:
                    case menuRunSQLQuery:
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
                // ------ DESIGN --------------------------------------------------------------

                case menuPutLibElementOnDiagram:
                    var putLibElementResult = Context.Designer.PutElementOnDiagram();
                    OutExecResult(putLibElementResult);
                    break;

                case menuPutContourContour:
                    var putContourContourResult = Context.Designer.PutContourContourOnDiagram(location);
                    OutExecResult(putContourContourResult);
                    break;

                case menuPutParentDHierarchyOnDiagram:
                    var putDeploymentHierarchyResult = Context.Designer.PutParentHierarchyOnDiagram();
                    OutExecResult(putDeploymentHierarchyResult);
                    break;

                case menuPutChildrenDHierarchyOnDiagram:
                    var putPutCDHResult = Context.Designer.PutChildrenDHierarchyOnDiagram();
                    OutExecResult(putPutCDHResult);
                    break;

                case menuPutChildrenDeployHierarchy:
                    var putChildrenDeployHierarchyResult = Context.Designer.PutChildrenDeployHierarchy(location);
                    OutExecResult(putChildrenDeployHierarchyResult);
                    break;

                case menuSetElementTags:
                    var setElementTagsResult = Context.Designer.SetElementTags(location);
                    OutExecResult(setElementTagsResult);
                    break;



                // ------ DESIGN LINKS --------------------------------------------------------------
                case menuCreateLink:
                    var createCommunicationResult = Context.LinkDesigner.CreateLink();
                    OutExecResult(createCommunicationResult);
                    break;

                case menuSetLinkVisibility:
                    var setLinkVisibilityResult = Context.LinkDesigner.SetConnectorVisibility();
                    OutExecResult(setLinkVisibilityResult);
                    break;

                case menuSetConnectorTags:
                    var setConnectorTagsResult = Context.LinkDesigner.SetConnectorTags(location);
                    OutExecResult(setConnectorTagsResult);
                    break;

                case menuSetSimilarLinksTags:
                    var setSimilarLinksTags = Context.LinkDesigner.SetSimilarLinksTags(location);
                    OutExecResult(setSimilarLinksTags);
                    break;

                case menuLinksSelection:
                    var linksSelectionResult = Context.LinkDesigner.LinksSelection(location);
                    OutExecResult(linksSelectionResult);
                    break;


                // ------ UTILS --------------------------------------------------------------
                case menuSetCurrentDiagram:
                    Context.CurrentDiagram = Context.EARepository.GetCurrentDiagram();
                    Logger.Out("Установлена текущая диаграмма = " + Context.CurrentDiagram.Name);
                    break;

                case menuSetCurrentLibrary:
                    var setCurrentLibraryResult = LibraryHelper.SetCurrentLibrary();
                    OutExecResult(setCurrentLibraryResult);
                    break;

                case menuSetDPLibratyTag:
                    var setDPLibratyTagResult = EATVHelper.SetDPLibratyTag(location);
                    OutExecResult(setDPLibratyTagResult);
                    break;


                case menuReloadConnectorData:
                    Context.ConnectorData = null;
                    Logger.Out("Сброшены данные коннекторов");
                    break;

                case menuDoOnConnectActions:
                    Addin_OnConnectActions();
                    break;

                case menuRunSQLQuery:
                    var fRunSQLQueryResult = FRunSQLQuery.Execute();
                    OutExecResult(fRunSQLQueryResult);
                    break;


                // ------ EXPORT --------------------------------------------------------------
                case menuExportDiagram:
                    var exportResult = DiagramExporter.Export(location);
                    break;


                // ------ TEST --------------------------------------------------------------
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
            Context.Designer = null;
            Context.LinkDesigner = null;
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
                var putDeploymentHierarchyResult = Context.Designer.PutParentHierarchyOnDiagram();
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
                Context.CurrentLibrary = Context.EARepository.GetPackageByGuid("{5C806428-D2F2-4bfc-A043-3B84D3E4CACD}"); ; // SELECT * FROM t_package WHERE ea_guid = "{5C806428-D2F2-4bfc-A043-3B84D3E4CACD}"
                LinkDesignerHelper.LoadConnectorData2();
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
                EA.Diagram diagram = Context.EARepository.GetCurrentDiagram();

                    foreach (EA.DiagramLink curDL in diagram.DiagramLinks)
                    {
                        EA.Connector connector = Context.EARepository.GetConnectorByID(curDL.ConnectorID);
                    foreach(EA.ConnectorTag connectorTag in connector.TaggedValues)
                    {
                        if(connectorTag.Name == "DP_Link")
                        {
                            connectorTag.Name = DAConst.DP_LinkTypeTag;
                            connectorTag.Update();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;

        }


        private void Addin_OnConnectActions()
        {
            if (startup_done) return;

            try
            {
                // Открываем диаграмму из настроек
                string onConnectOpenDiagram = DPConfig.AppSettings["onConnectOpenDiagram"].Value;

                if (onConnectOpenDiagram != "")
                {
                    EA.Diagram diagram = Context.EARepository.GetDiagramByGuid(onConnectOpenDiagram);
                    if (diagram != null)
                    {
                        Context.EARepository.OpenDiagram(diagram.DiagramID);
                        Context.EARepository.ShowInProjectView(diagram);
                    }
                }

                // Устанавливаем библиотеку из настроек
                string onConnectSetLibrary = DPConfig.AppSettings["onConnectSetLibrary"].Value;
                if (onConnectSetLibrary != "")
                {
                    EA.Package libPackage = Context.EARepository.GetPackageByGuid(onConnectSetLibrary);
                    Context.CurrentLibrary = libPackage;
                }

                startup_done = true;
            }
            catch (Exception ex)
            {
                Logger.Out("Addin_Statrtup error: " + ex.StackTrace);
            }

            
        }

    }
}