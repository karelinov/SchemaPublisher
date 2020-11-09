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
    public partial class FEditConnector : Form
    {
        public FEditConnector()
        {
            InitializeComponent();

            LoadConnectorTypes();

            cbFlowID.Items.Clear();
            cbFlowID.Items.AddRange(ConnectorHelper.GetCurrentFlowIDs());
        }

        public static ExecResult<ConnectorData> Execute(ConnectorData connectorData)
        {
            var result = new ExecResult<ConnectorData>();
            try
            {
                var form = new FEditConnector();

                form.tbID.Text = connectorData.ConnectorID.ToString();
                form.tbGUID.Text = connectorData.Connector.ConnectorGUID;
                form.tbName.Text = connectorData.Name;
                form.cbType.SelectedIndex = form.cbType.SelectedIndex = form.cbType.Items.IndexOf(connectorData.LinkType);
                form.tbClientID.Text = connectorData.SourceElementID.ToString()+" ("+ Context.ElementData[connectorData.SourceElementID].DisplayName +")";
                form.tbClientID.Tag = connectorData.SourceElementID;
                form.tbSupplierID.Text = connectorData.TargetElementID.ToString() + " (" + Context.ElementData[connectorData.TargetElementID].DisplayName + ")";
                form.tbSupplierID.Tag = connectorData.TargetElementID;
                form.cbFlowID.Text = connectorData.FlowID;
                form.cbSegmentID.Text = connectorData.SegmentID;
                form.tbNotes.Text = connectorData.Notes;

                form.lvTaggedValues.Items.Clear();
                foreach (EA.ConnectorTag connectorTag in connectorData.Connector.TaggedValues)
                {
                    ListViewItem item = new ListViewItem(connectorTag.Name);
                    item.SubItems.Add(connectorTag.Value);
                    form.lvTaggedValues.Items.Add(item);
                }


                DialogResult res = form.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    result.value = new ConnectorData();

                    result.value.ConnectorID = int.Parse(form.tbID.Text);
                    result.value.Name = form.tbName.Text;
                    result.value.IsLibrary = true;
                    result.value.LinkType = (LinkType)Enum.Parse(typeof(LinkType), form.cbType.Text);
                    result.value.SourceElementID = (int)form.tbClientID.Tag;
                    result.value.TargetElementID = (int)form.tbSupplierID.Tag;
                    result.value.FlowID = form.cbFlowID.Text;
                    result.value.SegmentID = form.cbSegmentID.Text;
                    result.value.Notes = form.tbNotes.Text;
                }
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }


        private void LoadConnectorTypes()
        {
            cbType.Items.Clear();

            foreach (LinkType linkType in Enum.GetValues(typeof(LinkType)))
            {
                cbType.Items.Add(linkType);
            }
        }

        private void cbFlowID_SelectedIndexChanged(object sender, EventArgs e)
        {
            // При изменении FlowID перезагружаем комбо с сегментами

            string flowID = cbFlowID.Text;

            cbSegmentID.Items.Clear();

            if (flowID != "")
                cbSegmentID.Items.AddRange(ConnectorHelper.GetSegmentsForFlowID(flowID));
        }

        private void btnSelectClient_Click(object sender, EventArgs e)
        {
            var selectObjectResult = FSelectObject.Execute();
            if(selectObjectResult.code == 0)
            {
                tbClientID.Text = selectObjectResult.value.ID.ToString() + " (" + Context.ElementData[selectObjectResult.value.ID].DisplayName + ")";
                tbClientID.Tag = selectObjectResult.value.ID;
            }
        }

        private void btnSelectSupplier_Click(object sender, EventArgs e)
        {
            var selectObjectResult = FSelectObject.Execute();
            if (selectObjectResult.code == 0)
            {
                tbSupplierID.Text = selectObjectResult.value.ID.ToString() + " (" + Context.ElementData[selectObjectResult.value.ID].DisplayName + ")";
                tbSupplierID.Tag = selectObjectResult.value.ID;
            }
        }

    }
}
