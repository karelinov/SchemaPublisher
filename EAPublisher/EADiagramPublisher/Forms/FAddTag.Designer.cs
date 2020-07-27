namespace EADiagramPublisher.Forms
{
    partial class FAddTag
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
            this.cbTagName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.cbTagValue = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cbTagName
            // 
            this.cbTagName.FormattingEnabled = true;
            this.cbTagName.Location = new System.Drawing.Point(79, 12);
            this.cbTagName.Name = "cbTagName";
            this.cbTagName.Size = new System.Drawing.Size(186, 28);
            this.cbTagName.TabIndex = 0;
            this.cbTagName.SelectedIndexChanged += new System.EventHandler(this.cbTagName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(395, 63);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(124, 42);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // cbTagValue
            // 
            this.cbTagValue.FormattingEnabled = true;
            this.cbTagValue.Location = new System.Drawing.Point(271, 12);
            this.cbTagValue.Name = "cbTagValue";
            this.cbTagValue.Size = new System.Drawing.Size(252, 28);
            this.cbTagValue.TabIndex = 0;
            // 
            // FAddTag
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 115);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbTagValue);
            this.Controls.Add(this.cbTagName);
            this.Name = "FAddTag";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FAddTag";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbTagName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ComboBox cbTagValue;
    }
}