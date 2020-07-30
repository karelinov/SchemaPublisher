using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EADiagramPublisher.Enums;
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

        /// <summary>
        /// Ищет родительский узел для узла, если не находит - создаёт
        /// Создаются служебные узлы уровня компонента (Node, Device) и контура (с названиями контуров)
        /// </summary>
        /// <param name="nodeData"></param>
        /// <returns></returns>
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
                NodeData componentNodeData = new NodeData();
                componentNodeData.ComponentLevel = nodeData.ComponentLevel;
                componentLevelNode.Tag = componentNodeData;

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

                NodeData contourNodeData = new NodeData();
                contourNodeData.ComponentLevel = ComponentLevel.ContourComponent;
                contourNodeData.Contour = nodeData.Contour;
                countourLevelNode.Tag = contourNodeData;

                componentLevelNode.Nodes.Add(countourLevelNode);
            }

            return countourLevelNode;
        }

        /// <summary>
        /// Вспомогательная рекурсивная функция для возврата отмеченных узлов с ComponentLevel = Node/Device и заполеннным элементом
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private List<NodeData> GetTreeCheckedNodesData(TreeNode node)
        {
            List<NodeData> result = new List<NodeData>();
            if (node.Checked && (new ComponentLevel[] { ComponentLevel.Device, ComponentLevel.Node}).Contains(((NodeData)node.Tag).ComponentLevel) && (((NodeData)node.Tag).Element != null))
                result.Add((NodeData)node.Tag);

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

            // если установлена галочка "только для выделенного контура", пытаемся его определить
            TreeNode countourNode = null;
            if (cbonlySelectedContour.Checked && tvNodes.SelectedNode != null)
            {
                TreeNode selectedNode = tvNodes.SelectedNode;
                NodeData selectednodeData = (NodeData)selectedNode.Tag;
                if (selectednodeData.ComponentLevel == ComponentLevel.ContourComponent)
                {
                    countourNode = tvNodes.SelectedNode;
                } 
            }


            foreach (TreeNode node in (countourNode !=null)? countourNode.Nodes: tvNodes.Nodes)
            {
                SetCheckStateForGroup(node, groupName, e.NewValue == CheckState.Checked);
            }

        }


        /// <summary>
        /// Вспомогательная рекурсивная функция установки CheckState для дерева, начиная с указанного узла
        /// Устанавливаются состояния для узлов, входящих в указанную группу
        /// </summary>
        /// <param name="node"></param>
        /// <param name="groupName"></param>
        /// <param name="checkState"></param>
        private void SetCheckStateForGroup(TreeNode node, string groupName, bool checkState)
        {
            if (node.Tag != null && ((NodeData)node.Tag).GroupNames !=null)
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

        private void btnReverseBranch_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = tvNodes.SelectedNode;
            if (selectedNode == null) return;

            List<TreeNode> currentLevelNodes = new List<TreeNode>();
            foreach (TreeNode node in selectedNode.Nodes)
                currentLevelNodes.Add(node);

            while (currentLevelNodes.Count > 0)
            {
                foreach(TreeNode node in currentLevelNodes)
                {
                    node.Checked = !node.Checked;
                }

                List<TreeNode> nextLevelNodes = new List<TreeNode>();
                foreach(TreeNode node in currentLevelNodes)
                {
                    foreach (TreeNode childNode in node.Nodes)
                        nextLevelNodes.Add(childNode);
                }

                currentLevelNodes = nextLevelNodes;
            }


        }
    }
}
