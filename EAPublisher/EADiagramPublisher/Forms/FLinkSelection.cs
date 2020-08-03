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
        }

        public ExecResult<LinksOperationData> Execute(List<ConnectorData> connectorDataList)
        {
            ExecResult<LinksOperationData> result = new ExecResult<LinksOperationData>();
            try
            {
                // Заливаем список узлов в дерево
                tvLinks.Nodes.Clear();

                foreach (var connectorData in connectorDataList)
                {

                    // Строим узел
                    TreeNode treeNode = new TreeNode(connectorData.Name);
                    treeNode.Tag = connectorData;

                    // Определяем родительский узел
                    TreeNode parentNode = GetNodeForConnectorData(connectorData);
                    parentNode.Nodes.Add(treeNode);

                }

                DialogResult res = this.ShowDialog();
                if (res == DialogResult.OK)
                {
                    result.code = 0;

                    // Операция:
                    if (rbResetAll.Checked)
                        result.value.Operation = LinkSOperation.ResetAll;
                    else
                        result.value.Operation = LinkSOperation.SetView;


                    // Линия и Цвет
                    result.value.Color = pbColor.BackColor;
                    result.value.LineSize = (int)nudLineSize.Value;

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
            if (node.Checked && ((ConnectorData)node.Tag)._ConnectorID != 0)
                result.Add((ConnectorData)node.Tag);

            foreach (TreeNode childNode in node.Nodes)
            {
                result.AddRange(GetTreeCheckedNodesData(childNode));
            }

            return result;
        }


    }
}
