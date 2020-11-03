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
    public partial class FSelectSoftwareClassification : Form
    {
        public FSelectSoftwareClassification()
        {
            InitializeComponent();
        }


        public static ExecResult<int[]> Execute(int[] alreadySelectedObjects)
        {
            var result = new ExecResult<int[]>();
            try
            {
                var form = new FSelectSoftwareClassification();
                form.LoadSoftwareClassification();
                if (alreadySelectedObjects != null)
                    form.SelectSoftwareClassification(form.tvSoftwareClassification.Nodes[0],alreadySelectedObjects);


                DialogResult res = form.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    List<int> selectedObjects = new List<int>();
                    foreach (TreeNode checkedNode in form.GetCheckedNodes(form.tvSoftwareClassification.Nodes[0]))
                    {
                        selectedObjects.Add(((ElementData)checkedNode.Tag)._ElementID);
                    }

                    result.value = selectedObjects.ToArray();
                }
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        public List<TreeNode> GetCheckedNodes(TreeNode node)
        {
            List<TreeNode> result = new List<TreeNode>();

            if (node.Checked)
                result.Add(node);

            foreach(TreeNode childNode in node.Nodes)
            {
                result.AddRange(GetCheckedNodes(childNode));
            }

            return result;
        }

        /// <summary>
        /// Функция конструирования дерева классификации ПО
        /// </summary>
        public void LoadSoftwareClassification()
        {
            DPTreeNode<ElementData> dpTreeNode = Context.SoftwareClassification;

            TreeNode node = new TreeNode(dpTreeNode.Value.DisplayName) { Tag = dpTreeNode.Value };
            tvSoftwareClassification.Nodes.Add(node);

            SetTreeView(node, dpTreeNode.Children.ToList());
        }

        private void SetTreeView(TreeNode parentNode, List<DPTreeNode<ElementData>> dpTreeNodeList)
        {
            foreach (var curDPTreeNode in dpTreeNodeList)
            {
                TreeNode node = new TreeNode(curDPTreeNode.Value.DisplayName) { Tag = curDPTreeNode.Value };
                parentNode.Nodes.Add(node);

                SetTreeView(node, curDPTreeNode.Children.ToList());
            }

        }

        /// <summary>
        /// Функция выделения узлов ПО в дереве по списку
        /// </summary>
        /// <param name="alreadySelectedObjects"></param>
        public void SelectSoftwareClassification(TreeNode node, int[] alreadySelectedObjects)
        {
            node.Checked = (alreadySelectedObjects.Contains(((ElementData)node.Tag)._ElementID));

            foreach (TreeNode subNode in node.Nodes)
            {
                SelectSoftwareClassification(subNode, alreadySelectedObjects);
            }
        }


        private void tsbExpandAll_Click(object sender, EventArgs e)
        {
            tvSoftwareClassification.ExpandAll();
        }

        private void tsbCollapseAll_Click(object sender, EventArgs e)
        {
            tvSoftwareClassification.CollapseAll();
        }


    }
}
