namespace EADiagramPublisher.Forms
{
    partial class FSelectSoftwareClassification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSelectSoftwareClassification));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tvSoftwareClassification = new System.Windows.Forms.TreeView();
            this.btnOk = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tsbSelectAll = new System.Windows.Forms.ToolStripButton();
            this.tsbClearAll = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSelectAll,
            this.tsbClearAll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(824, 55);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tvSoftwareClassification
            // 
            this.tvSoftwareClassification.CheckBoxes = true;
            this.tvSoftwareClassification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSoftwareClassification.FullRowSelect = true;
            this.tvSoftwareClassification.Location = new System.Drawing.Point(0, 55);
            this.tvSoftwareClassification.Name = "tvSoftwareClassification";
            this.tvSoftwareClassification.Size = new System.Drawing.Size(824, 589);
            this.tvSoftwareClassification.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(666, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(146, 32);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 644);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(824, 41);
            this.panel1.TabIndex = 4;
            // 
            // tsbSelectAll
            // 
            this.tsbSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbSelectAll.Image")));
            this.tsbSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSelectAll.Name = "tsbSelectAll";
            this.tsbSelectAll.Size = new System.Drawing.Size(52, 52);
            this.tsbSelectAll.Text = "SelectAll";
            // 
            // tsbClearAll
            // 
            this.tsbClearAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClearAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbClearAll.Image")));
            this.tsbClearAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClearAll.Name = "tsbClearAll";
            this.tsbClearAll.Size = new System.Drawing.Size(52, 52);
            this.tsbClearAll.Text = "ClearAll";
            // 
            // FSelectSoftwareClassification
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 685);
            this.Controls.Add(this.tvSoftwareClassification);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FSelectSoftwareClassification";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FSelectSoftwareClassification";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TreeView tvSoftwareClassification;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton tsbSelectAll;
        private System.Windows.Forms.ToolStripButton tsbClearAll;
    }
}