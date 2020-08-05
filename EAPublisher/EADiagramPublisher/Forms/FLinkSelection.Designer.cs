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
            this.chkSetLineSize = new System.Windows.Forms.CheckBox();
            this.chkSetColor = new System.Windows.Forms.CheckBox();
            this.chkSetLineStyle = new System.Windows.Forms.CheckBox();
            this.cbLineStyle = new System.Windows.Forms.ComboBox();
            this.pbColor = new System.Windows.Forms.PictureBox();
            this.btnSelectColor = new System.Windows.Forms.Button();
            this.nudLineSize = new System.Windows.Forms.NumericUpDown();
            this.gbSelection = new System.Windows.Forms.GroupBox();
            this.btnExpandTree = new System.Windows.Forms.Button();
            this.clbNodeGroups = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.gbOperation.SuspendLayout();
            this.gbLineAndColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineSize)).BeginInit();
            this.gbSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(1154, 713);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(143, 39);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // tvLinks
            // 
            this.tvLinks.CheckBoxes = true;
            this.tvLinks.Location = new System.Drawing.Point(12, 284);
            this.tvLinks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tvLinks.Name = "tvLinks";
            this.tvLinks.Size = new System.Drawing.Size(798, 479);
            this.tvLinks.TabIndex = 3;
            this.tvLinks.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvLinks_AfterCheck);
            // 
            // gbOperation
            // 
            this.gbOperation.Controls.Add(this.rbSetLineAndColor);
            this.gbOperation.Controls.Add(this.rbResetAll);
            this.gbOperation.Location = new System.Drawing.Point(831, 347);
            this.gbOperation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbOperation.Name = "gbOperation";
            this.gbOperation.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbOperation.Size = new System.Drawing.Size(462, 138);
            this.gbOperation.TabIndex = 4;
            this.gbOperation.TabStop = false;
            this.gbOperation.Text = "Операция";
            // 
            // rbSetLineAndColor
            // 
            this.rbSetLineAndColor.AutoSize = true;
            this.rbSetLineAndColor.Checked = true;
            this.rbSetLineAndColor.Location = new System.Drawing.Point(7, 56);
            this.rbSetLineAndColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbSetLineAndColor.Name = "rbSetLineAndColor";
            this.rbSetLineAndColor.Size = new System.Drawing.Size(258, 24);
            this.rbSetLineAndColor.TabIndex = 0;
            this.rbSetLineAndColor.TabStop = true;
            this.rbSetLineAndColor.Text = "Установить выделенный вид";
            this.rbSetLineAndColor.UseVisualStyleBackColor = true;
            // 
            // rbResetAll
            // 
            this.rbResetAll.AutoSize = true;
            this.rbResetAll.Location = new System.Drawing.Point(7, 26);
            this.rbResetAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbResetAll.Name = "rbResetAll";
            this.rbResetAll.Size = new System.Drawing.Size(233, 24);
            this.rbResetAll.TabIndex = 0;
            this.rbResetAll.TabStop = true;
            this.rbResetAll.Text = "Сбросить вид всех связей";
            this.rbResetAll.UseVisualStyleBackColor = true;
            // 
            // gbLineAndColor
            // 
            this.gbLineAndColor.Controls.Add(this.chkSetLineSize);
            this.gbLineAndColor.Controls.Add(this.chkSetColor);
            this.gbLineAndColor.Controls.Add(this.chkSetLineStyle);
            this.gbLineAndColor.Controls.Add(this.cbLineStyle);
            this.gbLineAndColor.Controls.Add(this.pbColor);
            this.gbLineAndColor.Controls.Add(this.btnSelectColor);
            this.gbLineAndColor.Controls.Add(this.nudLineSize);
            this.gbLineAndColor.Location = new System.Drawing.Point(835, 504);
            this.gbLineAndColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbLineAndColor.Name = "gbLineAndColor";
            this.gbLineAndColor.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbLineAndColor.Size = new System.Drawing.Size(458, 190);
            this.gbLineAndColor.TabIndex = 4;
            this.gbLineAndColor.TabStop = false;
            this.gbLineAndColor.Text = "Вид";
            // 
            // chkSetLineSize
            // 
            this.chkSetLineSize.AutoSize = true;
            this.chkSetLineSize.Checked = true;
            this.chkSetLineSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSetLineSize.Location = new System.Drawing.Point(6, 47);
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
            this.chkSetColor.Location = new System.Drawing.Point(6, 96);
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
            this.chkSetLineStyle.Location = new System.Drawing.Point(6, 147);
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
            this.cbLineStyle.Location = new System.Drawing.Point(242, 145);
            this.cbLineStyle.Name = "cbLineStyle";
            this.cbLineStyle.Size = new System.Drawing.Size(198, 28);
            this.cbLineStyle.TabIndex = 4;
            // 
            // pbColor
            // 
            this.pbColor.BackColor = System.Drawing.Color.Black;
            this.pbColor.Location = new System.Drawing.Point(256, 92);
            this.pbColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbColor.Name = "pbColor";
            this.pbColor.Size = new System.Drawing.Size(96, 30);
            this.pbColor.TabIndex = 3;
            this.pbColor.TabStop = false;
            // 
            // btnSelectColor
            // 
            this.btnSelectColor.Location = new System.Drawing.Point(370, 92);
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
            this.nudLineSize.Location = new System.Drawing.Point(319, 45);
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
            // gbSelection
            // 
            this.gbSelection.Controls.Add(this.btnSelect);
            this.gbSelection.Controls.Add(this.label1);
            this.gbSelection.Controls.Add(this.clbNodeGroups);
            this.gbSelection.Location = new System.Drawing.Point(12, 12);
            this.gbSelection.Name = "gbSelection";
            this.gbSelection.Size = new System.Drawing.Size(1281, 266);
            this.gbSelection.TabIndex = 5;
            this.gbSelection.TabStop = false;
            this.gbSelection.Text = "Выделение";
            // 
            // btnExpandTree
            // 
            this.btnExpandTree.Location = new System.Drawing.Point(831, 284);
            this.btnExpandTree.Name = "btnExpandTree";
            this.btnExpandTree.Size = new System.Drawing.Size(211, 35);
            this.btnExpandTree.TabIndex = 6;
            this.btnExpandTree.Text = "ExpandTree";
            this.btnExpandTree.UseVisualStyleBackColor = true;
            this.btnExpandTree.Click += new System.EventHandler(this.btnExpandTree_Click);
            // 
            // clbNodeGroups
            // 
            this.clbNodeGroups.CheckOnClick = true;
            this.clbNodeGroups.FormattingEnabled = true;
            this.clbNodeGroups.Location = new System.Drawing.Point(15, 67);
            this.clbNodeGroups.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clbNodeGroups.Name = "clbNodeGroups";
            this.clbNodeGroups.ScrollAlwaysVisible = true;
            this.clbNodeGroups.Size = new System.Drawing.Size(191, 193);
            this.clbNodeGroups.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Связанные с ПО";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(1161, 22);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(102, 238);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "Выбрать";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // FLinkSelection
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1305, 764);
            this.Controls.Add(this.btnExpandTree);
            this.Controls.Add(this.gbSelection);
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
            this.gbSelection.ResumeLayout(false);
            this.gbSelection.PerformLayout();
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
        private System.Windows.Forms.PictureBox pbColor;
        private System.Windows.Forms.Button btnSelectColor;
        private System.Windows.Forms.ComboBox cbLineStyle;
        private System.Windows.Forms.CheckBox chkSetLineStyle;
        private System.Windows.Forms.CheckBox chkSetLineSize;
        private System.Windows.Forms.CheckBox chkSetColor;
        private System.Windows.Forms.GroupBox gbSelection;
        private System.Windows.Forms.Button btnExpandTree;
        private System.Windows.Forms.CheckedListBox clbNodeGroups;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelect;
    }
}