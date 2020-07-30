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
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.btnOk = new System.Windows.Forms.Button();
            this.tvNodes = new System.Windows.Forms.TreeView();
            this.btnExpandAll = new System.Windows.Forms.Button();
            this.gbSelectnodeGroup = new System.Windows.Forms.GroupBox();
            this.cbonlySelectedContour = new System.Windows.Forms.CheckBox();
            this.clbNodeGroups = new System.Windows.Forms.CheckedListBox();
            this.btnReverseBranch = new System.Windows.Forms.Button();
            this.gbSelectnodeGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(760, 580);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(94, 35);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // tvNodes
            // 
            this.tvNodes.CheckBoxes = true;
            this.tvNodes.Location = new System.Drawing.Point(11, 49);
            this.tvNodes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tvNodes.Name = "tvNodes";
            treeNode5.Checked = true;
            treeNode5.Name = "Node1";
            treeNode5.Text = "Node1";
            treeNode6.Name = "Node0";
            treeNode6.Text = "Node0";
            this.tvNodes.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.tvNodes.Size = new System.Drawing.Size(454, 567);
            this.tvNodes.TabIndex = 9;
            // 
            // btnExpandAll
            // 
            this.btnExpandAll.Location = new System.Drawing.Point(11, 11);
            this.btnExpandAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExpandAll.Name = "btnExpandAll";
            this.btnExpandAll.Size = new System.Drawing.Size(206, 26);
            this.btnExpandAll.TabIndex = 10;
            this.btnExpandAll.Text = "Expand All";
            this.btnExpandAll.UseVisualStyleBackColor = true;
            this.btnExpandAll.Click += new System.EventHandler(this.btnExpandAll_Click);
            // 
            // gbSelectnodeGroup
            // 
            this.gbSelectnodeGroup.Controls.Add(this.cbonlySelectedContour);
            this.gbSelectnodeGroup.Controls.Add(this.clbNodeGroups);
            this.gbSelectnodeGroup.Location = new System.Drawing.Point(497, 29);
            this.gbSelectnodeGroup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbSelectnodeGroup.Name = "gbSelectnodeGroup";
            this.gbSelectnodeGroup.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbSelectnodeGroup.Size = new System.Drawing.Size(357, 546);
            this.gbSelectnodeGroup.TabIndex = 11;
            this.gbSelectnodeGroup.TabStop = false;
            this.gbSelectnodeGroup.Text = "Выделить сервера группы";
            // 
            // cbonlySelectedContour
            // 
            this.cbonlySelectedContour.AutoSize = true;
            this.cbonlySelectedContour.Checked = true;
            this.cbonlySelectedContour.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbonlySelectedContour.Location = new System.Drawing.Point(5, 522);
            this.cbonlySelectedContour.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbonlySelectedContour.Name = "cbonlySelectedContour";
            this.cbonlySelectedContour.Size = new System.Drawing.Size(252, 21);
            this.cbonlySelectedContour.TabIndex = 1;
            this.cbonlySelectedContour.Text = "Только для выделенного контура";
            this.cbonlySelectedContour.UseVisualStyleBackColor = true;
            // 
            // clbNodeGroups
            // 
            this.clbNodeGroups.CheckOnClick = true;
            this.clbNodeGroups.FormattingEnabled = true;
            this.clbNodeGroups.Location = new System.Drawing.Point(5, 20);
            this.clbNodeGroups.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clbNodeGroups.Name = "clbNodeGroups";
            this.clbNodeGroups.ScrollAlwaysVisible = true;
            this.clbNodeGroups.Size = new System.Drawing.Size(347, 480);
            this.clbNodeGroups.TabIndex = 0;
            this.clbNodeGroups.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbNodeGroups_ItemCheck);
            // 
            // btnReverseBranch
            // 
            this.btnReverseBranch.Location = new System.Drawing.Point(223, 11);
            this.btnReverseBranch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReverseBranch.Name = "btnReverseBranch";
            this.btnReverseBranch.Size = new System.Drawing.Size(242, 26);
            this.btnReverseBranch.TabIndex = 10;
            this.btnReverseBranch.Text = "ReverseBranch";
            this.btnReverseBranch.UseVisualStyleBackColor = true;
            this.btnReverseBranch.Click += new System.EventHandler(this.btnReverseBranch_Click);
            // 
            // FSelectNodesAndDevices
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 625);
            this.Controls.Add(this.gbSelectnodeGroup);
            this.Controls.Add(this.btnReverseBranch);
            this.Controls.Add(this.btnExpandAll);
            this.Controls.Add(this.tvNodes);
            this.Controls.Add(this.btnOk);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
        private System.Windows.Forms.CheckBox cbonlySelectedContour;
        private System.Windows.Forms.Button btnReverseBranch;
    }
}