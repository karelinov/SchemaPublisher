namespace EADiagramPublisher.Forms
{
    partial class FSetTags
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Итем",
            "21312312",
            "22222222222",
            "3333333333"}, -1);
            this.listView1 = new System.Windows.Forms.ListView();
            this.chTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTagValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chOnOff = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chOnOff,
            this.chTag,
            this.chTagValue});
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listView1.Location = new System.Drawing.Point(28, 32);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(765, 376);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // chTag
            // 
            this.chTag.Text = "Tag";
            this.chTag.Width = 201;
            // 
            // chTagValue
            // 
            this.chTagValue.Text = "Значение";
            this.chTagValue.Width = 232;
            // 
            // chOnOff
            // 
            this.chOnOff.Text = "Вкл";
            this.chOnOff.Width = 238;
            // 
            // FSetTags
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 580);
            this.Controls.Add(this.listView1);
            this.Name = "FSetTags";
            this.Text = "Установка тэгов элемента";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        public System.Windows.Forms.ColumnHeader chTag;
        public System.Windows.Forms.ColumnHeader chTagValue;
        private System.Windows.Forms.ColumnHeader chOnOff;
    }
}