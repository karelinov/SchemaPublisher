namespace EADiagramPublisher.Forms
{
    partial class FSelectFlowID
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSelectFlowID));
            this.clbFlowIDs = new System.Windows.Forms.CheckedListBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbCheckAll = new System.Windows.Forms.ToolStripButton();
            this.tsbClearSelection = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // clbFlowIDs
            // 
            this.clbFlowIDs.CheckOnClick = true;
            this.clbFlowIDs.FormattingEnabled = true;
            this.clbFlowIDs.Location = new System.Drawing.Point(11, 43);
            this.clbFlowIDs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clbFlowIDs.Name = "clbFlowIDs";
            this.clbFlowIDs.Size = new System.Drawing.Size(493, 242);
            this.clbFlowIDs.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(368, 303);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(135, 33);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCheckAll,
            this.tsbClearSelection});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(514, 55);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbCheckAll
            // 
            this.tsbCheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCheckAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbCheckAll.Image")));
            this.tsbCheckAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCheckAll.Name = "tsbCheckAll";
            this.tsbCheckAll.Size = new System.Drawing.Size(52, 52);
            this.tsbCheckAll.Text = "CheckAll";
            this.tsbCheckAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // tsbClearSelection
            // 
            this.tsbClearSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClearSelection.Image = ((System.Drawing.Image)(resources.GetObject("tsbClearSelection.Image")));
            this.tsbClearSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClearSelection.Name = "tsbClearSelection";
            this.tsbClearSelection.Size = new System.Drawing.Size(52, 52);
            this.tsbClearSelection.Text = "ClearAll";
            this.tsbClearSelection.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // FSelectFlowID
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 342);
            this.Controls.Add(this.clbFlowIDs);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnOk);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FSelectFlowID";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FSelectFlowID";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        public System.Windows.Forms.CheckedListBox clbFlowIDs;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbCheckAll;
        private System.Windows.Forms.ToolStripButton tsbClearSelection;
    }
}