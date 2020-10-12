namespace EADiagramPublisher.Forms
{
    partial class FSelectDiagramObjects
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSelectDiagramObjects));
            this.btnOk = new System.Windows.Forms.Button();
            this.lvDiagramObjects = new System.Windows.Forms.ListView();
            this.chID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chEAType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chComponentType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chNotes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbShow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbHide = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(1106, 586);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 35);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lvDiagramObjects
            // 
            this.lvDiagramObjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chID,
            this.chName,
            this.chEAType,
            this.chComponentType,
            this.chNotes});
            this.lvDiagramObjects.FullRowSelect = true;
            this.lvDiagramObjects.Location = new System.Drawing.Point(13, 96);
            this.lvDiagramObjects.Name = "lvDiagramObjects";
            this.lvDiagramObjects.Size = new System.Drawing.Size(1197, 473);
            this.lvDiagramObjects.TabIndex = 1;
            this.lvDiagramObjects.UseCompatibleStateImageBehavior = false;
            this.lvDiagramObjects.View = System.Windows.Forms.View.Details;
            this.lvDiagramObjects.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvDiagramObjects_ColumnClick);
            // 
            // chID
            // 
            this.chID.Text = "ID";
            this.chID.Width = 89;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 146;
            // 
            // chEAType
            // 
            this.chEAType.Text = "EAType";
            this.chEAType.Width = 136;
            // 
            // chComponentType
            // 
            this.chComponentType.Text = "ComponentType";
            this.chComponentType.Width = 183;
            // 
            // chNotes
            // 
            this.chNotes.Text = "Notes";
            this.chNotes.Width = 601;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbShow,
            this.toolStripSeparator1,
            this.tsbHide});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1222, 55);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbShow
            // 
            this.tsbShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbShow.Image = ((System.Drawing.Image)(resources.GetObject("tsbShow.Image")));
            this.tsbShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShow.Name = "tsbShow";
            this.tsbShow.Size = new System.Drawing.Size(52, 52);
            this.tsbShow.Text = "Показать коннекторы";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 55);
            // 
            // tsbHide
            // 
            this.tsbHide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbHide.Image = ((System.Drawing.Image)(resources.GetObject("tsbHide.Image")));
            this.tsbHide.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHide.Name = "tsbHide";
            this.tsbHide.Size = new System.Drawing.Size(52, 52);
            this.tsbHide.Text = "Скрыть коннекторы";
            // 
            // FSelectDiagramObjects
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1222, 633);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lvDiagramObjects);
            this.Controls.Add(this.btnOk);
            this.Name = "FSelectDiagramObjects";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FSelectDiagramObjects";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ListView lvDiagramObjects;
        private System.Windows.Forms.ColumnHeader chID;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chEAType;
        private System.Windows.Forms.ColumnHeader chComponentType;
        private System.Windows.Forms.ColumnHeader chNotes;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbShow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbHide;
    }
}