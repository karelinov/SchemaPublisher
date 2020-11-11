namespace EADiagramPublisher.Forms
{
    partial class FSelectComponentLevel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSelectComponentLevel));
            this.clbLinkTypes = new System.Windows.Forms.CheckedListBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbCheckAll = new System.Windows.Forms.ToolStripButton();
            this.tsbClearSelection = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // clbLinkTypes
            // 
            this.clbLinkTypes.CheckOnClick = true;
            this.clbLinkTypes.FormattingEnabled = true;
            this.clbLinkTypes.Location = new System.Drawing.Point(13, 55);
            this.clbLinkTypes.Name = "clbLinkTypes";
            this.clbLinkTypes.Size = new System.Drawing.Size(310, 235);
            this.clbLinkTypes.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(196, 297);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 39);
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
            this.toolStrip1.Size = new System.Drawing.Size(341, 55);
            this.toolStrip1.TabIndex = 2;
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
            this.tsbCheckAll.Click += new System.EventHandler(this.tsbCheckAll_Click);
            // 
            // tsbClearSelection
            // 
            this.tsbClearSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClearSelection.Image = ((System.Drawing.Image)(resources.GetObject("tsbClearSelection.Image")));
            this.tsbClearSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClearSelection.Name = "tsbClearSelection";
            this.tsbClearSelection.Size = new System.Drawing.Size(52, 52);
            this.tsbClearSelection.Text = "ClearAll";
            this.tsbClearSelection.Click += new System.EventHandler(this.tsbClearSelection_Click);
            // 
            // FSelectLinkType
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 353);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.clbLinkTypes);
            this.Name = "FSelectLinkType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FSelectLinkType";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbLinkTypes;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbClearSelection;
        private System.Windows.Forms.ToolStripButton tsbCheckAll;
    }
}