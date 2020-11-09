using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EADiagramPublisher.Forms
{
    public partial class FManageLinks : Form
    {
        private ListViewComparer listViewComparer;


        public FManageLinks()
        {
            InitializeComponent();

            listViewComparer = new ListViewComparer();
            this.lvConnectors.ListViewItemSorter = listViewComparer;
        }

        public static ExecResult<bool> Execute(bool ShowModal = false)
        {
            var result = new ExecResult<bool>();
            try
            {
                var form = new FManageLinks();
                form.LoadConnectorList();
                form.ShowDialog();


            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        private void LoadConnectorList()
        {
            // заливаем список коннекторов на форму
            lvConnectors.Items.Clear();
            foreach (ConnectorData connectorData in Context.ConnectorData.Values)
            {
                if (!AllowByFilter(connectorData)) continue; // Проверяем, что показ этого коннектора в списке разрешён фильтрами

                // Создаём ListViewItem
                ListViewItem item = new ListViewItem();
                item.Tag = connectorData;
                item.Text = connectorData.ConnectorID.ToString();
                if (Context.ElementData.ContainsKey(connectorData.SourceElementID))
                {
                    item.SubItems.Add(Context.ElementData[connectorData.SourceElementID].DisplayName);
                }
                else
                {
                    item.SubItems.Add(connectorData.SourceElementID.ToString() + ",NOT LIB");
                }
                item.SubItems.Add(connectorData.NameForShow());
                item.SubItems.Add(connectorData.IsLibrary ? connectorData.LinkType.ToString() : "");
                item.SubItems.Add(connectorData.IsLibrary ? connectorData.FlowID : "");
                item.SubItems.Add(connectorData.IsLibrary ? connectorData.SegmentID : "");
                item.SubItems.Add(connectorData.Notes);
                if (Context.ElementData.ContainsKey(connectorData.TargetElementID))
                {
                    item.SubItems.Add(Context.ElementData[connectorData.TargetElementID].DisplayName);
                }
                else
                {
                    item.SubItems.Add(connectorData.TargetElementID.ToString()+ ",NOT LIB");
                }

                lvConnectors.Items.Add(item);

            }
            RefreshConnectorCountLabel();
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

        private void btnLinkTypeFilter_Click(object sender, EventArgs e)
        {
            ExecResult<LinkType[]> selectLinkTypeResult = FSelectLinkType.Execute((LinkType[])lblLinkTypeFilter.Tag);
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


        private void RefreshConnectorCountLabel()
        {
            tsslConnectorCount.Text = "Отобрано/всего коннекторов " + lvConnectors.Items.Count.ToString() + "/" + Context.ConnectorData.Count.ToString();
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

        private void tsbEditProperties_Click(object sender, EventArgs e)
        {
            if (lvConnectors.SelectedItems.Count > 0)
            {
                var editResult = FEditConnector.Execute((ConnectorData)lvConnectors.SelectedItems[0].Tag);
                if(editResult.code == 0)
                {
                    ConnectorHelper.UpdateConnectorByData(editResult.value);
                }


                LoadConnectorList();
            }
        }

        private void lvConnectors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsbEditProperties_Click(tsbEditProperties, null);
        }

        private void tsbEditSelected_Click(object sender, EventArgs e)
        {
            if(lvConnectors.SelectedItems.Count > 0)
            {
                List<ConnectorData> connectorDatas = new List<ConnectorData>(); 

                foreach(ListViewItem item in lvConnectors.SelectedItems)
                {
                    connectorDatas.Add((ConnectorData)item.Tag);
                }


                var editResult = FEditConnectors.Execute(connectorDatas);
                if (editResult.code == 0)
                {
                    foreach(var connectorData in editResult.value)
                    {
                        ConnectorHelper.UpdateConnectorByData(connectorData);
                    }
                }

                LoadConnectorList();
            }
        }
    }
}
