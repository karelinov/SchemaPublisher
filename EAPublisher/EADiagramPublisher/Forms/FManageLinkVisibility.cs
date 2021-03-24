using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EADiagramPublisher.Forms
{
    public partial class FManageLinkVisibility : Form
    {
        private ListViewComparer listViewComparer;

        public FManageLinkVisibility()
        {
            InitializeComponent();

            listViewComparer = new ListViewComparer();
            this.lvConnectors.ListViewItemSorter = listViewComparer;

            LoadConnectorList();
        }

        public static ExecResult<bool> Execute(bool ShowModal = false)
        {
            var result = new ExecResult<bool>();
            try
            {
                var form = new FManageLinkVisibility();
                if (ShowModal)
                {
                    DialogResult res = form.ShowDialog();
                }
                else
                {
                    form.Show();
                    form.BringToFront();
                }


            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        private void lvConnectors_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == listViewComparer.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (listViewComparer.Order == SortOrder.Ascending)
                {
                    listViewComparer.Order = SortOrder.Descending;
                }
                else
                {
                    listViewComparer.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                listViewComparer.SortColumn = e.Column;
                listViewComparer.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvConnectors.Sort();
        }

        private void tsbShow_Click(object sender, EventArgs e)
        {
            // Сначала запрашиваем стиль и пообще не передумали ли мы
            ExecResult<SetLinkStyle> setLinkStyleResult = new FLinkStyle().Execute();
            if (setLinkStyleResult.code != 0)
                return;

            // Проходимся по списку выделенных коннекторов
            foreach (ListViewItem item in lvConnectors.SelectedItems)
            {
                ConnectorData connectorData = (ConnectorData)item.Tag;

                EA.DiagramLink diagramLink = DiagramLinkHelper.GetDLFromConnector(connectorData.ConnectorID);
                if (diagramLink == null)
                {
                    diagramLink = DiagramLinkHelper.CreateDiagramLink(connectorData.Connector);
                }

                DiagramLinkHelper.SetDiagramLinkVisibility(diagramLink, true); // устанавливаем видимость
                item.Text = (!diagramLink.IsHidden).ToString();

                if (setLinkStyleResult.value.DoSetLinkStyle) // устанавливаем стиль линий
                {
                    DiagramLinkHelper.ApplyStyleToDL(diagramLink, setLinkStyleResult.value);
                }


            }
        }

        private void tsbHide_Click(object sender, EventArgs e)
        {
            // Проходимся по списку выделенных коннекторов
            foreach (ListViewItem item in lvConnectors.SelectedItems)
            {
                ConnectorData connectorData = (ConnectorData)item.Tag;

                EA.DiagramLink diagramLink = DiagramLinkHelper.GetDLFromConnector(connectorData.ConnectorID);
                if (diagramLink == null)
                {
                    diagramLink = DiagramLinkHelper.CreateDiagramLink(connectorData.Connector);
                }

                DiagramLinkHelper.SetDiagramLinkVisibility(diagramLink, false); // устанавливаем видимость
                item.Text = (!diagramLink.IsHidden).ToString();
            }
        }

        private void tsbReloadCurrentDiagram_Click(object sender, EventArgs e)
        {
            Context.EARepository.ReloadDiagram(Context.CurrentDiagram.DiagramID);
        }


        private void LoadConnectorList()
        {
            // получаем список описателей элементов диаграммы
            Dictionary<int, ElementData> elementDataList = EAHelper.GetCurDiagramElementData();

            // получаем список коннекторов диаграммы
            Dictionary<int, bool> curDiagramConnectorsID = EAHelper.GetCurDiagramConnectorsID();


            // заливаем список коннекторов на форму
            lvConnectors.Items.Clear();
            foreach (int curConnectorID in curDiagramConnectorsID.Keys)
            {
                ConnectorData connectorData;
                if (Context.ConnectorData.ContainsKey(curConnectorID))
                {
                    connectorData = Context.ConnectorData[curConnectorID];
                }
                else // небиблиотечный коннектор, его нет в списке 
                {
                    EA.Connector connector = Context.EARepository.GetConnectorByID(curConnectorID);
                    connectorData = new ConnectorData(connector);
                }

                if (!AllowByFilter(connectorData)) continue; // Проверяем, что показ этого коннектора в списке разрешён фильтрами


                // Создаём ListViewItem
                ListViewItem item = new ListViewItem();
                item.Tag = connectorData;
                item.Text = (!curDiagramConnectorsID[curConnectorID]).ToString();

                item.SubItems.Add(elementDataList[connectorData.SourceElementID].DisplayName);
                item.SubItems.Add(connectorData.NameForShow());
                item.SubItems.Add(connectorData.IsLibrary ? connectorData.LinkType.ToString() : "");
                item.SubItems.Add(connectorData.IsLibrary ? connectorData.FlowID : "");
                item.SubItems.Add(connectorData.IsLibrary ? connectorData.SegmentID : "");
                item.SubItems.Add(connectorData.Notes);
                item.SubItems.Add(elementDataList[connectorData.TargetElementID].DisplayName);

                lvConnectors.Items.Add(item);

            }

        }

        /// <summary>
        /// Функция проверяет, разрешён ли показ коннекторат текущим фильтром
        /// </summary>
        private bool AllowByFilter(ConnectorData connectorData)
        {
            bool result = true;

            if (lblFlowIDFilter.Tag != null && ((string[])lblFlowIDFilter.Tag).Length > 0)
            {
                if (!((string[])lblFlowIDFilter.Tag).Contains(connectorData.FlowID))
                    return false;
            }

            if (lblSourceElementFilter.Tag != null && ((int[])lblSourceElementFilter.Tag).Length != 0)
                if (!(
                    ((int[])lblSourceElementFilter.Tag).Contains(connectorData.SourceElementID)
                    ||
                    ((int[])lblSourceElementFilter.Tag).Contains(connectorData.TargetElementID)
                    )
                   )
                {
                    return false;
                }

            if (lblLinkTypeFilter.Tag != null && ((LinkType[])lblLinkTypeFilter.Tag).Length != 0)
            {
                if (!((LinkType[])lblLinkTypeFilter.Tag).Contains(connectorData.LinkType))
                {
                    return false;
                }
            }

            if (lblSoftwareClassificationFilter1.Tag != null && ((int[])lblSoftwareClassificationFilter1.Tag).Length != 0)
            {
                bool belongsToSoftware = false;
                foreach (int softwareID in (int[])lblSoftwareClassificationFilter1.Tag)
                {
                    ElementData softwareElementData = Context.SoftwareClassification.AllNodes[softwareID].Value;

                    ElementData sourceElementData = Context.ElementData[connectorData.SourceElementID];
                    if (SoftwareClassificationHelper.ISBelongsToSoftware(sourceElementData, softwareElementData))
                    {
                        belongsToSoftware = true;
                        break;
                    }
                    ElementData targetElementData = Context.ElementData[connectorData.TargetElementID];
                    if (SoftwareClassificationHelper.ISBelongsToSoftware(targetElementData, softwareElementData))
                    {
                        belongsToSoftware = true;
                        break;
                    }

                }

                if (!belongsToSoftware)
                {
                    return false;
                }
            }



            return result;
        }

        /// <summary>
        /// При щелчке открываем форму выбора FlowID, присваиваем и показывает результат в lblFlowIDFilter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectFlowID_Click(object sender, EventArgs e)
        {
            ExecResult<string[]> selectFlowIDResult = FSelectFlowID.Execute();
            if (selectFlowIDResult.code == 0)
            {
                lblFlowIDFilter.Tag = selectFlowIDResult.value;
                lblFlowIDFilter.Text = String.Join(",", selectFlowIDResult.value);

                SetFilterLabel(tpFlowIDFilter, selectFlowIDResult.value.Length > 0);
                LoadConnectorList();
            }
        }

        private void btnSourceElementFilter_Click(object sender, EventArgs e)
        {
            ExecResult<int[]> selectDiagramObjectsResult = FSelectDiagramObjects.Execute((int[])lblSourceElementFilter.Tag);
            if (selectDiagramObjectsResult.code == 0)
            {
                lblSourceElementFilter.Tag = selectDiagramObjectsResult.value;
                lblSourceElementFilter.Text = "";
                for (int i = 0; i < selectDiagramObjectsResult.value.Length; i++)
                {
                    int elementID = selectDiagramObjectsResult.value[i];

                    if (lblSourceElementFilter.Text == "")
                        lblSourceElementFilter.Text += elementID.ToString();
                    else
                        lblSourceElementFilter.Text += "," + elementID.ToString();

                    if (i == 5) break;
                }
                if (lblSourceElementFilter.Text != "")
                    lblSourceElementFilter.Text += "...";

                SetFilterLabel(tpSourceElementFilter, selectDiagramObjectsResult.value.Length > 0);
                LoadConnectorList();
            }
        }

        private void btnLinkTypeFilter_Click(object sender, EventArgs e)
        {
            ExecResult<LinkType[]> selectLinkTypeResult = FSelectComponentLevel.Execute((LinkType[])lblLinkTypeFilter.Tag);
            if (selectLinkTypeResult.code == 0)
            {
                lblLinkTypeFilter.Tag = selectLinkTypeResult.value;
                lblLinkTypeFilter.Text = "";
                for (int i = 0; i < selectLinkTypeResult.value.Length; i++)
                {
                    LinkType linkType = selectLinkTypeResult.value[i];

                    if (lblLinkTypeFilter.Text == "")
                        lblLinkTypeFilter.Text += linkType.ToString();
                    else
                        lblLinkTypeFilter.Text += "," + linkType.ToString();

                    if (i == 5) break;
                }
                if (lblLinkTypeFilter.Text != "")
                    lblLinkTypeFilter.Text += "...";


                SetFilterLabel(tpLinkTypeFilter, selectLinkTypeResult.value.Length > 0);
                LoadConnectorList();
            }

        }

        private void btnSelectConnectorObjects_Click(object sender, EventArgs e)
        {
            while (Context.CurrentDiagram.SelectedObjects.Count > 0)
                Context.CurrentDiagram.SelectedObjects.Delete(0);

            foreach (ListViewItem item in lvConnectors.SelectedItems)
            {
                ConnectorData connectorData = (ConnectorData)item.Tag;
                //EA.DiagramObject sourceDiagramObject = Context.CurrentDiagram.GetDiagramObjectByID(connectorData.SourceElementID);
                Context.CurrentDiagram.SelectedObjects.AddNew(connectorData.SourceElementID.ToString(), "");
                Context.CurrentDiagram.SelectedObjects.AddNew(connectorData.TargetElementID.ToString(), "");
            }
        }

        private void tsbShowInProject_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvConnectors.SelectedItems)
            {
                ConnectorData connectorData = (ConnectorData)item.Tag;
                EA.Element sourceElement = Context.EARepository.GetElementByID(connectorData.SourceElementID);

                Context.EARepository.ShowInProjectView(sourceElement);

                break;
            }

        }

        private void tsbShowEndInProject_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvConnectors.SelectedItems)
            {
                ConnectorData connectorData = (ConnectorData)item.Tag;
                EA.Element targetElement = Context.EARepository.GetElementByID(connectorData.TargetElementID);

                Context.EARepository.ShowInProjectView(targetElement);

                break;
            }

        }

        private void tsbSelectConnector_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvConnectors.SelectedItems)
            {
                ConnectorData connectorData = (ConnectorData)item.Tag;
                Context.CurrentDiagram.SelectedConnector = Context.EARepository.GetConnectorByID(connectorData.ConnectorID);
                break;
            }
        }


        /// <summary>
        /// Добавляет/убирает к названию страницы текст с указанием, что фильтр установлен
        /// </summary>
        public void SetFilterLabel(TabPage tabPage, bool filterSet)
        {
            string filterText = "(Filtered)";

            if (filterSet)
            {
                if (!tabPage.Text.Contains(filterText))
                    tabPage.Text += filterText;
            }
            else
            {
                tabPage.Text = tabPage.Text.Replace(filterText, "");
            }

        }

        private void btnSoftwareClassificationFilter_Click(object sender, EventArgs e)
        {
            ExecResult<int[]> selectSoftwareClassificationResult = FSelectSoftwareClassification.Execute((int[])lblSoftwareClassificationFilter1.Tag);
            if (selectSoftwareClassificationResult.code == 0)
            {
                lblSoftwareClassificationFilter1.Tag = selectSoftwareClassificationResult.value;
                lblSoftwareClassificationFilter1.Text = "";
                for (int i = 0; i < selectSoftwareClassificationResult.value.Length; i++)
                {
                    int elementDataID = selectSoftwareClassificationResult.value[i];

                    if (lblSoftwareClassificationFilter1.Text == "")
                        lblSoftwareClassificationFilter1.Text += elementDataID.ToString();
                    else
                        lblSoftwareClassificationFilter1.Text += "," + elementDataID.ToString();

                    if (i == 5) break;
                }
                if (lblSoftwareClassificationFilter1.Text != "")
                    lblSoftwareClassificationFilter1.Text += "...";


                SetFilterLabel(tpSoftwareClassificationFilter, selectSoftwareClassificationResult.value.Length > 0);
                LoadConnectorList();
            }
        }

        private void lvConnectors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvConnectors.SelectedItems.Count > 0)
            {
                FConnectorProperties.Execute(lvConnectors.SelectedItems[0].Tag as ConnectorData);
            }
        }

        /// <summary>
        /// Выделение в списке линков, связанных с выделенными на диаграмме элементами
        /// Производится заполеннием фильтра по компонентам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSelectedComponentsLinks_Click(object sender, EventArgs e)
        {
            // Получаем список выделенных на диаграмме элемнетов
            List<EA.Element> selectedElements = EAHelper.GetSelectedLibElement_Diagram();

            List<int> filteredElements = new List<int> { };
            if (lblSourceElementFilter.Tag != null)
            {
                filteredElements.AddRange((int[])lblSourceElementFilter.Tag);
            }
            filteredElements.AddRange(selectedElements.Select((se, index) => se.ElementID));

            if (filteredElements.Count > 0)
            {
                lblSourceElementFilter.Tag = filteredElements.ToArray();
            }

            LoadConnectorList();

        }

        private void tsbHideAll_Click(object sender, EventArgs e)
        {
            // Проходимся по списку выделенных коннекторов
            foreach (EA.DiagramLink diagramLink in Context.EARepository.GetCurrentDiagram().DiagramLinks) 
            { 
                DiagramLinkHelper.SetDiagramLinkVisibility(diagramLink, false); // устанавливаем видимость
                //item.Text = (!diagramLink.IsHidden).ToString();
            }
        }
    }

}

