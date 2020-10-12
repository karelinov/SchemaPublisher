namespace EADiagramPublisher.Forms
{
    partial class FManageLinkVisibility
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FManageLinkVisibility));
            this.lvConnectors = new System.Windows.Forms.ListView();
            this.colShown = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhStartElement = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhConnectorType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhFlowID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhSegmentID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhNotes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhEndElement = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panFilters = new System.Windows.Forms.Panel();
            this.gbSourceElementFilter = new System.Windows.Forms.GroupBox();
            this.btnSourceElementFilter = new System.Windows.Forms.Button();
            this.lblSourceElementFilter = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gbFlowIDFilter = new System.Windows.Forms.GroupBox();
            this.lblFlowIDFilter = new System.Windows.Forms.Label();
            this.btnSelectFlowID = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbShow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbHide = new System.Windows.Forms.ToolStripButton();
            this.tsbReloadCurrentDiagram = new System.Windows.Forms.ToolStripButton();
            this.panFilters.SuspendLayout();
            this.gbSourceElementFilter.SuspendLayout();
            this.gbFlowIDFilter.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvConnectors
            // 
            this.lvConnectors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colShown,
            this.colhStartElement,
            this.colhName,
            this.colhConnectorType,
            this.colhFlowID,
            this.colhSegmentID,
            this.colhNotes,
            this.colhEndElement});
            this.lvConnectors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvConnectors.FullRowSelect = true;
            this.lvConnectors.Location = new System.Drawing.Point(0, 155);
            this.lvConnectors.Name = "lvConnectors";
            this.lvConnectors.Size = new System.Drawing.Size(1739, 612);
            this.lvConnectors.TabIndex = 0;
            this.lvConnectors.UseCompatibleStateImageBehavior = false;
            this.lvConnectors.View = System.Windows.Forms.View.Details;
            this.lvConnectors.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvConnectors_ColumnClick);
            // 
            // colShown
            // 
            this.colShown.Text = "Отображается";
            // 
            // colhStartElement
            // 
            this.colhStartElement.Text = "Начало";
            this.colhStartElement.Width = 240;
            // 
            // colhName
            // 
            this.colhName.Text = "Название";
            this.colhName.Width = 271;
            // 
            // colhConnectorType
            // 
            this.colhConnectorType.Text = "Тип";
            this.colhConnectorType.Width = 177;
            // 
            // colhFlowID
            // 
            this.colhFlowID.Text = "FlowID";
            this.colhFlowID.Width = 293;
            // 
            // colhSegmentID
            // 
            this.colhSegmentID.Text = "SegmentID";
            this.colhSegmentID.Width = 101;
            // 
            // colhNotes
            // 
            this.colhNotes.Text = "Описание";
            this.colhNotes.Width = 329;
            // 
            // colhEndElement
            // 
            this.colhEndElement.Text = "Конец";
            this.colhEndElement.Width = 266;
            // 
            // panFilters
            // 
            this.panFilters.Controls.Add(this.gbSourceElementFilter);
            this.panFilters.Controls.Add(this.gbFlowIDFilter);
            this.panFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panFilters.Location = new System.Drawing.Point(0, 55);
            this.panFilters.Name = "panFilters";
            this.panFilters.Size = new System.Drawing.Size(1739, 100);
            this.panFilters.TabIndex = 1;
            // 
            // gbSourceElementFilter
            // 
            this.gbSourceElementFilter.Controls.Add(this.btnSourceElementFilter);
            this.gbSourceElementFilter.Controls.Add(this.lblSourceElementFilter);
            this.gbSourceElementFilter.Controls.Add(this.label2);
            this.gbSourceElementFilter.Location = new System.Drawing.Point(60, 4);
            this.gbSourceElementFilter.Name = "gbSourceElementFilter";
            this.gbSourceElementFilter.Size = new System.Drawing.Size(293, 84);
            this.gbSourceElementFilter.TabIndex = 1;
            this.gbSourceElementFilter.TabStop = false;
            this.gbSourceElementFilter.Text = "SourceElementFilter";
            // 
            // btnSourceElementFilter
            // 
            this.btnSourceElementFilter.Location = new System.Drawing.Point(233, 58);
            this.btnSourceElementFilter.Name = "btnSourceElementFilter";
            this.btnSourceElementFilter.Size = new System.Drawing.Size(54, 26);
            this.btnSourceElementFilter.TabIndex = 4;
            this.btnSourceElementFilter.Text = "...";
            this.btnSourceElementFilter.UseVisualStyleBackColor = true;
            this.btnSourceElementFilter.Click += new System.EventHandler(this.btnSourceElementFilter_Click);
            // 
            // lblSourceElementFilter
            // 
            this.lblSourceElementFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSourceElementFilter.Location = new System.Drawing.Point(6, 56);
            this.lblSourceElementFilter.Name = "lblSourceElementFilter";
            this.lblSourceElementFilter.Size = new System.Drawing.Size(221, 25);
            this.lblSourceElementFilter.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(282, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Показывать только для элементов";
            // 
            // gbFlowIDFilter
            // 
            this.gbFlowIDFilter.Controls.Add(this.lblFlowIDFilter);
            this.gbFlowIDFilter.Controls.Add(this.btnSelectFlowID);
            this.gbFlowIDFilter.Controls.Add(this.label1);
            this.gbFlowIDFilter.Location = new System.Drawing.Point(748, 3);
            this.gbFlowIDFilter.Name = "gbFlowIDFilter";
            this.gbFlowIDFilter.Size = new System.Drawing.Size(393, 91);
            this.gbFlowIDFilter.TabIndex = 0;
            this.gbFlowIDFilter.TabStop = false;
            this.gbFlowIDFilter.Text = "FlowIDFilter";
            // 
            // lblFlowIDFilter
            // 
            this.lblFlowIDFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFlowIDFilter.Location = new System.Drawing.Point(10, 58);
            this.lblFlowIDFilter.Name = "lblFlowIDFilter";
            this.lblFlowIDFilter.Size = new System.Drawing.Size(303, 27);
            this.lblFlowIDFilter.TabIndex = 3;
            // 
            // btnSelectFlowID
            // 
            this.btnSelectFlowID.Location = new System.Drawing.Point(319, 57);
            this.btnSelectFlowID.Name = "btnSelectFlowID";
            this.btnSelectFlowID.Size = new System.Drawing.Size(68, 28);
            this.btnSelectFlowID.TabIndex = 2;
            this.btnSelectFlowID.Text = "...";
            this.btnSelectFlowID.UseVisualStyleBackColor = true;
            this.btnSelectFlowID.Click += new System.EventHandler(this.btnSelectFlowID_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Показывать только указанные FlowID";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbShow,
            this.toolStripSeparator1,
            this.tsbHide,
            this.tsbReloadCurrentDiagram});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1739, 55);
            this.toolStrip1.TabIndex = 1;
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
            this.tsbShow.Click += new System.EventHandler(this.tsbShow_Click);
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
            this.tsbHide.Click += new System.EventHandler(this.tsbHide_Click);
            // 
            // tsbReloadCurrentDiagram
            // 
            this.tsbReloadCurrentDiagram.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbReloadCurrentDiagram.Image = ((System.Drawing.Image)(resources.GetObject("tsbReloadCurrentDiagram.Image")));
            this.tsbReloadCurrentDiagram.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReloadCurrentDiagram.Name = "tsbReloadCurrentDiagram";
            this.tsbReloadCurrentDiagram.Size = new System.Drawing.Size(52, 52);
            this.tsbReloadCurrentDiagram.Text = "Обновить текущую диаграмму";
            this.tsbReloadCurrentDiagram.Click += new System.EventHandler(this.tsbReloadCurrentDiagram_Click);
            // 
            // FManageLinkVisibility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1739, 767);
            this.Controls.Add(this.lvConnectors);
            this.Controls.Add(this.panFilters);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FManageLinkVisibility";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FManageLinkVisibility";
            this.panFilters.ResumeLayout(false);
            this.gbSourceElementFilter.ResumeLayout(false);
            this.gbSourceElementFilter.PerformLayout();
            this.gbFlowIDFilter.ResumeLayout(false);
            this.gbFlowIDFilter.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panFilters;
        private System.Windows.Forms.ColumnHeader colhStartElement;
        private System.Windows.Forms.ColumnHeader colhEndElement;
        private System.Windows.Forms.ColumnHeader colhConnectorType;
        private System.Windows.Forms.ColumnHeader colhFlowID;
        private System.Windows.Forms.ColumnHeader colhSegmentID;
        private System.Windows.Forms.ColumnHeader colhName;
        private System.Windows.Forms.ColumnHeader colhNotes;
        public System.Windows.Forms.ListView lvConnectors;
        private System.Windows.Forms.ColumnHeader colShown;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbShow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbHide;
        private System.Windows.Forms.ToolStripButton tsbReloadCurrentDiagram;
        private System.Windows.Forms.GroupBox gbFlowIDFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectFlowID;
        private System.Windows.Forms.Label lblFlowIDFilter;
        private System.Windows.Forms.GroupBox gbSourceElementFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSourceElementFilter;
        private System.Windows.Forms.Label lblSourceElementFilter;
    }
}