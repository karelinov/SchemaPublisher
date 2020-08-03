﻿using System;
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


        public ExecResult<ConnectorData> Execute(EA.DiagramObject firstDA, EA.DiagramObject secondDA)
        {
            ExecResult<ConnectorData> result = new ExecResult<ConnectorData>() { value = new ConnectorData() };
            try
            {
                EA.Element firstElement = Context.EARepository.GetElementByID(firstDA.ElementID);
                tbSource.Text = EAHelper.DumpObject(firstElement);
                tbSource.Tag = firstElement;

                EA.Element secondElement = Context.EARepository.GetElementByID(secondDA.ElementID);
                tbDestination.Text = EAHelper.DumpObject(secondElement);
                tbDestination.Tag = secondElement;

                cbFlowID.Items.Clear();
                cbFlowID.Items.AddRange(Context.ConnectorData[LinkType.InformationFlow].Keys.ToArray());


                DialogResult res = this.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    if(clbLinkType.CheckedItems.Count == 0)
                    {
                        result.code = (int)DialogResult.Cancel;
                    }
                    else
                    {
                        result.value.Name = tbFlowName.Text;
                        result.value.LinkType = ((LinkType)clbLinkType.CheckedItems[0]);
                        result.value.FlowID = cbFlowID.Text;
                        result.value.SegmentID = cbSegmentID.Text;

                        result.value.SourceElementID = ((EA.Element)tbSource.Tag).ElementID;
                        result.value.DestinationElementID = ((EA.Element)tbDestination.Tag).ElementID;
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
                if(i!= checkedContext)
                    clbLinkType.SetItemChecked(i, value);
            }
        }

        private void cbFlowID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string flowID = cbFlowID.Text;

            cbSegmentID.Items.Clear();

            if (flowID != "")
            {
                if(Context.ConnectorData[LinkType.InformationFlow].ContainsKey(flowID))
                {
                    foreach(ConnectorData connectorData in Context.ConnectorData[LinkType.InformationFlow][flowID])
                    {
                        cbSegmentID.Items.Add(connectorData.SegmentID);
                    }
                }
            }


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
            if (tbFlowName.Text =="")
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
