namespace Maze_Generator
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
            this.components = new System.ComponentModel.Container();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.numericUpDownLevel = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.labelWorking = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownSpeed = new System.Windows.Forms.NumericUpDown();
            this.pictureBoxDraw = new System.Windows.Forms.PictureBox();
            this.buttonSolve = new System.Windows.Forms.Button();
            this.comboBoxGenerate = new System.Windows.Forms.ComboBox();
            this.comboBoxSolve = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLevel)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDraw)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(267, 593);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(86, 23);
            this.buttonGenerate.TabIndex = 1;
            this.buttonGenerate.Text = "Generator";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // numericUpDownLevel
            // 
            this.numericUpDownLevel.Location = new System.Drawing.Point(110, 596);
            this.numericUpDownLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLevel.Name = "numericUpDownLevel";
            this.numericUpDownLevel.Size = new System.Drawing.Size(73, 20);
            this.numericUpDownLevel.TabIndex = 3;
            this.numericUpDownLevel.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 598);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Tingkat Kesulitan %";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // labelWorking
            // 
            this.labelWorking.AutoSize = true;
            this.labelWorking.Location = new System.Drawing.Point(191, 598);
            this.labelWorking.Name = "labelWorking";
            this.labelWorking.Size = new System.Drawing.Size(70, 13);
            this.labelWorking.TabIndex = 5;
            this.labelWorking.Text = "Memproses...";
            this.labelWorking.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveImageToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(189, 26);
            // 
            // saveImageToolStripMenuItem
            // 
            this.saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            this.saveImageToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.saveImageToolStripMenuItem.Text = "&Save maze as image...";
            this.saveImageToolStripMenuItem.Click += new System.EventHandler(this.saveImageToolStripMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1052, 593);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Tentang";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(790, 598);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Kecepatan%";
            // 
            // numericUpDownSpeed
            // 
            this.numericUpDownSpeed.Location = new System.Drawing.Point(877, 595);
            this.numericUpDownSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSpeed.Name = "numericUpDownSpeed";
            this.numericUpDownSpeed.Size = new System.Drawing.Size(73, 20);
            this.numericUpDownSpeed.TabIndex = 9;
            this.numericUpDownSpeed.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.numericUpDownSpeed.ValueChanged += new System.EventHandler(this.numericUpDownSpeed_ValueChanged);
            // 
            // pictureBoxDraw
            // 
            this.pictureBoxDraw.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBoxDraw.Location = new System.Drawing.Point(7, 12);
            this.pictureBoxDraw.Name = "pictureBoxDraw";
            this.pictureBoxDraw.Size = new System.Drawing.Size(1131, 575);
            this.pictureBoxDraw.TabIndex = 11;
            this.pictureBoxDraw.TabStop = false;
            this.pictureBoxDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBoxDraw_Paint);
            // 
            // buttonSolve
            // 
            this.buttonSolve.Location = new System.Drawing.Point(531, 593);
            this.buttonSolve.Name = "buttonSolve";
            this.buttonSolve.Size = new System.Drawing.Size(79, 23);
            this.buttonSolve.TabIndex = 12;
            this.buttonSolve.Text = "Penyelesaian";
            this.buttonSolve.UseVisualStyleBackColor = true;
            this.buttonSolve.Click += new System.EventHandler(this.buttonSolve_Click);
            // 
            // comboBoxGenerate
            // 
            this.comboBoxGenerate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGenerate.FormattingEnabled = true;
            this.comboBoxGenerate.Items.AddRange(new object[] {
            "Depth-First Search",
            "Breadth-First Search"});
            this.comboBoxGenerate.Location = new System.Drawing.Point(374, 593);
            this.comboBoxGenerate.Name = "comboBoxGenerate";
            this.comboBoxGenerate.Size = new System.Drawing.Size(135, 21);
            this.comboBoxGenerate.TabIndex = 13;
            // 
            // comboBoxSolve
            // 
            this.comboBoxSolve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSolve.FormattingEnabled = true;
            this.comboBoxSolve.Items.AddRange(new object[] {
            "DFS backtracking",
            "Breadth-first search",
            "Right-hand rule"});
            this.comboBoxSolve.Location = new System.Drawing.Point(624, 593);
            this.comboBoxSolve.Name = "comboBoxSolve";
            this.comboBoxSolve.Size = new System.Drawing.Size(135, 21);
            this.comboBoxSolve.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 628);
            this.Controls.Add(this.comboBoxSolve);
            this.Controls.Add(this.comboBoxGenerate);
            this.Controls.Add(this.pictureBoxDraw);
            this.Controls.Add(this.buttonSolve);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownSpeed);
            this.Controls.Add(this.labelWorking);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownLevel);
            this.Controls.Add(this.buttonGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Maze Generator & Solver";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLevel)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDraw)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.NumericUpDown numericUpDownLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label labelWorking;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveImageToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownSpeed;
        private System.Windows.Forms.PictureBox pictureBoxDraw;
        private System.Windows.Forms.Button buttonSolve;
        private System.Windows.Forms.ComboBox comboBoxGenerate;
        private System.Windows.Forms.ComboBox comboBoxSolve;
    }
}

