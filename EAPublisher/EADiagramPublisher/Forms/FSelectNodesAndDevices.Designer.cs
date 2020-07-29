namespace EADiagramPublisher.Forms
{
    partial class FSelectNodesAndDevices
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode11});
            this.btnOk = new System.Windows.Forms.Button();
            this.tvNodes = new System.Windows.Forms.TreeView();
            this.btnExpandAll = new System.Windows.Forms.Button();
            this.gbSelectnodeGroup = new System.Windows.Forms.GroupBox();
            this.clbNodeGroups = new System.Windows.Forms.CheckedListBox();
            this.cbCurContourNodeGroup = new System.Windows.Forms.CheckBox();
            this.gbSelectnodeGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(855, 725);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(106, 44);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // tvNodes
            // 
            this.tvNodes.CheckBoxes = true;
            this.tvNodes.Location = new System.Drawing.Point(12, 36);
            this.tvNodes.Name = "tvNodes";
            treeNode11.Checked = true;
            treeNode11.Name = "Node1";
            treeNode11.Text = "Node1";
            treeNode12.Name = "Node0";
            treeNode12.Text = "Node0";
            this.tvNodes.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode12});
            this.tvNodes.Size = new System.Drawing.Size(510, 733);
            this.tvNodes.TabIndex = 9;
            // 
            // btnExpandAll
            // 
            this.btnExpandAll.Location = new System.Drawing.Point(12, 7);
            this.btnExpandAll.Name = "btnExpandAll";
            this.btnExpandAll.Size = new System.Drawing.Size(232, 23);
            this.btnExpandAll.TabIndex = 10;
            this.btnExpandAll.Text = "Expand All";
            this.btnExpandAll.UseVisualStyleBackColor = true;
            this.btnExpandAll.Click += new System.EventHandler(this.btnExpandAll_Click);
            // 
            // gbSelectnodeGroup
            // 
            this.gbSelectnodeGroup.Controls.Add(this.cbCurContourNodeGroup);
            this.gbSelectnodeGroup.Controls.Add(this.clbNodeGroups);
            this.gbSelectnodeGroup.Location = new System.Drawing.Point(559, 36);
            this.gbSelectnodeGroup.Name = "gbSelectnodeGroup";
            this.gbSelectnodeGroup.Size = new System.Drawing.Size(402, 683);
            this.gbSelectnodeGroup.TabIndex = 11;
            this.gbSelectnodeGroup.TabStop = false;
            this.gbSelectnodeGroup.Text = "Выделить сервера группы";
            // 
            // clbNodeGroups
            // 
            this.clbNodeGroups.CheckOnClick = true;
            this.clbNodeGroups.FormattingEnabled = true;
            this.clbNodeGroups.Location = new System.Drawing.Point(6, 25);
            this.clbNodeGroups.Name = "clbNodeGroups";
            this.clbNodeGroups.ScrollAlwaysVisible = true;
            this.clbNodeGroups.Size = new System.Drawing.Size(390, 613);
            this.clbNodeGroups.TabIndex = 0;
            this.clbNodeGroups.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbNodeGroups_ItemCheck);
            // 
            // cbCurContourNodeGroup
            // 
            this.cbCurContourNodeGroup.AutoSize = true;
            this.cbCurContourNodeGroup.Checked = true;
            this.cbCurContourNodeGroup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCurContourNodeGroup.Enabled = false;
            this.cbCurContourNodeGroup.Location = new System.Drawing.Point(6, 653);
            this.cbCurContourNodeGroup.Name = "cbCurContourNodeGroup";
            this.cbCurContourNodeGroup.Size = new System.Drawing.Size(293, 24);
            this.cbCurContourNodeGroup.TabIndex = 1;
            this.cbCurContourNodeGroup.Text = "Только для выделенного контура";
            this.cbCurContourNodeGroup.UseVisualStyleBackColor = true;
            // 
            // FSelectNodesAndDevices
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 781);
            this.Controls.Add(this.gbSelectnodeGroup);
            this.Controls.Add(this.btnExpandAll);
            this.Controls.Add(this.tvNodes);
            this.Controls.Add(this.btnOk);
            this.Name = "FSelectNodesAndDevices";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FSelectNodesAndDevices";
            this.gbSelectnodeGroup.ResumeLayout(false);
            this.gbSelectnodeGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TreeView tvNodes;
        private System.Windows.Forms.Button btnExpandAll;
        private System.Windows.Forms.GroupBox gbSelectnodeGroup;
        private System.Windows.Forms.CheckedListBox clbNodeGroups;
        private System.Windows.Forms.CheckBox cbCurContourNodeGroup;
    }
}