using EADiagramPublisher.Contracts;
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
            foreach(ListViewItem item in lvConnectors.SelectedItems)
            {
                ConnectorData connectorData =  (ConnectorData)item.Tag;

                EA.DiagramLink diagramLink = DiagramLinkHelper.GetDLFromConnector(connectorData._ConnectorID);
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

                EA.DiagramLink diagramLink = DiagramLinkHelper.GetDLFromConnector(connectorData._ConnectorID);
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
            // заливаем список коннекторов на форму
            lvConnectors.Items.Clear();
            foreach (EA.DiagramLink diagramLink in Context.CurrentDiagram.DiagramLinks)
            {

                EA.Connector connector = Context.EARepository.GetConnectorByID(diagramLink.ConnectorID);

                ConnectorData connectorData = null;
                if (Context.ConnectorData.Keys.Contains(diagramLink.ConnectorID))
                    connectorData = Context.ConnectorData[diagramLink.ConnectorID];
                else
                {
                    connectorData = new ConnectorData(connector);
                }

                if (!AllowByFilter(connectorData)) continue; // Проверяем, что показ этого коннектора в списке разрешён фильтрами

                EA.Element sourceElement = Context.EARepository.GetElementByID(connector.ClientID);
                EA.Element targetElement = Context.EARepository.GetElementByID(connector.SupplierID);

                // Создаём ListViewItem
                ListViewItem item = new ListViewItem();
                item.Tag = connectorData;
                item.Text = (!diagramLink.IsHidden).ToString();

                item.SubItems.Add(ElementDesignerHelper.ElementDisplayName(sourceElement));
                item.SubItems.Add(connectorData.NameForShow());
                item.SubItems.Add(connectorData.LinkType.ToString());
                item.SubItems.Add(connectorData.FlowID);
                item.SubItems.Add(connectorData.SegmentID);
                item.SubItems.Add("");
                item.SubItems.Add(ElementDesignerHelper.ElementDisplayName(targetElement));

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

            if(lblSourceElementFilter.Tag != null)
                if (! ( 
                    ((int[])lblSourceElementFilter.Tag).Contains(connectorData.SourceElementID) 
                    || 
                    ((int[])lblSourceElementFilter.Tag).Contains(connectorData.TargetElementID)
                    )
                   )
                {
                    return false;
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
                lblFlowIDFilter.Text = String.Join(",",selectFlowIDResult.value);

                LoadConnectorList();
            }
            else
            {
                if (selectFlowIDResult.code == (int)DialogResult.Cancel)
                    throw new Exception(selectFlowIDResult.message);
            }

        }

        private void btnSourceElementFilter_Click(object sender, EventArgs e)
        {
            ExecResult<int[]> selectDiagramObjectsResult = FSelectDiagramObjects.Execute((int[])lblSourceElementFilter.Tag);
            if (selectDiagramObjectsResult.code == 0)
            {
                lblSourceElementFilter.Tag = selectDiagramObjectsResult.value;
                lblFlowIDFilter.Text = "";
                for (int i=0; i< selectDiagramObjectsResult.value.Length; i++)
                {
                    int elementID = selectDiagramObjectsResult.value[i];

                    if (lblFlowIDFilter.Text == "")
                        lblFlowIDFilter.Text += elementID.ToString();
                    else
                        lblFlowIDFilter.Text += "," + elementID.ToString();

                    if (i == 5) break;
                }
                if (lblFlowIDFilter.Text != "")
                    lblFlowIDFilter.Text += "...";

                LoadConnectorList();
            }
            else
            {
                if (selectDiagramObjectsResult.code != (int)DialogResult.Cancel)
                    throw new Exception(selectDiagramObjectsResult.message);
            }
        }
    }
}
