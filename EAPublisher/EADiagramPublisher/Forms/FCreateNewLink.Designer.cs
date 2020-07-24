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
            this.cbTempLink = new System.Windows.Forms.CheckBox();
            this.tbFlowID = new System.Windows.Forms.TextBox();
            this.tbTempLinkDiagramID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSegmentID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // clbLinkType
            // 
            this.clbLinkType.CheckOnClick = true;
            this.clbLinkType.FormattingEnabled = true;
            this.clbLinkType.Location = new System.Drawing.Point(12, 10);
            this.clbLinkType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clbLinkType.Name = "clbLinkType";
            this.clbLinkType.Size = new System.Drawing.Size(313, 293);
            this.clbLinkType.TabIndex = 0;
            this.clbLinkType.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbLinkType_ItemCheck);
            // 
            // cbShowOnDiagram
            // 
            this.cbShowOnDiagram.AutoSize = true;
            this.cbShowOnDiagram.Checked = true;
            this.cbShowOnDiagram.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowOnDiagram.Location = new System.Drawing.Point(12, 331);
            this.cbShowOnDiagram.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbShowOnDiagram.Name = "cbShowOnDiagram";
            this.cbShowOnDiagram.Size = new System.Drawing.Size(290, 21);
            this.cbShowOnDiagram.TabIndex = 1;
            this.cbShowOnDiagram.Text = "Показать созданый линк на диаграмме";
            this.cbShowOnDiagram.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(530, 327);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(119, 30);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // cbTempLink
            // 
            this.cbTempLink.AutoSize = true;
            this.cbTempLink.Enabled = false;
            this.cbTempLink.Location = new System.Drawing.Point(331, 161);
            this.cbTempLink.Name = "cbTempLink";
            this.cbTempLink.Size = new System.Drawing.Size(295, 21);
            this.cbTempLink.TabIndex = 3;
            this.cbTempLink.Text = "Временный линк только для диаграммы";
            this.cbTempLink.UseVisualStyleBackColor = true;
            // 
            // tbFlowID
            // 
            this.tbFlowID.Location = new System.Drawing.Point(428, 64);
            this.tbFlowID.Name = "tbFlowID";
            this.tbFlowID.Size = new System.Drawing.Size(198, 22);
            this.tbFlowID.TabIndex = 4;
            // 
            // tbTempLinkDiagramID
            // 
            this.tbTempLinkDiagramID.Enabled = false;
            this.tbTempLinkDiagramID.Location = new System.Drawing.Point(331, 188);
            this.tbTempLinkDiagramID.Name = "tbTempLinkDiagramID";
            this.tbTempLinkDiagramID.Size = new System.Drawing.Size(295, 22);
            this.tbTempLinkDiagramID.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(342, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "ID потока";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(345, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "ID сегмента в потоке";
            // 
            // tbSegmentID
            // 
            this.tbSegmentID.Location = new System.Drawing.Point(497, 104);
            this.tbSegmentID.Name = "tbSegmentID";
            this.tbSegmentID.Size = new System.Drawing.Size(129, 22);
            this.tbSegmentID.TabIndex = 4;
            // 
            // FCreateNewLink
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 368);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTempLinkDiagramID);
            this.Controls.Add(this.tbSegmentID);
            this.Controls.Add(this.tbFlowID);
            this.Controls.Add(this.cbTempLink);
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
        private System.Windows.Forms.CheckBox cbTempLink;
        private System.Windows.Forms.TextBox tbFlowID;
        private System.Windows.Forms.TextBox tbTempLinkDiagramID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSegmentID;
    }
}