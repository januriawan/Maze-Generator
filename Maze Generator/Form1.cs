/// Uploaded to codeproject.com
/// Ibraheem AlKialnny - Egypt
/// Nov 2011

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maze_Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // preventing out of range indexes
            this.comboBoxGenerate.SelectedIndex = 0;
            this.comboBoxSolve.SelectedIndex = 0;
        }

        private Maze maze;

        /// <summary>
        /// Gets or sets a value whether the form has a maze
        /// </summary>
        private bool hasMaze;

        /// <summary>
        /// Gets or sets a value whether the current maze has a solution
        /// </summary>
        private bool hasSolution;

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            this.hasSolution = false;
            this.labelWorking.Visible = true;
            this.buttonGenerate.Enabled = false;
            this.timer1.Enabled = true;
            this.buttonSolve.Enabled = false;
            // the bigger level, the larger maze. 
            // Since we divide on the level, it should be smaller to get bigger size
            // therefore we evaluate 100 - value
            this.backgroundWorker1.RunWorkerAsync(new object[]
            { 100 - (int)this.numericUpDownLevel.Value, false, this.comboBoxGenerate.SelectedIndex });
            this.hasMaze = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.pictureBoxDraw.Invalidate();
        }

        private void PictureBoxDraw_Paint(object sender, PaintEventArgs e)
        {
            if (this.maze != null)
            {
                this.maze.Draw(e.Graphics);
                if (this.hasSolution)
                    this.maze.DrawPath(e.Graphics);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = e.Argument as object[];

            int value = (int)args[0];
            bool solving = (bool)args[1];
            if (!solving)
            {
                this.maze.Generate(this.pictureBoxDraw.Width / value,
                    (this.pictureBoxDraw.Height - value) / value,
                    (int)args[2]);
            }
            else
            {
                this.maze.Solve((int)args[2]);
                this.hasSolution = true;
            }
            this.pictureBoxDraw.Invalidate();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.labelWorking.Visible = false;
            this.timer1.Enabled = false;
            this.buttonGenerate.Enabled = true;
            this.buttonSolve.Enabled = true;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.maze = new Maze(this.pictureBoxDraw.Width, this.pictureBoxDraw.Height);
            // Causes the Maze.Sleep to update
            this.numericUpDownSpeed_ValueChanged(sender, e);
            // re-draw the picture
            this.pictureBoxDraw.Invalidate();
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.hasMaze)
                return;

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "BMP Image |*.bmp";
                dlg.Title = "Chooce where to save maze";

                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    using (Bitmap bitmap = new Bitmap(this.pictureBoxDraw.Width, this.pictureBoxDraw.Height))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            this.maze.Draw(g);
                            if (this.hasSolution)
                                this.maze.DrawPath(g);
                            using (Font font = new System.Drawing.Font(this.Font.FontFamily, 14, FontStyle.Bold))
                                g.DrawString("Copyright (c) 2011 By Ibraheem AlKilanny", font, Brushes.Red, 12, this.pictureBoxDraw.Height - 40);
                            bitmap.Save(dlg.FileName);
                        }
                    }
                }
            }
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            using (AboutBox1 box = new AboutBox1())
            {
                box.ShowDialog(this);
            }
        }

        private void numericUpDownSpeed_ValueChanged(object sender, EventArgs e)
        {
            this.maze.Sleep = (100 - (int)this.numericUpDownSpeed.Value) * 100;
        }

        private void buttonSolve_Click(object sender, EventArgs e)
        {
            if (!this.hasMaze)
            {
                MessageBox.Show(this, "You must generate a maze first!", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            this.labelWorking.Visible = true;
            this.buttonGenerate.Enabled = false;
            this.timer1.Enabled = true;
            this.buttonSolve.Enabled = false;
            this.backgroundWorker1.RunWorkerAsync(new object[] { 0, true, this.comboBoxSolve.SelectedIndex });
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
