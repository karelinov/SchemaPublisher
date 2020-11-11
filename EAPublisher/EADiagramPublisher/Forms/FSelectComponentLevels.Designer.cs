namespace EADiagramPublisher.Forms

{
    partial class FSelectComponentLevels
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSelectComponentLevels));
            this.clbHierarchyLevels = new System.Windows.Forms.CheckedListBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbCheckAll = new System.Windows.Forms.ToolStripButton();
            this.tsbClearSelection = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // clbHierarchyLevels
            // 
            this.clbHierarchyLevels.CheckOnClick = true;
            this.clbHierarchyLevels.FormattingEnabled = true;
            this.clbHierarchyLevels.Location = new System.Drawing.Point(12, 58);
            this.clbHierarchyLevels.Name = "clbHierarchyLevels";
            this.clbHierarchyLevels.Size = new System.Drawing.Size(459, 340);
            this.clbHierarchyLevels.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(340, 404);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(131, 36);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
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
            this.toolStrip1.Size = new System.Drawing.Size(478, 55);
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
            // FSelectComponentLevels
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.clbHierarchyLevels);
            this.Name = "FSelectComponentLevels";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выберите отображаемые уровни иерархии компонентов";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FSelectHierarcyLevels_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        public System.Windows.Forms.CheckedListBox clbHierarchyLevels;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbCheckAll;
        private System.Windows.Forms.ToolStripButton tsbClearSelection;
    }
}