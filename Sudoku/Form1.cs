using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        public Sudoku Sudoku;

        private TextBox[] textBoxes = new TextBox[81];

        public Form1()
        {
            InitializeComponent();
            InitTextBoxes();
            Sudoku = new Sudoku();
        }

        private void InitTextBoxes()
        {
            for(int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is TextBox) textBoxes[((TextBox)this.Controls[i]).TabIndex] = (TextBox)this.Controls[i];
            }
        }

        public void Sync()
        {
            for(int i = 0; i < 81; i++)
            {
                if (textBoxes[i].Text == "") Sudoku.gameArea[i] = Sudoku.EMPTYSLOT; else Sudoku.gameArea[i] = Convert.ToInt32(textBoxes[i].Text);
            }
        }

        public void ReverseSync()
        {
            for(int i = 0; i < 81; i++)
            {
                if (Sudoku.gameArea[i] == Sudoku.EMPTYSLOT) textBoxes[i].Text = ""; else textBoxes[i].Text = Convert.ToString(Sudoku.gameArea[i]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Validate
            Sync();
            if (Sudoku.isValidSolved())
            {
                MessageBox.Show("This Sudoku is solved correctly!");
            }
            else
            {
                MessageBox.Show("There is at least one mistake in the sudoku!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Export Csv
            Sync();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV File (*.csv)|*.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, new CsvExporter().Export(Sudoku));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Import Csv
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV File (*.csv)|*.csv";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                Sudoku = new CsvImporter().Import(File.ReadAllBytes(ofd.FileName));
                ReverseSync();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Solve
            Sync();
            if(new SudokuSolver().Solve(Sudoku))
            {
                ReverseSync();
            }
            else
            {
                MessageBox.Show("Unsolvable!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Clear
            for(int i = 0; i < textBoxes.Length; i++)
            {
                textBoxes[i].Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Generate
            Sudoku = new SudokuGenerator().Generate();
            ReverseSync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Export Png
            Sync();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG Image (*.png)|*.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, new PngExporter().Export(Sudoku));
            }
        }
    }
}
