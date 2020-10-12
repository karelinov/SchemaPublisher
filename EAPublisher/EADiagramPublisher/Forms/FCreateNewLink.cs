using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;

namespace EADiagramPublisher.Forms
{
    public partial class FCreateNewLink : Form
    {
        private int? checkedContext = null;

        public FCreateNewLink()
        {
            InitializeComponent();


            this.clbLinkType.Items.Clear();
            foreach (LinkType linkType in (LinkType[])Enum.GetValues(typeof(LinkType)))
            {
                this.clbLinkType.Items.Add(linkType, false);
            }

            //tbTempLinkDiagramID.Text = Context.CurrentDiagram.DiagramGUID.ToString();

        }


        public static ExecResult<ConnectorData> Execute(EA.DiagramObject firstDA, EA.DiagramObject secondDA)
        {
            ExecResult<ConnectorData> result = new ExecResult<ConnectorData>() { value = new ConnectorData() };
            try
            {
                var fCreateNewLink = new FCreateNewLink();

                EA.Element firstElement = Context.EARepository.GetElementByID(firstDA.ElementID);
                fCreateNewLink.tbSource.Text = Logger.DumpObject(firstElement);
                fCreateNewLink.tbSource.Tag = firstElement;

                EA.Element secondElement = Context.EARepository.GetElementByID(secondDA.ElementID);
                fCreateNewLink.tbDestination.Text = Logger.DumpObject(secondElement);
                fCreateNewLink.tbDestination.Tag = secondElement;

                fCreateNewLink.cbFlowID.Items.Clear();
                fCreateNewLink.cbFlowID.Items.AddRange(ConnectorHelper.GetCurrentFlowIDs());


                DialogResult res = fCreateNewLink.ShowDialog();

                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    if (fCreateNewLink.clbLinkType.CheckedItems.Count == 0)
                    {
                        result.code = (int)DialogResult.Cancel;
                    }
                    else
                    {
                        result.value.Name = fCreateNewLink.tbFlowName.Text;
                        result.value.LinkType = ((LinkType)fCreateNewLink.clbLinkType.CheckedItems[0]);
                        result.value.FlowID = fCreateNewLink.cbFlowID.Text;
                        result.value.SegmentID = fCreateNewLink.cbSegmentID.Text;

                        result.value.SourceElementID = ((EA.Element)fCreateNewLink.tbSource.Tag).ElementID;
                        result.value.TargetElementID = ((EA.Element)fCreateNewLink.tbDestination.Tag).ElementID;
                        //result.value.tempLink = cbTempLink.Checked;
                        //result.value.tempLinkDiagramID = tbTempLinkDiagramID.Text;
                    }
                }


            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        private void clbLinkType_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if (checkedContext == null) checkedContext = e.Index;
                ChangeAllCheckBoxValues(false);
                checkedContext = null;
            }
        }

        private void ChangeAllCheckBoxValues(bool value)
        {
            for (int i = 1; i < clbLinkType.Items.Count; i++)
            {
                if (i != checkedContext)
                    clbLinkType.SetItemChecked(i, value);
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

        private void btnSwitchSourceDestination_Click(object sender, EventArgs e)
        {
            object xxx = tbSource.Tag;
            string sxxx = tbSource.Text;
            tbSource.Tag = tbDestination.Tag;
            tbSource.Text = tbDestination.Text;
            tbDestination.Tag = xxx;
            tbDestination.Text = sxxx;

        }

        private void btnSuggestFromSource_Click(object sender, EventArgs e)
        {
            cbFlowID.Text = LibraryHelper.SuggestFlowIDName((EA.Element)tbSource.Tag);
        }

        private void cbFlowID_TextUpdate(object sender, EventArgs e)
        {
            if (tbFlowName.Text == "")
            {
                tbFlowName.Text = cbFlowID.Text;
            }
        }

        private void btnSuggestFromDest_Click(object sender, EventArgs e)
        {
            cbFlowID.Text = LibraryHelper.SuggestFlowIDName((EA.Element)tbDestination.Tag);
        }


    }

}
