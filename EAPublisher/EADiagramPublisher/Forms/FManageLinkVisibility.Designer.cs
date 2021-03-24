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
            this.tcFilters = new System.Windows.Forms.TabControl();
            this.tpFlowIDFilter = new System.Windows.Forms.TabPage();
            this.btnSelectFlowID = new System.Windows.Forms.Button();
            this.lblFlowIDFilter = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tpLinkTypeFilter = new System.Windows.Forms.TabPage();
            this.lblLinkTypeFilter = new System.Windows.Forms.Label();
            this.btnLinkTypeFilter = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tpSourceElementFilter = new System.Windows.Forms.TabPage();
            this.btnSourceElementFilter = new System.Windows.Forms.Button();
            this.lblSourceElementFilter = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tpSoftwareClassificationFilter = new System.Windows.Forms.TabPage();
            this.lblSoftwareClassificationFilter1 = new System.Windows.Forms.Label();
            this.btnSoftwareClassificationFilter1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbReloadCurrentDiagram = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbShow = new System.Windows.Forms.ToolStripButton();
            this.tsbHide = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSelectConnector = new System.Windows.Forms.ToolStripButton();
            this.btnSelectConnectorObjects = new System.Windows.Forms.ToolStripButton();
            this.tsbShowStartInProject = new System.Windows.Forms.ToolStripButton();
            this.tsbShowEndInProject = new System.Windows.Forms.ToolStripButton();
            this.btnSoftwareClassificationFilter = new System.Windows.Forms.Button();
            this.panFilters.SuspendLayout();
            this.tcFilters.SuspendLayout();
            this.tpFlowIDFilter.SuspendLayout();
            this.tpLinkTypeFilter.SuspendLayout();
            this.tpSourceElementFilter.SuspendLayout();
            this.tpSoftwareClassificationFilter.SuspendLayout();
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
            this.lvConnectors.Location = new System.Drawing.Point(0, 201);
            this.lvConnectors.Name = "lvConnectors";
            this.lvConnectors.Size = new System.Drawing.Size(1739, 566);
            this.lvConnectors.TabIndex = 0;
            this.lvConnectors.UseCompatibleStateImageBehavior = false;
            this.lvConnectors.View = System.Windows.Forms.View.Details;
            this.lvConnectors.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvConnectors_ColumnClick);
            this.lvConnectors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvConnectors_MouseDoubleClick);
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
            this.panFilters.Controls.Add(this.tcFilters);
            this.panFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panFilters.Location = new System.Drawing.Point(0, 55);
            this.panFilters.Name = "panFilters";
            this.panFilters.Size = new System.Drawing.Size(1739, 146);
            this.panFilters.TabIndex = 1;
            // 
            // tcFilters
            // 
            this.tcFilters.Controls.Add(this.tpFlowIDFilter);
            this.tcFilters.Controls.Add(this.tpLinkTypeFilter);
            this.tcFilters.Controls.Add(this.tpSourceElementFilter);
            this.tcFilters.Controls.Add(this.tpSoftwareClassificationFilter);
            this.tcFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcFilters.Location = new System.Drawing.Point(0, 0);
            this.tcFilters.Name = "tcFilters";
            this.tcFilters.SelectedIndex = 0;
            this.tcFilters.Size = new System.Drawing.Size(1739, 146);
            this.tcFilters.TabIndex = 2;
            // 
            // tpFlowIDFilter
            // 
            this.tpFlowIDFilter.Controls.Add(this.btnSelectFlowID);
            this.tpFlowIDFilter.Controls.Add(this.lblFlowIDFilter);
            this.tpFlowIDFilter.Controls.Add(this.label1);
            this.tpFlowIDFilter.Location = new System.Drawing.Point(4, 29);
            this.tpFlowIDFilter.Name = "tpFlowIDFilter";
            this.tpFlowIDFilter.Padding = new System.Windows.Forms.Padding(3);
            this.tpFlowIDFilter.Size = new System.Drawing.Size(1731, 113);
            this.tpFlowIDFilter.TabIndex = 0;
            this.tpFlowIDFilter.Text = "FlowIDFilter";
            this.tpFlowIDFilter.UseVisualStyleBackColor = true;
            // 
            // btnSelectFlowID
            // 
            this.btnSelectFlowID.Location = new System.Drawing.Point(319, 40);
            this.btnSelectFlowID.Name = "btnSelectFlowID";
            this.btnSelectFlowID.Size = new System.Drawing.Size(68, 28);
            this.btnSelectFlowID.TabIndex = 2;
            this.btnSelectFlowID.Text = "...";
            this.btnSelectFlowID.UseVisualStyleBackColor = true;
            this.btnSelectFlowID.Click += new System.EventHandler(this.btnSelectFlowID_Click);
            // 
            // lblFlowIDFilter
            // 
            this.lblFlowIDFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFlowIDFilter.Location = new System.Drawing.Point(10, 43);
            this.lblFlowIDFilter.Name = "lblFlowIDFilter";
            this.lblFlowIDFilter.Size = new System.Drawing.Size(303, 27);
            this.lblFlowIDFilter.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Показывать только указанные FlowID";
            // 
            // tpLinkTypeFilter
            // 
            this.tpLinkTypeFilter.Controls.Add(this.lblLinkTypeFilter);
            this.tpLinkTypeFilter.Controls.Add(this.btnLinkTypeFilter);
            this.tpLinkTypeFilter.Controls.Add(this.label4);
            this.tpLinkTypeFilter.Location = new System.Drawing.Point(4, 29);
            this.tpLinkTypeFilter.Name = "tpLinkTypeFilter";
            this.tpLinkTypeFilter.Padding = new System.Windows.Forms.Padding(3);
            this.tpLinkTypeFilter.Size = new System.Drawing.Size(1731, 113);
            this.tpLinkTypeFilter.TabIndex = 1;
            this.tpLinkTypeFilter.Text = "LinkTypeFilter";
            this.tpLinkTypeFilter.UseVisualStyleBackColor = true;
            // 
            // lblLinkTypeFilter
            // 
            this.lblLinkTypeFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLinkTypeFilter.Location = new System.Drawing.Point(10, 53);
            this.lblLinkTypeFilter.Name = "lblLinkTypeFilter";
            this.lblLinkTypeFilter.Size = new System.Drawing.Size(221, 26);
            this.lblLinkTypeFilter.TabIndex = 5;
            // 
            // btnLinkTypeFilter
            // 
            this.btnLinkTypeFilter.Location = new System.Drawing.Point(237, 53);
            this.btnLinkTypeFilter.Name = "btnLinkTypeFilter";
            this.btnLinkTypeFilter.Size = new System.Drawing.Size(54, 26);
            this.btnLinkTypeFilter.TabIndex = 4;
            this.btnLinkTypeFilter.Text = "...";
            this.btnLinkTypeFilter.UseVisualStyleBackColor = true;
            this.btnLinkTypeFilter.Click += new System.EventHandler(this.btnLinkTypeFilter_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(260, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Показывать только типы линков";
            // 
            // tpSourceElementFilter
            // 
            this.tpSourceElementFilter.Controls.Add(this.btnSourceElementFilter);
            this.tpSourceElementFilter.Controls.Add(this.lblSourceElementFilter);
            this.tpSourceElementFilter.Controls.Add(this.label2);
            this.tpSourceElementFilter.Location = new System.Drawing.Point(4, 29);
            this.tpSourceElementFilter.Name = "tpSourceElementFilter";
            this.tpSourceElementFilter.Padding = new System.Windows.Forms.Padding(3);
            this.tpSourceElementFilter.Size = new System.Drawing.Size(1731, 113);
            this.tpSourceElementFilter.TabIndex = 2;
            this.tpSourceElementFilter.Text = "SourceElementFilter";
            this.tpSourceElementFilter.UseVisualStyleBackColor = true;
            // 
            // btnSourceElementFilter
            // 
            this.btnSourceElementFilter.Location = new System.Drawing.Point(234, 48);
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
            this.lblSourceElementFilter.Location = new System.Drawing.Point(6, 48);
            this.lblSourceElementFilter.Name = "lblSourceElementFilter";
            this.lblSourceElementFilter.Size = new System.Drawing.Size(221, 25);
            this.lblSourceElementFilter.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(282, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Показывать только для элементов";
            // 
            // tpSoftwareClassificationFilter
            // 
            this.tpSoftwareClassificationFilter.Controls.Add(this.lblSoftwareClassificationFilter1);
            this.tpSoftwareClassificationFilter.Controls.Add(this.btnSoftwareClassificationFilter1);
            this.tpSoftwareClassificationFilter.Controls.Add(this.label3);
            this.tpSoftwareClassificationFilter.Location = new System.Drawing.Point(4, 29);
            this.tpSoftwareClassificationFilter.Name = "tpSoftwareClassificationFilter";
            this.tpSoftwareClassificationFilter.Padding = new System.Windows.Forms.Padding(3);
            this.tpSoftwareClassificationFilter.Size = new System.Drawing.Size(1731, 113);
            this.tpSoftwareClassificationFilter.TabIndex = 3;
            this.tpSoftwareClassificationFilter.Text = "SoftwareClassificationFilter";
            this.tpSoftwareClassificationFilter.UseVisualStyleBackColor = true;
            // 
            // lblSoftwareClassificationFilter1
            // 
            this.lblSoftwareClassificationFilter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSoftwareClassificationFilter1.Location = new System.Drawing.Point(22, 62);
            this.lblSoftwareClassificationFilter1.Name = "lblSoftwareClassificationFilter1";
            this.lblSoftwareClassificationFilter1.Size = new System.Drawing.Size(421, 23);
            this.lblSoftwareClassificationFilter1.TabIndex = 2;
            // 
            // btnSoftwareClassificationFilter1
            // 
            this.btnSoftwareClassificationFilter1.Location = new System.Drawing.Point(449, 50);
            this.btnSoftwareClassificationFilter1.Name = "btnSoftwareClassificationFilter1";
            this.btnSoftwareClassificationFilter1.Size = new System.Drawing.Size(75, 35);
            this.btnSoftwareClassificationFilter1.TabIndex = 1;
            this.btnSoftwareClassificationFilter1.Text = "...";
            this.btnSoftwareClassificationFilter1.UseVisualStyleBackColor = true;
            this.btnSoftwareClassificationFilter1.Click += new System.EventHandler(this.btnSoftwareClassificationFilter_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(403, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Показывать только коннекторы, относящиеся к ПО";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbReloadCurrentDiagram,
            this.toolStripSeparator1,
            this.tsbShow,
            this.tsbHide,
            this.toolStripSeparator2,
            this.tsbSelectConnector,
            this.btnSelectConnectorObjects,
            this.tsbShowStartInProject,
            this.tsbShowEndInProject});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1739, 55);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 55);
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 55);
            // 
            // tsbSelectConnector
            // 
            this.tsbSelectConnector.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSelectConnector.Image = ((System.Drawing.Image)(resources.GetObject("tsbSelectConnector.Image")));
            this.tsbSelectConnector.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSelectConnector.Name = "tsbSelectConnector";
            this.tsbSelectConnector.Size = new System.Drawing.Size(52, 52);
            this.tsbSelectConnector.Text = "Выделить коннектор на диаграмме";
            this.tsbSelectConnector.Click += new System.EventHandler(this.tsbSelectConnector_Click);
            // 
            // btnSelectConnectorObjects
            // 
            this.btnSelectConnectorObjects.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelectConnectorObjects.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectConnectorObjects.Image")));
            this.btnSelectConnectorObjects.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectConnectorObjects.Name = "btnSelectConnectorObjects";
            this.btnSelectConnectorObjects.Size = new System.Drawing.Size(52, 52);
            this.btnSelectConnectorObjects.Text = "Выделить объекты коннекторов на диаграмме";
            this.btnSelectConnectorObjects.Click += new System.EventHandler(this.btnSelectConnectorObjects_Click);
            // 
            // tsbShowStartInProject
            // 
            this.tsbShowStartInProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbShowStartInProject.Image = ((System.Drawing.Image)(resources.GetObject("tsbShowStartInProject.Image")));
            this.tsbShowStartInProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowStartInProject.Name = "tsbShowStartInProject";
            this.tsbShowStartInProject.Size = new System.Drawing.Size(52, 52);
            this.tsbShowStartInProject.Text = "Выделить начальный элемент в дереве проекта";
            this.tsbShowStartInProject.Click += new System.EventHandler(this.tsbShowInProject_Click);
            // 
            // tsbShowEndInProject
            // 
            this.tsbShowEndInProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbShowEndInProject.Image = ((System.Drawing.Image)(resources.GetObject("tsbShowEndInProject.Image")));
            this.tsbShowEndInProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowEndInProject.Name = "tsbShowEndInProject";
            this.tsbShowEndInProject.Size = new System.Drawing.Size(52, 52);
            this.tsbShowEndInProject.Text = "Выделить конечный элемент в дереве проекта";
            this.tsbShowEndInProject.Click += new System.EventHandler(this.tsbShowEndInProject_Click);
            // 
            // btnSoftwareClassificationFilter
            // 
            this.btnSoftwareClassificationFilter.Location = new System.Drawing.Point(352, 59);
            this.btnSoftwareClassificationFilter.Name = "btnSoftwareClassificationFilter";
            this.btnSoftwareClassificationFilter.Size = new System.Drawing.Size(75, 24);
            this.btnSoftwareClassificationFilter.TabIndex = 1;
            this.btnSoftwareClassificationFilter.Text = "...";
            this.btnSoftwareClassificationFilter.UseVisualStyleBackColor = true;
            this.btnSoftwareClassificationFilter.Click += new System.EventHandler(this.btnSoftwareClassificationFilter_Click);
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
            this.tcFilters.ResumeLayout(false);
            this.tpFlowIDFilter.ResumeLayout(false);
            this.tpFlowIDFilter.PerformLayout();
            this.tpLinkTypeFilter.ResumeLayout(false);
            this.tpLinkTypeFilter.PerformLayout();
            this.tpSourceElementFilter.ResumeLayout(false);
            this.tpSourceElementFilter.PerformLayout();
            this.tpSoftwareClassificationFilter.ResumeLayout(false);
            this.tpSoftwareClassificationFilter.PerformLayout();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectFlowID;
        private System.Windows.Forms.Label lblFlowIDFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSourceElementFilter;
        private System.Windows.Forms.Label lblSourceElementFilter;
        private System.Windows.Forms.Button btnLinkTypeFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnSelectConnectorObjects;
        private System.Windows.Forms.ToolStripButton tsbShowStartInProject;
        private System.Windows.Forms.ToolStripButton tsbShowEndInProject;
        private System.Windows.Forms.ToolStripButton tsbSelectConnector;
        private System.Windows.Forms.TabControl tcFilters;
        private System.Windows.Forms.TabPage tpFlowIDFilter;
        private System.Windows.Forms.TabPage tpLinkTypeFilter;
        private System.Windows.Forms.TabPage tpSourceElementFilter;
        private System.Windows.Forms.TabPage tpSoftwareClassificationFilter;
        private System.Windows.Forms.Button btnSoftwareClassificationFilter;
        private System.Windows.Forms.Label lblSoftwareClassificationFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblLinkTypeFilter;
        private System.Windows.Forms.Label lblSoftwareClassificationFilter1;
        private System.Windows.Forms.Button btnSoftwareClassificationFilter1;
    }
}