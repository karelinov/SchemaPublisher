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
            this.SuspendLayout();
            // 
            // clbLinkType
            // 
            this.clbLinkType.CheckOnClick = true;
            this.clbLinkType.FormattingEnabled = true;
            this.clbLinkType.Location = new System.Drawing.Point(13, 13);
            this.clbLinkType.Name = "clbLinkType";
            this.clbLinkType.Size = new System.Drawing.Size(441, 382);
            this.clbLinkType.TabIndex = 0;
            // 
            // cbShowOnDiagram
            // 
            this.cbShowOnDiagram.AutoSize = true;
            this.cbShowOnDiagram.Checked = true;
            this.cbShowOnDiagram.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowOnDiagram.Location = new System.Drawing.Point(13, 414);
            this.cbShowOnDiagram.Name = "cbShowOnDiagram";
            this.cbShowOnDiagram.Size = new System.Drawing.Size(337, 24);
            this.cbShowOnDiagram.TabIndex = 1;
            this.cbShowOnDiagram.Text = "Показать созданый линк на диаграмме";
            this.cbShowOnDiagram.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(654, 401);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(134, 37);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // FCreateNewLink
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbShowOnDiagram);
            this.Controls.Add(this.clbLinkType);
            this.Name = "FCreateNewLink";
            this.Text = "Выберите типо создаваемой связи";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.CheckedListBox clbLinkType;
        public System.Windows.Forms.CheckBox cbShowOnDiagram;
    }
}