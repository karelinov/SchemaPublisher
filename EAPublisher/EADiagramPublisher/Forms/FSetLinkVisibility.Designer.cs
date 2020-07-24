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
            this.clbShowLinkType = new System.Windows.Forms.CheckedListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbShowNotLibConnectors = new System.Windows.Forms.CheckBox();
            this.clbHideLinkType = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbHideTempDiagramLinks = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // clbShowLinkType
            // 
            this.clbShowLinkType.CheckOnClick = true;
            this.clbShowLinkType.FormattingEnabled = true;
            this.clbShowLinkType.Location = new System.Drawing.Point(2, 60);
            this.clbShowLinkType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clbShowLinkType.Name = "clbShowLinkType";
            this.clbShowLinkType.Size = new System.Drawing.Size(275, 208);
            this.clbShowLinkType.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(516, 366);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(119, 30);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // cbShowNotLibConnectors
            // 
            this.cbShowNotLibConnectors.AutoSize = true;
            this.cbShowNotLibConnectors.Location = new System.Drawing.Point(8, 272);
            this.cbShowNotLibConnectors.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbShowNotLibConnectors.Name = "cbShowNotLibConnectors";
            this.cbShowNotLibConnectors.Size = new System.Drawing.Size(269, 21);
            this.cbShowNotLibConnectors.TabIndex = 4;
            this.cbShowNotLibConnectors.Text = "Показывать небиблиотечные линки";
            this.cbShowNotLibConnectors.UseVisualStyleBackColor = true;
            // 
            // clbHideLinkType
            // 
            this.clbHideLinkType.CheckOnClick = true;
            this.clbHideLinkType.FormattingEnabled = true;
            this.clbHideLinkType.Location = new System.Drawing.Point(305, 60);
            this.clbHideLinkType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clbHideLinkType.Name = "clbHideLinkType";
            this.clbHideLinkType.Size = new System.Drawing.Size(241, 208);
            this.clbHideLinkType.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(279, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Скрыть";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Показать";
            // 
            // cbHideTempDiagramLinks
            // 
            this.cbHideTempDiagramLinks.AutoSize = true;
            this.cbHideTempDiagramLinks.Checked = true;
            this.cbHideTempDiagramLinks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHideTempDiagramLinks.Location = new System.Drawing.Point(305, 272);
            this.cbHideTempDiagramLinks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbHideTempDiagramLinks.Name = "cbHideTempDiagramLinks";
            this.cbHideTempDiagramLinks.Size = new System.Drawing.Size(308, 21);
            this.cbHideTempDiagramLinks.TabIndex = 4;
            this.cbHideTempDiagramLinks.Text = "Скрыть временные линки чужих диаграмм";
            this.cbHideTempDiagramLinks.UseVisualStyleBackColor = true;
            // 
            // FSetLinkVisibility
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 416);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbHideTempDiagramLinks);
            this.Controls.Add(this.cbShowNotLibConnectors);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.clbHideLinkType);
            this.Controls.Add(this.clbShowLinkType);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FSetLinkVisibility";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Установка видимости (библиотечных связей)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckedListBox clbShowLinkType;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox cbShowNotLibConnectors;
        public System.Windows.Forms.CheckedListBox clbHideLinkType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbHideTempDiagramLinks;
    }
}