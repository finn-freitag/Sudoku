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

        public Form1()
        {
            InitializeComponent();
            Sudoku = new Sudoku();
        }

        public void Sync()
        {
            for(int i = 0; i < 9; i++)
            {
                if (textBoxes[i].Text == "") Sudoku.gameArea[i] = Sudoku.EMPTYSLOT; else Sudoku.gameArea[i] = Convert.ToInt32(textBoxes[i].Text);
            }
        }

        public void ReverseSync()
        {
            for(int i = 0; i < 9; i++)
            {
                if (Sudoku.gameArea[i] == Sudoku.EMPTYSLOT) textBoxes[i].Text = ""; else textBoxes[i].Text = Convert.ToString(Sudoku.gameArea[i]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Validate
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
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "CSV File|*.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, new CsvExporter().Export(Sudoku));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Import Csv
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV File|*.csv";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                Sudoku = new CsvImporter().Import(File.ReadAllBytes(ofd.FileName));
                ReverseSync();
            }
        }
    }
}
