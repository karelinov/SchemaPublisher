using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EADiagramPublisher.Contracts;

namespace EADiagramPublisher.Forms
{
    public partial class FSelectNodesAndDevices : Form
    {
        public FSelectNodesAndDevices()
        {
            InitializeComponent();

            clbNodeGroups.Items.Clear();
            clbNodeGroups.Items.AddRange(EAHelper.GetNodeGroupEnum().ToArray());

        }


        public ExecResult<List<NodeData>> Execute(List<NodeData> nodeDataList)
        {
            ExecResult<List<NodeData>> result = new ExecResult<List<NodeData>>() { value = new List<NodeData>() };
            try
            {
                // Заливаем список 'элементов в tvNodes
                tvNodes.Nodes.Clear();

                foreach (var nodeData in nodeDataList)
                {

                    // Строим узел
                    TreeNode treeNode = new TreeNode(nodeData.ElementName);
                    treeNode.Tag = nodeData;

                    // Определяем родительский узел
                    TreeNode parentNode = GetNodeForNodeData(nodeData);
                    parentNode.Nodes.Add(treeNode);

                }

                DialogResult res = this.ShowDialog();
                if (res == DialogResult.OK)
                {
                    result.code = 0;

                    // Собираем список отмеченных
                    foreach (TreeNode node in tvNodes.Nodes)
                    {
                        result.value.AddRange(GetTreeCheckedNodesData(node));
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

        // Ищет родительский узел для узла, если не находит - создаёт
        private TreeNode GetNodeForNodeData(NodeData nodeData)
        {
            TreeNode componentLevelNode = null;

            foreach (TreeNode node in tvNodes.Nodes)
            {
                if (node.Text == nodeData.ComponentLevel.ToString())
                {
                    componentLevelNode = node;
                    break;
                }
            }

            if (componentLevelNode == null)
            {
                componentLevelNode = new TreeNode(nodeData.ComponentLevel.ToString());
                tvNodes.Nodes.Add(componentLevelNode);
            }

            TreeNode countourLevelNode = null;

            string countourName;
            if (nodeData.Contour == null)
                countourName = "";
            else
                countourName = nodeData.Contour.Name;

            foreach (TreeNode node in componentLevelNode.Nodes)
            {
                if (node.Text == countourName)
                {
                    countourLevelNode = node;
                    break;
                }
            }

            if (countourLevelNode == null)
            {
                countourLevelNode = new TreeNode(countourName);
                componentLevelNode.Nodes.Add(countourLevelNode);
            }

            return countourLevelNode;
        }

        private List<NodeData> GetTreeCheckedNodesData(TreeNode node)
        {
            List<NodeData> result = new List<NodeData>();
            if (node.Checked && node.Tag != null) result.Add((NodeData)node.Tag);

            foreach (TreeNode childNode in node.Nodes)
            {
                result.AddRange(GetTreeCheckedNodesData(childNode));
            }

            return result;
        }

        private void btnExpandAll_Click(object sender, EventArgs e)
        {
            tvNodes.ExpandAll();
        }

        private void clbNodeGroups_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string groupName = clbNodeGroups.Items[e.Index].ToString();

            foreach (TreeNode node in tvNodes.Nodes)
            {
                SetCheckStateForGroup(node, groupName, e.NewValue == CheckState.Checked);
            }

        }

        private void SetCheckStateForGroup(TreeNode node, string groupName, bool checkState)
        {
            if (node.Tag != null)
            {
                if (((NodeData)node.Tag).GroupNames.Contains(groupName))
                {
                    node.Checked = checkState;
                }

            }

            foreach (TreeNode childNode in node.Nodes)
            {
                SetCheckStateForGroup(childNode, groupName, checkState);
            }

        }
    }
}
