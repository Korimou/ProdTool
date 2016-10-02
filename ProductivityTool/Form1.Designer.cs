namespace ProductivityTool
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstRules = new System.Windows.Forms.ListBox();
            this.grpRules = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWindowTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbComparisonType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbActivationMethod = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProcessName = new System.Windows.Forms.TextBox();
            this.cmbSearchParents = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudDuration = new System.Windows.Forms.NumericUpDown();
            this.btnGo = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.grpRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(564, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // lstRules
            // 
            this.lstRules.FormattingEnabled = true;
            this.lstRules.Location = new System.Drawing.Point(13, 28);
            this.lstRules.Name = "lstRules";
            this.lstRules.Size = new System.Drawing.Size(176, 225);
            this.lstRules.TabIndex = 1;
            this.lstRules.SelectedIndexChanged += new System.EventHandler(this.lstRules_SelectedIndexChanged);
            // 
            // grpRules
            // 
            this.grpRules.Controls.Add(this.nudDuration);
            this.grpRules.Controls.Add(this.label7);
            this.grpRules.Controls.Add(this.cmbSearchParents);
            this.grpRules.Controls.Add(this.label6);
            this.grpRules.Controls.Add(this.label5);
            this.grpRules.Controls.Add(this.txtProcessName);
            this.grpRules.Controls.Add(this.cmbActivationMethod);
            this.grpRules.Controls.Add(this.label4);
            this.grpRules.Controls.Add(this.label3);
            this.grpRules.Controls.Add(this.cmbComparisonType);
            this.grpRules.Controls.Add(this.label2);
            this.grpRules.Controls.Add(this.txtWindowTitle);
            this.grpRules.Controls.Add(this.label1);
            this.grpRules.Controls.Add(this.txtName);
            this.grpRules.Enabled = false;
            this.grpRules.Location = new System.Drawing.Point(196, 28);
            this.grpRules.Name = "grpRules";
            this.grpRules.Size = new System.Drawing.Size(356, 201);
            this.grpRules.TabIndex = 2;
            this.grpRules.TabStop = false;
            this.grpRules.Text = "Rule Details";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(90, 16);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(260, 20);
            this.txtName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Section Name:";
            // 
            // txtWindowTitle
            // 
            this.txtWindowTitle.Location = new System.Drawing.Point(90, 42);
            this.txtWindowTitle.Name = "txtWindowTitle";
            this.txtWindowTitle.Size = new System.Drawing.Size(260, 20);
            this.txtWindowTitle.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Window Title:";
            // 
            // cmbComparisonType
            // 
            this.cmbComparisonType.FormattingEnabled = true;
            this.cmbComparisonType.Items.AddRange(new object[] {
            "Substring",
            "Exact"});
            this.cmbComparisonType.Location = new System.Drawing.Point(105, 94);
            this.cmbComparisonType.Name = "cmbComparisonType";
            this.cmbComparisonType.Size = new System.Drawing.Size(245, 21);
            this.cmbComparisonType.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Comparison Type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Rule Activation Method:";
            // 
            // cmbActivationMethod
            // 
            this.cmbActivationMethod.FormattingEnabled = true;
            this.cmbActivationMethod.Items.AddRange(new object[] {
            "Enter",
            "Activate"});
            this.cmbActivationMethod.Location = new System.Drawing.Point(134, 118);
            this.cmbActivationMethod.Name = "cmbActivationMethod";
            this.cmbActivationMethod.Size = new System.Drawing.Size(216, 21);
            this.cmbActivationMethod.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Process Name:";
            // 
            // txtProcessName
            // 
            this.txtProcessName.Location = new System.Drawing.Point(90, 68);
            this.txtProcessName.Name = "txtProcessName";
            this.txtProcessName.Size = new System.Drawing.Size(260, 20);
            this.txtProcessName.TabIndex = 8;
            // 
            // cmbSearchParents
            // 
            this.cmbSearchParents.FormattingEnabled = true;
            this.cmbSearchParents.Items.AddRange(new object[] {
            "Any",
            "Single"});
            this.cmbSearchParents.Location = new System.Drawing.Point(96, 145);
            this.cmbSearchParents.Name = "cmbSearchParents";
            this.cmbSearchParents.Size = new System.Drawing.Size(254, 21);
            this.cmbSearchParents.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Search Parents:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Timer Duration:";
            // 
            // nudDuration
            // 
            this.nudDuration.Location = new System.Drawing.Point(96, 172);
            this.nudDuration.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudDuration.Name = "nudDuration";
            this.nudDuration.Size = new System.Drawing.Size(254, 20);
            this.nudDuration.TabIndex = 13;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(471, 230);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 3;
            this.btnGo.Text = "Launch";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 264);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.grpRules);
            this.Controls.Add(this.lstRules);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Productivity Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.grpRules.ResumeLayout(false);
            this.grpRules.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ListBox lstRules;
        private System.Windows.Forms.GroupBox grpRules;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWindowTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbComparisonType;
        private System.Windows.Forms.ComboBox cmbActivationMethod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProcessName;
        private System.Windows.Forms.ComboBox cmbSearchParents;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudDuration;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnGo;
    }
}

