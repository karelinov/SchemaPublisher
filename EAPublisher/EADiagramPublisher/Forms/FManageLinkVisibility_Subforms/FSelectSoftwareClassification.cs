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
                    form.SelectSoftwareClassification(alreadySelectedObjects);


                DialogResult res = form.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    List<int> selectedObjects = new List<int>();
                    foreach (TreeNode checkedNode in form.GetCheckedNodes(form.tvSoftwareClassification.TopNode))
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

            TreeNode node = new TreeNode() { Tag = dpTreeNode.Value };
            tvSoftwareClassification.Nodes.Add(node);

            SetTreeView(node, dpTreeNode.Children.ToList());
        }

        private void SetTreeView(TreeNode parentNode, List<DPTreeNode<ElementData>> dpTreeNodeList)
        {
            foreach (var curDPTreeNode in dpTreeNodeList)
            {
                TreeNode node = new TreeNode() { Tag = curDPTreeNode.Value };
                parentNode.Nodes.Add(node);

                SetTreeView(node, curDPTreeNode.Children.ToList());
            }

        }

        /// <summary>
        /// Функция выделения узлов ПО в дереве по списку
        /// </summary>
        /// <param name="alreadySelectedObjects"></param>
        public void SelectSoftwareClassification(int[] alreadySelectedObjects)
        {

            int _level = 0;
            TreeNode _currentNode = tvSoftwareClassification.Nodes[0];
            do
            {
                // Собсвенно установка checked
                _currentNode.Checked = (alreadySelectedObjects.Contains(((ElementData)_currentNode.Tag)._ElementID));

                // альше хитрый код обхода дерева по уровням без рекурсии (с SO, интересно, работает?)
                if (_currentNode.Nodes.Count > 0)
                {
                    _currentNode = _currentNode.Nodes[0];
                    _level++;
                }
                else
                {
                    if (_currentNode.NextNode != null)
                        _currentNode = _currentNode.NextNode;
                    else
                    {
                        _currentNode = _currentNode.Parent.NextNode;
                        _level--;
                    }
                }
            }
            while (_level > 0);


        }



    }
}
