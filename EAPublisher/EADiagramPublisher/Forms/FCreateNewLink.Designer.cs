namespace EADiagramPublisher.Forms
{
    partial class FCreateNewLink
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
            this.clbLinkType = new System.Windows.Forms.CheckedListBox();
            this.cbShowOnDiagram = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbFlowID = new System.Windows.Forms.ComboBox();
            this.cbSegmentID = new System.Windows.Forms.ComboBox();
            this.btnSuggestFromSource = new System.Windows.Forms.Button();
            this.btnSuggestFromDest = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSource = new System.Windows.Forms.TextBox();
            this.tbDestination = new System.Windows.Forms.TextBox();
            this.btnSwitchSourceDestination = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tbFlowName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // clbLinkType
            // 
            this.clbLinkType.CheckOnClick = true;
            this.clbLinkType.FormattingEnabled = true;
            this.clbLinkType.Location = new System.Drawing.Point(14, 98);
            this.clbLinkType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clbLinkType.Name = "clbLinkType";
            this.clbLinkType.Size = new System.Drawing.Size(352, 298);
            this.clbLinkType.TabIndex = 0;
            this.clbLinkType.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbLinkType_ItemCheck);
            // 
            // cbShowOnDiagram
            // 
            this.cbShowOnDiagram.AutoSize = true;
            this.cbShowOnDiagram.Checked = true;
            this.cbShowOnDiagram.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowOnDiagram.Location = new System.Drawing.Point(14, 414);
            this.cbShowOnDiagram.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbShowOnDiagram.Name = "cbShowOnDiagram";
            this.cbShowOnDiagram.Size = new System.Drawing.Size(337, 24);
            this.cbShowOnDiagram.TabIndex = 1;
            this.cbShowOnDiagram.Text = "Показать созданый линк на диаграмме";
            this.cbShowOnDiagram.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(768, 521);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(134, 38);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(403, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "ID потока";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(403, 288);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "ID сегмента в потоке";
            // 
            // cbFlowID
            // 
            this.cbFlowID.FormattingEnabled = true;
            this.cbFlowID.Location = new System.Drawing.Point(492, 197);
            this.cbFlowID.Name = "cbFlowID";
            this.cbFlowID.Size = new System.Drawing.Size(410, 28);
            this.cbFlowID.TabIndex = 7;
            this.cbFlowID.SelectedIndexChanged += new System.EventHandler(this.cbFlowID_SelectedIndexChanged);
            this.cbFlowID.TextUpdate += new System.EventHandler(this.cbFlowID_TextUpdate);
            // 
            // cbSegmentID
            // 
            this.cbSegmentID.FormattingEnabled = true;
            this.cbSegmentID.Location = new System.Drawing.Point(605, 280);
            this.cbSegmentID.Name = "cbSegmentID";
            this.cbSegmentID.Size = new System.Drawing.Size(297, 28);
            this.cbSegmentID.TabIndex = 7;
            // 
            // btnSuggestFromSource
            // 
            this.btnSuggestFromSource.Location = new System.Drawing.Point(390, 162);
            this.btnSuggestFromSource.Name = "btnSuggestFromSource";
            this.btnSuggestFromSource.Size = new System.Drawing.Size(162, 29);
            this.btnSuggestFromSource.TabIndex = 8;
            this.btnSuggestFromSource.Text = "SuggestFromSource";
            this.btnSuggestFromSource.UseVisualStyleBackColor = true;
            this.btnSuggestFromSource.Click += new System.EventHandler(this.btnSuggestFromSource_Click);
            // 
            // btnSuggestFromDest
            // 
            this.btnSuggestFromDest.Location = new System.Drawing.Point(558, 162);
            this.btnSuggestFromDest.Name = "btnSuggestFromDest";
            this.btnSuggestFromDest.Size = new System.Drawing.Size(162, 29);
            this.btnSuggestFromDest.TabIndex = 8;
            this.btnSuggestFromDest.Text = "SuggestFromDest";
            this.btnSuggestFromDest.UseVisualStyleBackColor = true;
            this.btnSuggestFromDest.Click += new System.EventHandler(this.btnSuggestFromDest_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Source";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Destination";
            // 
            // tbSource
            // 
            this.tbSource.Enabled = false;
            this.tbSource.Location = new System.Drawing.Point(121, 15);
            this.tbSource.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(331, 26);
            this.tbSource.TabIndex = 4;
            // 
            // tbDestination
            // 
            this.tbDestination.Enabled = false;
            this.tbDestination.Location = new System.Drawing.Point(121, 49);
            this.tbDestination.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbDestination.Name = "tbDestination";
            this.tbDestination.Size = new System.Drawing.Size(331, 26);
            this.tbDestination.TabIndex = 4;
            // 
            // btnSwitchSourceDestination
            // 
            this.btnSwitchSourceDestination.Location = new System.Drawing.Point(469, 31);
            this.btnSwitchSourceDestination.Name = "btnSwitchSourceDestination";
            this.btnSwitchSourceDestination.Size = new System.Drawing.Size(162, 29);
            this.btnSwitchSourceDestination.TabIndex = 8;
            this.btnSwitchSourceDestination.Text = "SwitchSourceDestination";
            this.btnSwitchSourceDestination.UseVisualStyleBackColor = true;
            this.btnSwitchSourceDestination.Click += new System.EventHandler(this.btnSwitchSourceDestination_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(386, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "ID потока";
            // 
            // tbFlowName
            // 
            this.tbFlowName.Enabled = false;
            this.tbFlowName.Location = new System.Drawing.Point(475, 108);
            this.tbFlowName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbFlowName.Name = "tbFlowName";
            this.tbFlowName.Size = new System.Drawing.Size(427, 26);
            this.tbFlowName.TabIndex = 4;
            // 
            // FCreateNewLink
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 580);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSuggestFromDest);
            this.Controls.Add(this.btnSwitchSourceDestination);
            this.Controls.Add(this.btnSuggestFromSource);
            this.Controls.Add(this.cbSegmentID);
            this.Controls.Add(this.cbFlowID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFlowName);
            this.Controls.Add(this.tbDestination);
            this.Controls.Add(this.tbSource);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbShowOnDiagram);
            this.Controls.Add(this.clbLinkType);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FCreateNewLink";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выберите тип создаваемой связи";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.CheckedListBox clbLinkType;
        public System.Windows.Forms.CheckBox cbShowOnDiagram;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFlowID;
        private System.Windows.Forms.ComboBox cbSegmentID;
        private System.Windows.Forms.Button btnSuggestFromSource;
        private System.Windows.Forms.Button btnSuggestFromDest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSource;
        private System.Windows.Forms.TextBox tbDestination;
        private System.Windows.Forms.Button btnSwitchSourceDestination;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbFlowName;
    }
}