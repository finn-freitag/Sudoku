using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class CsvImporter : ISudokuImporter
    {
        public string GetFileExtension()
        {
            return "csv";
        }

        public string GetMimeType()
        {
            return "text/csv";
        }

        public Sudoku Import(byte[] bytes)
        {
            Sudoku sudoku = new Sudoku();
            string content = Encoding.ASCII.GetString(bytes);
            string[] lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for(int l = 0; l < lines.Length; l++)
            {
                string[] numbers = lines[l].Split(new char[] { ',', ';' }, StringSplitOptions.None);
                for(int n = 0; n < numbers.Length; n++)
                {
                    if (numbers[n] == "") sudoku[l, n] = Sudoku.EMPTYSLOT; else sudoku[l, n] = Convert.ToInt32(numbers[n]);
                }
            }
            return sudoku;
        }
    }
}
