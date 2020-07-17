namespace EADiagramPublisher.Forms

{
    partial class FSelectHierarcyLevels
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
            this.clbHierarchyLevels = new System.Windows.Forms.CheckedListBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clbHierarchyLevels
            // 
            this.clbHierarchyLevels.CheckOnClick = true;
            this.clbHierarchyLevels.FormattingEnabled = true;
            this.clbHierarchyLevels.Location = new System.Drawing.Point(25, 24);
            this.clbHierarchyLevels.Name = "clbHierarchyLevels";
            this.clbHierarchyLevels.Size = new System.Drawing.Size(459, 340);
            this.clbHierarchyLevels.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(353, 385);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(131, 36);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // FSelectHierarcyLevels
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 450);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.clbHierarchyLevels);
            this.Name = "FSelectHierarcyLevels";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выберите отображаемые уровни иерархии компонентов";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FSelectHierarcyLevels_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        public System.Windows.Forms.CheckedListBox clbHierarchyLevels;
    }
}