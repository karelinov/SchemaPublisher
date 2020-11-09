namespace EADiagramPublisher.Forms
{
    partial class FEditConnectors
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
            this.label4 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbFlowID = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSegmentID = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.colProperty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvTaggedValues = new System.Windows.Forms.ListView();
            this.colIDs = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvConnectors = new System.Windows.Forms.ListView();
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(654, 660);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(126, 37);
            this.btnOk.TabIndex = 19;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 288);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Type";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(151, 280);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(629, 28);
            this.cbType.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 320);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 20);
            this.label9.TabIndex = 14;
            this.label9.Text = "FlowID";
            // 
            // cbFlowID
            // 
            this.cbFlowID.FormattingEnabled = true;
            this.cbFlowID.Location = new System.Drawing.Point(151, 314);
            this.cbFlowID.Name = "cbFlowID";
            this.cbFlowID.Size = new System.Drawing.Size(629, 28);
            this.cbFlowID.TabIndex = 22;
            this.cbFlowID.SelectedIndexChanged += new System.EventHandler(this.cbFlowID_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 354);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 20);
            this.label10.TabIndex = 14;
            this.label10.Text = "SegmentID";
            // 
            // cbSegmentID
            // 
            this.cbSegmentID.FormattingEnabled = true;
            this.cbSegmentID.Location = new System.Drawing.Point(151, 348);
            this.cbSegmentID.Name = "cbSegmentID";
            this.cbSegmentID.Size = new System.Drawing.Size(629, 28);
            this.cbSegmentID.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 410);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "TaggedValues";
            // 
            // colProperty
            // 
            this.colProperty.Text = "Property";
            this.colProperty.Width = 232;
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            this.colValue.Width = 498;
            // 
            // lvTaggedValues
            // 
            this.lvTaggedValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIDs,
            this.colProperty,
            this.colValue});
            this.lvTaggedValues.Location = new System.Drawing.Point(21, 443);
            this.lvTaggedValues.Name = "lvTaggedValues";
            this.lvTaggedValues.Size = new System.Drawing.Size(759, 211);
            this.lvTaggedValues.TabIndex = 20;
            this.lvTaggedValues.UseCompatibleStateImageBehavior = false;
            this.lvTaggedValues.View = System.Windows.Forms.View.Details;
            // 
            // colIDs
            // 
            this.colIDs.Text = "ID";
            this.colIDs.Width = 160;
            // 
            // lvConnectors
            // 
            this.lvConnectors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colID,
            this.colName});
            this.lvConnectors.Location = new System.Drawing.Point(21, 42);
            this.lvConnectors.Name = "lvConnectors";
            this.lvConnectors.Size = new System.Drawing.Size(759, 198);
            this.lvConnectors.TabIndex = 20;
            this.lvConnectors.UseCompatibleStateImageBehavior = false;
            this.lvConnectors.View = System.Windows.Forms.View.Details;
            // 
            // colID
            // 
            this.colID.Text = "ID";
            this.colID.Width = 100;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 500;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Коннекторы";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(147, 379);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(366, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "не заполненные поля не будут перезаписсаны!";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 252);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Name";
            // 
            // cbName
            // 
            this.cbName.FormattingEnabled = true;
            this.cbName.Location = new System.Drawing.Point(151, 246);
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(629, 28);
            this.cbName.TabIndex = 22;
            this.cbName.SelectedIndexChanged += new System.EventHandler(this.cbFlowID_SelectedIndexChanged);
            // 
            // FEditConnectors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 711);
            this.Controls.Add(this.cbSegmentID);
            this.Controls.Add(this.cbName);
            this.Controls.Add(this.cbFlowID);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.lvConnectors);
            this.Controls.Add(this.lvTaggedValues);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Name = "FEditConnectors";
            this.Text = "FEditConnectors";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbFlowID;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbSegmentID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader colProperty;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.ListView lvTaggedValues;
        private System.Windows.Forms.ColumnHeader colIDs;
        private System.Windows.Forms.ListView lvConnectors;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbName;
    }
}