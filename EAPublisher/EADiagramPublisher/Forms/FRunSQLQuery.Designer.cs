namespace EADiagramPublisher.Forms
{
    partial class FRunSQLQuery
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
            this.tcQuery = new System.Windows.Forms.TabControl();
            this.tpQuery = new System.Windows.Forms.TabPage();
            this.tbQuery = new System.Windows.Forms.TextBox();
            this.tpResults = new System.Windows.Forms.TabPage();
            this.wbResults = new System.Windows.Forms.WebBrowser();
            this.btnRun = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tcQuery.SuspendLayout();
            this.tpQuery.SuspendLayout();
            this.tpResults.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcQuery
            // 
            this.tcQuery.Controls.Add(this.tpQuery);
            this.tcQuery.Controls.Add(this.tpResults);
            this.tcQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcQuery.Location = new System.Drawing.Point(0, 74);
            this.tcQuery.Name = "tcQuery";
            this.tcQuery.SelectedIndex = 0;
            this.tcQuery.Size = new System.Drawing.Size(1108, 618);
            this.tcQuery.TabIndex = 1;
            // 
            // tpQuery
            // 
            this.tpQuery.Controls.Add(this.tbQuery);
            this.tpQuery.Location = new System.Drawing.Point(4, 25);
            this.tpQuery.Name = "tpQuery";
            this.tpQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tpQuery.Size = new System.Drawing.Size(1100, 589);
            this.tpQuery.TabIndex = 0;
            this.tpQuery.Text = "Запрос";
            this.tpQuery.UseVisualStyleBackColor = true;
            // 
            // tbQuery
            // 
            this.tbQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbQuery.Location = new System.Drawing.Point(3, 3);
            this.tbQuery.Multiline = true;
            this.tbQuery.Name = "tbQuery";
            this.tbQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbQuery.Size = new System.Drawing.Size(1094, 583);
            this.tbQuery.TabIndex = 0;
            // 
            // tpResults
            // 
            this.tpResults.Controls.Add(this.wbResults);
            this.tpResults.Location = new System.Drawing.Point(4, 25);
            this.tpResults.Name = "tpResults";
            this.tpResults.Padding = new System.Windows.Forms.Padding(3);
            this.tpResults.Size = new System.Drawing.Size(1100, 589);
            this.tpResults.TabIndex = 1;
            this.tpResults.Text = "Результаты";
            this.tpResults.UseVisualStyleBackColor = true;
            // 
            // wbResults
            // 
            this.wbResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbResults.Location = new System.Drawing.Point(3, 3);
            this.wbResults.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbResults.Name = "wbResults";
            this.wbResults.Size = new System.Drawing.Size(1094, 583);
            this.wbResults.TabIndex = 0;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(1001, 12);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(100, 40);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRun);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1108, 74);
            this.panel2.TabIndex = 3;
            // 
            // FRunSQLQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 692);
            this.Controls.Add(this.tcQuery);
            this.Controls.Add(this.panel2);
            this.Name = "FRunSQLQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FRunSQLQuery";
            this.tcQuery.ResumeLayout(false);
            this.tpQuery.ResumeLayout(false);
            this.tpQuery.PerformLayout();
            this.tpResults.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tcQuery;
        private System.Windows.Forms.TabPage tpQuery;
        private System.Windows.Forms.TabPage tpResults;
        private System.Windows.Forms.WebBrowser wbResults;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox tbQuery;
        private System.Windows.Forms.Panel panel2;
    }
}