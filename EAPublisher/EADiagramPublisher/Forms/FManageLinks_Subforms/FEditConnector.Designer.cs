namespace EADiagramPublisher.Forms
{
    partial class FEditConnector
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
            this.lvTaggedValues = new System.Windows.Forms.ListView();
            this.colProperty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOk = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSupplierID = new System.Windows.Forms.TextBox();
            this.tbClientID = new System.Windows.Forms.TextBox();
            this.tbNotes = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbGUID = new System.Windows.Forms.TextBox();
            this.tbID = new System.Windows.Forms.TextBox();
            this.btnSelectClient = new System.Windows.Forms.Button();
            this.btnSelectSupplier = new System.Windows.Forms.Button();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbFlowID = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSegmentID = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lvTaggedValues
            // 
            this.lvTaggedValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colProperty,
            this.colValue});
            this.lvTaggedValues.Location = new System.Drawing.Point(21, 443);
            this.lvTaggedValues.Name = "lvTaggedValues";
            this.lvTaggedValues.Size = new System.Drawing.Size(759, 211);
            this.lvTaggedValues.TabIndex = 20;
            this.lvTaggedValues.UseCompatibleStateImageBehavior = false;
            this.lvTaggedValues.View = System.Windows.Forms.View.Details;
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 410);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "TaggedValues";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 146);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 20);
            this.label7.TabIndex = 16;
            this.label7.Text = "SupplierID";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(17, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "ClientID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 246);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 20);
            this.label8.TabIndex = 13;
            this.label8.Text = "Notes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(302, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "GUID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "ID";
            // 
            // tbSupplierID
            // 
            this.tbSupplierID.Location = new System.Drawing.Point(151, 140);
            this.tbSupplierID.Name = "tbSupplierID";
            this.tbSupplierID.Size = new System.Drawing.Size(629, 26);
            this.tbSupplierID.TabIndex = 9;
            // 
            // tbClientID
            // 
            this.tbClientID.Location = new System.Drawing.Point(151, 108);
            this.tbClientID.Name = "tbClientID";
            this.tbClientID.Size = new System.Drawing.Size(629, 26);
            this.tbClientID.TabIndex = 8;
            // 
            // tbNotes
            // 
            this.tbNotes.Location = new System.Drawing.Point(151, 240);
            this.tbNotes.Multiline = true;
            this.tbNotes.Name = "tbNotes";
            this.tbNotes.Size = new System.Drawing.Size(629, 161);
            this.tbNotes.TabIndex = 6;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(151, 44);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(629, 26);
            this.tbName.TabIndex = 5;
            // 
            // tbGUID
            // 
            this.tbGUID.BackColor = System.Drawing.SystemColors.Control;
            this.tbGUID.Enabled = false;
            this.tbGUID.Location = new System.Drawing.Point(359, 12);
            this.tbGUID.Name = "tbGUID";
            this.tbGUID.Size = new System.Drawing.Size(421, 26);
            this.tbGUID.TabIndex = 10;
            // 
            // tbID
            // 
            this.tbID.BackColor = System.Drawing.SystemColors.Control;
            this.tbID.Enabled = false;
            this.tbID.Location = new System.Drawing.Point(151, 12);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(132, 26);
            this.tbID.TabIndex = 4;
            // 
            // btnSelectClient
            // 
            this.btnSelectClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSelectClient.Location = new System.Drawing.Point(786, 110);
            this.btnSelectClient.Name = "btnSelectClient";
            this.btnSelectClient.Size = new System.Drawing.Size(75, 24);
            this.btnSelectClient.TabIndex = 21;
            this.btnSelectClient.Text = "...";
            this.btnSelectClient.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSelectClient.UseVisualStyleBackColor = true;
            this.btnSelectClient.Click += new System.EventHandler(this.btnSelectClient_Click);
            // 
            // btnSelectSupplier
            // 
            this.btnSelectSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSelectSupplier.Location = new System.Drawing.Point(786, 143);
            this.btnSelectSupplier.Name = "btnSelectSupplier";
            this.btnSelectSupplier.Size = new System.Drawing.Size(75, 24);
            this.btnSelectSupplier.TabIndex = 21;
            this.btnSelectSupplier.Text = "...";
            this.btnSelectSupplier.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSelectSupplier.UseVisualStyleBackColor = true;
            this.btnSelectSupplier.Click += new System.EventHandler(this.btnSelectSupplier_Click);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(151, 76);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(629, 28);
            this.cbType.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 178);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 20);
            this.label9.TabIndex = 14;
            this.label9.Text = "FlowID";
            // 
            // cbFlowID
            // 
            this.cbFlowID.FormattingEnabled = true;
            this.cbFlowID.Location = new System.Drawing.Point(151, 172);
            this.cbFlowID.Name = "cbFlowID";
            this.cbFlowID.Size = new System.Drawing.Size(629, 28);
            this.cbFlowID.TabIndex = 22;
            this.cbFlowID.SelectedIndexChanged += new System.EventHandler(this.cbFlowID_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 212);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 20);
            this.label10.TabIndex = 14;
            this.label10.Text = "SegmentID";
            // 
            // cbSegmentID
            // 
            this.cbSegmentID.FormattingEnabled = true;
            this.cbSegmentID.Location = new System.Drawing.Point(151, 206);
            this.cbSegmentID.Name = "cbSegmentID";
            this.cbSegmentID.Size = new System.Drawing.Size(629, 28);
            this.cbSegmentID.TabIndex = 22;
            // 
            // FEditConnector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 711);
            this.Controls.Add(this.cbSegmentID);
            this.Controls.Add(this.cbFlowID);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.btnSelectSupplier);
            this.Controls.Add(this.btnSelectClient);
            this.Controls.Add(this.lvTaggedValues);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbSupplierID);
            this.Controls.Add(this.tbClientID);
            this.Controls.Add(this.tbNotes);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.tbGUID);
            this.Controls.Add(this.tbID);
            this.Name = "FEditConnector";
            this.Text = "FEditConnector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvTaggedValues;
        private System.Windows.Forms.ColumnHeader colProperty;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSupplierID;
        private System.Windows.Forms.TextBox tbClientID;
        private System.Windows.Forms.TextBox tbNotes;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbGUID;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Button btnSelectClient;
        private System.Windows.Forms.Button btnSelectSupplier;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbFlowID;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbSegmentID;
    }
}