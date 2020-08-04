namespace EADiagramPublisher.Forms
{
    partial class FLinkSelection
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
            this.tvLinks = new System.Windows.Forms.TreeView();
            this.gbOperation = new System.Windows.Forms.GroupBox();
            this.rbSetLineAndColor = new System.Windows.Forms.RadioButton();
            this.rbResetAll = new System.Windows.Forms.RadioButton();
            this.gbLineAndColor = new System.Windows.Forms.GroupBox();
            this.pbColor = new System.Windows.Forms.PictureBox();
            this.btnSelectColor = new System.Windows.Forms.Button();
            this.nudLineSize = new System.Windows.Forms.NumericUpDown();
            this.s = new System.Windows.Forms.Label();
            this.lbLineSize = new System.Windows.Forms.Label();
            this.gbOperation.SuspendLayout();
            this.gbLineAndColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineSize)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(1053, 612);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 31);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // tvLinks
            // 
            this.tvLinks.CheckBoxes = true;
            this.tvLinks.Location = new System.Drawing.Point(11, 130);
            this.tvLinks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tvLinks.Name = "tvLinks";
            this.tvLinks.Size = new System.Drawing.Size(860, 513);
            this.tvLinks.TabIndex = 3;
            // 
            // gbOperation
            // 
            this.gbOperation.Controls.Add(this.rbSetLineAndColor);
            this.gbOperation.Controls.Add(this.rbResetAll);
            this.gbOperation.Location = new System.Drawing.Point(877, 130);
            this.gbOperation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbOperation.Name = "gbOperation";
            this.gbOperation.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbOperation.Size = new System.Drawing.Size(303, 110);
            this.gbOperation.TabIndex = 4;
            this.gbOperation.TabStop = false;
            this.gbOperation.Text = "Операция";
            // 
            // rbSetLineAndColor
            // 
            this.rbSetLineAndColor.AutoSize = true;
            this.rbSetLineAndColor.Checked = true;
            this.rbSetLineAndColor.Location = new System.Drawing.Point(6, 45);
            this.rbSetLineAndColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbSetLineAndColor.Name = "rbSetLineAndColor";
            this.rbSetLineAndColor.Size = new System.Drawing.Size(219, 21);
            this.rbSetLineAndColor.TabIndex = 0;
            this.rbSetLineAndColor.TabStop = true;
            this.rbSetLineAndColor.Text = "Установить выделенный вид";
            this.rbSetLineAndColor.UseVisualStyleBackColor = true;
            // 
            // rbResetAll
            // 
            this.rbResetAll.AutoSize = true;
            this.rbResetAll.Location = new System.Drawing.Point(6, 21);
            this.rbResetAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbResetAll.Name = "rbResetAll";
            this.rbResetAll.Size = new System.Drawing.Size(199, 21);
            this.rbResetAll.TabIndex = 0;
            this.rbResetAll.TabStop = true;
            this.rbResetAll.Text = "Сбросить вид всех связей";
            this.rbResetAll.UseVisualStyleBackColor = true;
            // 
            // gbLineAndColor
            // 
            this.gbLineAndColor.Controls.Add(this.pbColor);
            this.gbLineAndColor.Controls.Add(this.btnSelectColor);
            this.gbLineAndColor.Controls.Add(this.nudLineSize);
            this.gbLineAndColor.Controls.Add(this.s);
            this.gbLineAndColor.Controls.Add(this.lbLineSize);
            this.gbLineAndColor.Location = new System.Drawing.Point(877, 257);
            this.gbLineAndColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbLineAndColor.Name = "gbLineAndColor";
            this.gbLineAndColor.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbLineAndColor.Size = new System.Drawing.Size(303, 110);
            this.gbLineAndColor.TabIndex = 4;
            this.gbLineAndColor.TabStop = false;
            this.gbLineAndColor.Text = "Вид";
            // 
            // pbColor
            // 
            this.pbColor.BackColor = System.Drawing.Color.Black;
            this.pbColor.Location = new System.Drawing.Point(64, 49);
            this.pbColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbColor.Name = "pbColor";
            this.pbColor.Size = new System.Drawing.Size(85, 24);
            this.pbColor.TabIndex = 3;
            this.pbColor.TabStop = false;
            // 
            // btnSelectColor
            // 
            this.btnSelectColor.Location = new System.Drawing.Point(169, 49);
            this.btnSelectColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectColor.Name = "btnSelectColor";
            this.btnSelectColor.Size = new System.Drawing.Size(62, 24);
            this.btnSelectColor.TabIndex = 2;
            this.btnSelectColor.Text = "...";
            this.btnSelectColor.UseVisualStyleBackColor = true;
            // 
            // nudLineSize
            // 
            this.nudLineSize.Location = new System.Drawing.Point(124, 19);
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
            this.nudLineSize.Size = new System.Drawing.Size(107, 22);
            this.nudLineSize.TabIndex = 1;
            this.nudLineSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // s
            // 
            this.s.AutoSize = true;
            this.s.Location = new System.Drawing.Point(6, 57);
            this.s.Name = "s";
            this.s.Size = new System.Drawing.Size(41, 17);
            this.s.TabIndex = 0;
            this.s.Text = "Цвет";
            // 
            // lbLineSize
            // 
            this.lbLineSize.AutoSize = true;
            this.lbLineSize.Location = new System.Drawing.Point(6, 21);
            this.lbLineSize.Name = "lbLineSize";
            this.lbLineSize.Size = new System.Drawing.Size(112, 17);
            this.lbLineSize.TabIndex = 0;
            this.lbLineSize.Text = "Толщина линии";
            // 
            // FLinkSelection
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 654);
            this.Controls.Add(this.gbLineAndColor);
            this.Controls.Add(this.gbOperation);
            this.Controls.Add(this.tvLinks);
            this.Controls.Add(this.btnOk);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FLinkSelection";
            this.Text = "FLinkSelection";
            this.gbOperation.ResumeLayout(false);
            this.gbOperation.PerformLayout();
            this.gbLineAndColor.ResumeLayout(false);
            this.gbLineAndColor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TreeView tvLinks;
        private System.Windows.Forms.GroupBox gbOperation;
        private System.Windows.Forms.RadioButton rbResetAll;
        private System.Windows.Forms.RadioButton rbSetLineAndColor;
        private System.Windows.Forms.GroupBox gbLineAndColor;
        private System.Windows.Forms.NumericUpDown nudLineSize;
        private System.Windows.Forms.Label lbLineSize;
        private System.Windows.Forms.Label s;
        private System.Windows.Forms.PictureBox pbColor;
        private System.Windows.Forms.Button btnSelectColor;
    }
}