using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

            tbTempLinkDiagramID.Text = Context.CurrentDiagram.DiagramGUID.ToString();

        }


        public ExecResult<CreateNewLinkData> Execute()
        {
            ExecResult<CreateNewLinkData> result = new ExecResult<CreateNewLinkData>() { value = new CreateNewLinkData() };
            try
            {
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
                        result.value.linkType = ((LinkType)clbLinkType.CheckedItems[0]);
                        result.value.flowID = tbFlowID.Text;
                        result.value.segmentID = tbSegmentID.Text;
                        result.value.tempLink = cbTempLink.Checked;
                        result.value.tempLinkDiagramID = tbTempLinkDiagramID.Text;
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

    }


    public class CreateNewLinkData
    {
        public LinkType linkType;
        public string flowID;
        public string segmentID;
        public bool tempLink;
        public string tempLinkDiagramID;
    }



}
