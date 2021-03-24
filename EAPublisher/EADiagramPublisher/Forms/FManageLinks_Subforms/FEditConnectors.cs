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
    public partial class FEditConnectors : Form
    {
        public FEditConnectors()
        {
            InitializeComponent();

            LoadConnectorTypes();

            cbFlowID.Items.Clear();
            cbFlowID.Items.AddRange(ConnectorHelper.GetCurrentFlowIDs());
        }

        public static ExecResult<List<ConnectorData>> Execute(List<ConnectorData> connectorDataList)
        {
            var result = new ExecResult<List<ConnectorData>>();
            try
            {
                var form = new FEditConnectors();

                // Список коннекторов
                form.lvConnectors.Items.Clear();
                foreach (var connectorData in connectorDataList)
                {
                    ListViewItem item = new ListViewItem(connectorData.ConnectorID.ToString());
                    item.SubItems.Add(connectorData.Name);
                    form.lvConnectors.Items.Add(item);
                }

                // Имена в комбобокс
                form.cbName.Items.Clear();
                form.cbName.Items.AddRange(connectorDataList.Select(cd => cd.Name).ToArray());

                // linkType и FlowID
                form.cbType.SelectedIndex = form.cbType.SelectedIndex = form.cbType.Items.IndexOf(connectorDataList[0].LinkType);
                form.cbFlowID.Text = connectorDataList[0].FlowID;
                //form.cbSegmentID.Text = connectorDataList[0].SegmentID; Идентификатор сегмента массово не редактируем

                form.lvTaggedValues.Items.Clear();
                foreach (var connectorData in connectorDataList)
                {
                    foreach (EA.ConnectorTag connectorTag in connectorData.Connector.TaggedValues)
                    {
                        ListViewItem item = new ListViewItem(connectorData.ConnectorID.ToString());
                        item.SubItems.Add(connectorTag.Name);
                        item.SubItems.Add(connectorTag.Value);
                        form.lvTaggedValues.Items.Add(item);
                    }
                }

                DialogResult res = form.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    result.value = new List<ConnectorData>();

                    foreach (var connectorData in connectorDataList)
                    {
                        ConnectorData newConnectorData = new ConnectorData();

                        newConnectorData.ConnectorID = connectorData.ConnectorID;
                        if (form.cbName.Text == "")
                            newConnectorData.Name = connectorData.Name;
                        else
                            newConnectorData.Name = form.cbName.Text;
                        newConnectorData.IsLibrary = true;
                        if (form.cbType.SelectedIndex == 0) // не усатновленные поля не записываются, берутся из старых объектов
                            newConnectorData.LinkType = connectorData.LinkType;
                        else
                            newConnectorData.LinkType = (LinkType)Enum.Parse(typeof(LinkType), form.cbType.Text);
                        newConnectorData.SourceElementID = connectorData.SourceElementID;
                        newConnectorData.TargetElementID = connectorData.TargetElementID;
                        if (form.cbFlowID.Text == "")
                            newConnectorData.FlowID = connectorData.FlowID;
                        else
                            newConnectorData.FlowID = form.cbFlowID.Text;
                        if (form.cbSegmentID.Text == "")
                            newConnectorData.SegmentID = connectorData.SegmentID;
                        else
                            newConnectorData.SegmentID = form.cbSegmentID.Text;
                        newConnectorData.Notes = connectorData.Notes;

                        result.value.Add(newConnectorData);
                    }
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
            cbType.Items.Add("");

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

    }
}
