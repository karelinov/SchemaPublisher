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
    public partial class FLinkSelection : Form
    {
        public FLinkSelection()
        {
            InitializeComponent();

            cbLineStyle.SelectedIndex = 0;

            clbNodeGroups.Items.Clear();
            clbNodeGroups.Items.AddRange(LibraryHelper.GetNodeGroupEnum().ToArray());

        }

        public ExecResult<LinksOperationData> Execute(List<ConnectorData> сonnectorDataList)
        {
            ExecResult<LinksOperationData> result = new ExecResult<LinksOperationData>();
            try
            {
                // Заливаем список узлов в дерево
                tvLinks.Nodes.Clear();

                foreach(ConnectorData connectorData in сonnectorDataList)
                {
                    TreeNode connectorDataNode = new TreeNode(connectorData.NameForShow());
                    connectorDataNode.Tag = connectorData;

                    TreeNode parentNode = GetNodeForConnectorData(connectorData);
                    parentNode.Nodes.Add(connectorDataNode);
                }

                DialogResult res = this.ShowDialog();
                if (res == DialogResult.OK)
                {
                    result.code = 0;
                    result.value = new LinksOperationData() { Connectors = new List<ConnectorData>()};

                    // Операция:
                    if (rbResetAll.Checked)
                        result.value.Operation = LinkSOperation.ResetAll;
                    else
                        result.value.Operation = LinkSOperation.SetStyle;

                    // Линия и Цвет
                    result.value.SetLineWidth = chkSetLineSize.Checked;
                    result.value.LineWidth = (int)nudLineSize.Value;

                    result.value.SetColor = chkSetColor.Checked;
                    result.value.Color = pbColor.BackColor;

                    result.value.SetLineStyle = chkSetLineStyle.Checked;
                    if (cbLineStyle.SelectedIndex == 0)
                        result.value.LineStyle = EA.LinkLineStyle.LineStyleOrthogonalRounded;
                    else
                        result.value.LineStyle = EA.LinkLineStyle.LineStyleDirect; 


                    // Собираем список отмеченных
                    result.value.Connectors = new List<ConnectorData>();
                    foreach (TreeNode node in tvLinks.Nodes)
                    {
                        result.value.Connectors.AddRange(GetTreeCheckedNodesData(node));
                    }
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

        /// <summary>
        /// Ищет родительский узел для узла, если не находит - создаёт
        /// Создаются служебные узлы уровня TBD
        /// </summary>
        /// <param name="nodeData"></param>
        /// <returns></returns>
        private TreeNode GetNodeForConnectorData(ConnectorData connectorData)
        {
            TreeNode linkTypeNode = null;

            foreach (TreeNode node in tvLinks.Nodes)
            {
                if (node.Text == connectorData.LinkType.ToString())
                {
                    linkTypeNode = node;
                    break;
                }
            }

            if (linkTypeNode == null)
            {
                linkTypeNode = new TreeNode(connectorData.LinkType.ToString());
                ConnectorData linkTypeNodeData = new ConnectorData();
                linkTypeNodeData.LinkType = connectorData.LinkType;
                linkTypeNodeData.Name = connectorData.LinkType.ToString();
                linkTypeNode.Tag = linkTypeNodeData;

                tvLinks.Nodes.Add(linkTypeNode);
            }

            // Для всех узлов кроме InformationFlow нужен только 1 уровень  = linkType
            if(connectorData.LinkType != LinkType.InformationFlow)
            {
                return linkTypeNode;
            }

            // для InformationFlow делаем ещё один уровень с flowID

            TreeNode flowIDLevelNode = null;

            string flowID;
            if (connectorData.FlowID == null)
                flowID = "";
            else
                flowID = connectorData.FlowID;

            foreach (TreeNode node in linkTypeNode.Nodes)
            {
                if (node.Text == flowID)
                {
                    flowIDLevelNode = node;
                    break;
                }
            }

            if (flowIDLevelNode == null)
            {
                flowIDLevelNode = new TreeNode(flowID);

                ConnectorData flowIDLevelNodeData = new ConnectorData();
                flowIDLevelNodeData.FlowID = connectorData.FlowID;
                flowIDLevelNodeData.Name = connectorData.FlowID;
                linkTypeNode.Tag = flowIDLevelNodeData;

                linkTypeNode.Nodes.Add(flowIDLevelNode);
            }

            return flowIDLevelNode;
        }


        /// <summary>
        /// Вспомогательная рекурсивная функция для возврата отмеченных узлов с  заполеннным коннектором
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private List<ConnectorData> GetTreeCheckedNodesData(TreeNode node)
        {
            List<ConnectorData> result = new List<ConnectorData>();
            if (node.Checked && node.Tag!=null && ((ConnectorData)node.Tag)._ConnectorID != 0)
                result.Add((ConnectorData)node.Tag);

            foreach (TreeNode childNode in node.Nodes)
            {
                result.AddRange(GetTreeCheckedNodesData(childNode));
            }

            return result;
        }

        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            DialogResult dialogResult = colorDialog.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                pbColor.BackColor = colorDialog.Color;
            }
        }

        private void btnExpandTree_Click(object sender, EventArgs e)
        {
            tvLinks.ExpandAll();
        }


        // Updates all child tree nodes recursively.
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void tvLinks_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckAllChildNodes(e.Node, e.Node.Checked);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            // Выделение коннекторов, связанных с элементами, относящимися к ПО
            if(clbNodeGroups.SelectedItems.Count > 0)
            {
                List<string> selectedSoftware = new List<string>();
                foreach (var selectedItem in clbNodeGroups.SelectedItems)
                    selectedSoftware.Add(selectedItem.ToString());


                List<ConnectorData> connectorsToSelect = LinkDesignerHelper.GetDAForSoftwareOnDiagram(selectedSoftware);
            }

            



        }
    }
}
