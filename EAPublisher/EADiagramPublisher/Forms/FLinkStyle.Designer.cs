namespace EADiagramPublisher.Forms
{
    partial class FLinkStyle
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
            this.btnOk = new System.Windows.Forms.Button();
            this.gbLineAndColor = new System.Windows.Forms.GroupBox();
            this.chkSetLineSize = new System.Windows.Forms.CheckBox();
            this.chkSetColor = new System.Windows.Forms.CheckBox();
            this.chkSetLineStyle = new System.Windows.Forms.CheckBox();
            this.cbLineStyle = new System.Windows.Forms.ComboBox();
            this.pbColor = new System.Windows.Forms.PictureBox();
            this.btnSelectColor = new System.Windows.Forms.Button();
            this.nudLineSize = new System.Windows.Forms.NumericUpDown();
            this.chkSetLinkStyle = new System.Windows.Forms.CheckBox();
            this.gbLineAndColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineSize)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(327, 259);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(143, 39);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // gbLineAndColor
            // 
            this.gbLineAndColor.Controls.Add(this.chkSetLinkStyle);
            this.gbLineAndColor.Controls.Add(this.chkSetLineSize);
            this.gbLineAndColor.Controls.Add(this.chkSetColor);
            this.gbLineAndColor.Controls.Add(this.chkSetLineStyle);
            this.gbLineAndColor.Controls.Add(this.cbLineStyle);
            this.gbLineAndColor.Controls.Add(this.pbColor);
            this.gbLineAndColor.Controls.Add(this.btnSelectColor);
            this.gbLineAndColor.Controls.Add(this.nudLineSize);
            this.gbLineAndColor.Location = new System.Drawing.Point(12, 11);
            this.gbLineAndColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbLineAndColor.Name = "gbLineAndColor";
            this.gbLineAndColor.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbLineAndColor.Size = new System.Drawing.Size(458, 234);
            this.gbLineAndColor.TabIndex = 4;
            this.gbLineAndColor.TabStop = false;
            this.gbLineAndColor.Text = "Вид";
            // 
            // chkSetLineSize
            // 
            this.chkSetLineSize.AutoSize = true;
            this.chkSetLineSize.Checked = true;
            this.chkSetLineSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSetLineSize.Location = new System.Drawing.Point(15, 96);
            this.chkSetLineSize.Name = "chkSetLineSize";
            this.chkSetLineSize.Size = new System.Drawing.Size(245, 24);
            this.chkSetLineSize.TabIndex = 5;
            this.chkSetLineSize.Text = "Установить толщину линии";
            this.chkSetLineSize.UseVisualStyleBackColor = true;
            // 
            // chkSetColor
            // 
            this.chkSetColor.AutoSize = true;
            this.chkSetColor.Checked = true;
            this.chkSetColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSetColor.Location = new System.Drawing.Point(15, 145);
            this.chkSetColor.Name = "chkSetColor";
            this.chkSetColor.Size = new System.Drawing.Size(165, 24);
            this.chkSetColor.TabIndex = 5;
            this.chkSetColor.Text = "Установить цвет";
            this.chkSetColor.UseVisualStyleBackColor = true;
            // 
            // chkSetLineStyle
            // 
            this.chkSetLineStyle.AutoSize = true;
            this.chkSetLineStyle.Checked = true;
            this.chkSetLineStyle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSetLineStyle.Location = new System.Drawing.Point(15, 196);
            this.chkSetLineStyle.Name = "chkSetLineStyle";
            this.chkSetLineStyle.Size = new System.Drawing.Size(224, 24);
            this.chkSetLineStyle.TabIndex = 5;
            this.chkSetLineStyle.Text = "Установить стиль линии";
            this.chkSetLineStyle.UseVisualStyleBackColor = true;
            // 
            // cbLineStyle
            // 
            this.cbLineStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLineStyle.FormattingEnabled = true;
            this.cbLineStyle.Items.AddRange(new object[] {
            "Orthogonal Rounded",
            "Direct"});
            this.cbLineStyle.Location = new System.Drawing.Point(251, 194);
            this.cbLineStyle.Name = "cbLineStyle";
            this.cbLineStyle.Size = new System.Drawing.Size(198, 28);
            this.cbLineStyle.TabIndex = 4;
            // 
            // pbColor
            // 
            this.pbColor.BackColor = System.Drawing.Color.Black;
            this.pbColor.Location = new System.Drawing.Point(265, 141);
            this.pbColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbColor.Name = "pbColor";
            this.pbColor.Size = new System.Drawing.Size(96, 30);
            this.pbColor.TabIndex = 3;
            this.pbColor.TabStop = false;
            // 
            // btnSelectColor
            // 
            this.btnSelectColor.Location = new System.Drawing.Point(379, 141);
            this.btnSelectColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectColor.Name = "btnSelectColor";
            this.btnSelectColor.Size = new System.Drawing.Size(70, 30);
            this.btnSelectColor.TabIndex = 2;
            this.btnSelectColor.Text = "...";
            this.btnSelectColor.UseVisualStyleBackColor = true;
            this.btnSelectColor.Click += new System.EventHandler(this.btnSelectColor_Click);
            // 
            // nudLineSize
            // 
            this.nudLineSize.Location = new System.Drawing.Point(328, 94);
            this.nudLineSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nudLineSize.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudLineSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLineSize.Name = "nudLineSize";
            this.nudLineSize.Size = new System.Drawing.Size(120, 26);
            this.nudLineSize.TabIndex = 1;
            this.nudLineSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkSetLinkStyle
            // 
            this.chkSetLinkStyle.AutoSize = true;
            this.chkSetLinkStyle.Location = new System.Drawing.Point(15, 40);
            this.chkSetLinkStyle.Name = "chkSetLinkStyle";
            this.chkSetLinkStyle.Size = new System.Drawing.Size(230, 24);
            this.chkSetLinkStyle.TabIndex = 5;
            this.chkSetLinkStyle.Text = "Установить стиль показа";
            this.chkSetLinkStyle.UseVisualStyleBackColor = true;
            this.chkSetLinkStyle.CheckedChanged += new System.EventHandler(this.cbSetShowStyle_CheckedChanged);
            // 
            // FLinkStyle
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 317);
            this.Controls.Add(this.gbLineAndColor);
            this.Controls.Add(this.btnOk);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FLinkStyle";
            this.Text = "FLinkStyle";
            this.gbLineAndColor.ResumeLayout(false);
            this.gbLineAndColor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox gbLineAndColor;
        private System.Windows.Forms.NumericUpDown nudLineSize;
        private System.Windows.Forms.PictureBox pbColor;
        private System.Windows.Forms.Button btnSelectColor;
        private System.Windows.Forms.ComboBox cbLineStyle;
        private System.Windows.Forms.CheckBox chkSetLineStyle;
        private System.Windows.Forms.CheckBox chkSetLineSize;
        private System.Windows.Forms.CheckBox chkSetColor;
        private System.Windows.Forms.CheckBox chkSetLinkStyle;
    }
}