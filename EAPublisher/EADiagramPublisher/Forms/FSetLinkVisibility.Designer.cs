namespace EADiagramPublisher.Forms
{
    partial class FSetLinkVisibility
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.clbShowLinkType = new System.Windows.Forms.CheckedListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbShowNotLibConnectors = new System.Windows.Forms.CheckBox();
            this.clbHideLinkType = new System.Windows.Forms.CheckedListBox();
            this.cbHideTempDiagramLinks = new System.Windows.Forms.CheckBox();
            this.gbHide = new System.Windows.Forms.GroupBox();
            this.gbShow = new System.Windows.Forms.GroupBox();
            this.gbComponents = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.cbShowOnlyForSelected = new System.Windows.Forms.CheckBox();
            this.gbHide.SuspendLayout();
            this.gbShow.SuspendLayout();
            this.gbComponents.SuspendLayout();
            this.SuspendLayout();
            // 
            // clbShowLinkType
            // 
            this.clbShowLinkType.CheckOnClick = true;
            this.clbShowLinkType.FormattingEnabled = true;
            this.clbShowLinkType.Location = new System.Drawing.Point(18, 33);
            this.clbShowLinkType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clbShowLinkType.Name = "clbShowLinkType";
            this.clbShowLinkType.Size = new System.Drawing.Size(310, 193);
            this.clbShowLinkType.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(838, 693);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(134, 38);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // cbShowNotLibConnectors
            // 
            this.cbShowNotLibConnectors.AutoSize = true;
            this.cbShowNotLibConnectors.Location = new System.Drawing.Point(18, 244);
            this.cbShowNotLibConnectors.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbShowNotLibConnectors.Name = "cbShowNotLibConnectors";
            this.cbShowNotLibConnectors.Size = new System.Drawing.Size(310, 24);
            this.cbShowNotLibConnectors.TabIndex = 4;
            this.cbShowNotLibConnectors.Text = "Показывать небиблиотечные линки";
            this.cbShowNotLibConnectors.UseVisualStyleBackColor = true;
            // 
            // clbHideLinkType
            // 
            this.clbHideLinkType.CheckOnClick = true;
            this.clbHideLinkType.FormattingEnabled = true;
            this.clbHideLinkType.Location = new System.Drawing.Point(19, 24);
            this.clbHideLinkType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clbHideLinkType.Name = "clbHideLinkType";
            this.clbHideLinkType.Size = new System.Drawing.Size(344, 193);
            this.clbHideLinkType.TabIndex = 1;
            // 
            // cbHideTempDiagramLinks
            // 
            this.cbHideTempDiagramLinks.AutoSize = true;
            this.cbHideTempDiagramLinks.Enabled = false;
            this.cbHideTempDiagramLinks.Location = new System.Drawing.Point(23, 221);
            this.cbHideTempDiagramLinks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbHideTempDiagramLinks.Name = "cbHideTempDiagramLinks";
            this.cbHideTempDiagramLinks.Size = new System.Drawing.Size(357, 24);
            this.cbHideTempDiagramLinks.TabIndex = 4;
            this.cbHideTempDiagramLinks.Text = "Скрыть временные линки чужих диаграмм";
            this.cbHideTempDiagramLinks.UseVisualStyleBackColor = true;
            // 
            // gbHide
            // 
            this.gbHide.Controls.Add(this.cbHideTempDiagramLinks);
            this.gbHide.Controls.Add(this.clbHideLinkType);
            this.gbHide.Location = new System.Drawing.Point(596, 23);
            this.gbHide.Name = "gbHide";
            this.gbHide.Size = new System.Drawing.Size(386, 257);
            this.gbHide.TabIndex = 6;
            this.gbHide.TabStop = false;
            this.gbHide.Text = "Скрыть";
            // 
            // gbShow
            // 
            this.gbShow.Controls.Add(this.gbComponents);
            this.gbShow.Controls.Add(this.cbShowOnlyForSelected);
            this.gbShow.Controls.Add(this.cbShowNotLibConnectors);
            this.gbShow.Controls.Add(this.clbShowLinkType);
            this.gbShow.Location = new System.Drawing.Point(12, 23);
            this.gbShow.Name = "gbShow";
            this.gbShow.Size = new System.Drawing.Size(578, 708);
            this.gbShow.TabIndex = 7;
            this.gbShow.TabStop = false;
            this.gbShow.Text = "Показать";
            // 
            // gbComponents
            // 
            this.gbComponents.Controls.Add(this.treeView1);
            this.gbComponents.Location = new System.Drawing.Point(6, 318);
            this.gbComponents.Name = "gbComponents";
            this.gbComponents.Size = new System.Drawing.Size(496, 371);
            this.gbComponents.TabIndex = 8;
            this.gbComponents.TabStop = false;
            this.gbComponents.Text = "Показ инфопотоков";
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(12, 34);
            this.treeView1.Name = "treeView1";
            treeNode1.Checked = true;
            treeNode1.Name = "Node1";
            treeNode1.Text = "Node1";
            treeNode2.Name = "Node0";
            treeNode2.Text = "Node0";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeView1.Size = new System.Drawing.Size(466, 331);
            this.treeView1.TabIndex = 8;
            // 
            // cbShowOnlyForSelected
            // 
            this.cbShowOnlyForSelected.AutoSize = true;
            this.cbShowOnlyForSelected.Location = new System.Drawing.Point(18, 289);
            this.cbShowOnlyForSelected.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbShowOnlyForSelected.Name = "cbShowOnlyForSelected";
            this.cbShowOnlyForSelected.Size = new System.Drawing.Size(310, 24);
            this.cbShowOnlyForSelected.TabIndex = 4;
            this.cbShowOnlyForSelected.Text = "Только для выделенных элементов";
            this.cbShowOnlyForSelected.UseVisualStyleBackColor = true;
            // 
            // FSetLinkVisibility
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 750);
            this.Controls.Add(this.gbShow);
            this.Controls.Add(this.gbHide);
            this.Controls.Add(this.btnOK);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FSetLinkVisibility";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Установка видимости (библиотечных связей)";
            this.gbHide.ResumeLayout(false);
            this.gbHide.PerformLayout();
            this.gbShow.ResumeLayout(false);
            this.gbShow.PerformLayout();
            this.gbComponents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.CheckedListBox clbShowLinkType;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox cbShowNotLibConnectors;
        public System.Windows.Forms.CheckedListBox clbHideLinkType;
        private System.Windows.Forms.CheckBox cbHideTempDiagramLinks;
        private System.Windows.Forms.GroupBox gbHide;
        private System.Windows.Forms.GroupBox gbShow;
        private System.Windows.Forms.GroupBox gbComponents;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.CheckBox cbShowOnlyForSelected;
    }
}