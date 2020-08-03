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
    public partial class FAddTag : Form
    {
        public FAddTag()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Форма для создания или редауктирования стандартных taggedValues
        /// </summary>
        /// <param name="standardTags"></param>
        /// <param name="CreateMode"></param>
        /// <returns></returns>
        public ExecResult<TagData> Execute(string[] standardTags, bool CreateMode = true, TagData dataToEdit = null)
        {
            ExecResult<TagData> result = new ExecResult<TagData>() { value = new TagData() };
            try
            {
                cbTagName.Items.Clear();
                cbTagName.Items.AddRange(standardTags);

                if (!CreateMode)
                {
                    cbTagName.SelectedIndex = cbTagName.Items.IndexOf(dataToEdit.TagName);
                    cbTagName.Enabled = false;
                    cbTagValue.Text = dataToEdit.TagValue;
                }


                DialogResult res = this.ShowDialog();
                if (res == DialogResult.OK && cbTagName.Text.Trim() !="")
                {
                    result.code = 0;
                    result.value.TagName = cbTagName.Text;
                    result.value.TagValue = cbTagValue.Text;
                    result.value.TagState = true;
                }
                else
                {
                    result.code = (int)DialogResult.Cancel;
                }

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        private void cbTagName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbTagName.Text == DAConst.DP_LinkTypeTag)
            {
                cbTagValue.Items.Clear();
                cbTagValue.Items.AddRange(Enum.GetNames(typeof(LinkType)));
            }
            else if(cbTagName.Text == DAConst.DP_ComponentLevelTag)
            {
                cbTagValue.Items.Clear();
                cbTagValue.Items.AddRange(Enum.GetNames(typeof(ComponentLevel)));
            }
            else if(cbTagName.Text == DAConst.DP_NodeGroupsTag)
            {
                cbTagValue.Items.Clear();
                cbTagValue.Items.AddRange(EAHelper.GetNodeGroupEnum().ToArray());
            }
            else if (cbTagName.Text == DAConst.DP_FlowIDTag)
            {
                cbTagValue.Items.Clear();
                cbTagValue.Items.AddRange(Context.ConnectorData[LinkType.InformationFlow].Keys.ToArray());
            }
            else
            {
                cbTagValue.Items.Clear();
            }
        }
    }
}
