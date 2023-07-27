using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class CsvExporter : ISudokuExporter
    {
        public byte[] Export(Sudoku sudoku)
        {
            string final = "";
            for(int y = 0; y < 9; y++)
            {
                for(int x = 0; x < 9; x++)
                {
                    final += Convert.ToString(sudoku[x, y]) + ",";
                }
                final += Environment.NewLine;
            }
            return Encoding.ASCII.GetBytes(final);
        }

        public string GetFileExtension()
        {
            return "csv";
        }

        public string GetMimeType()
        {
            return "text/csv";
        }
    }
}
