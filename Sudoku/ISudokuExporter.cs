using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public interface ISudokuExporter
    {
        byte[] Export(Sudoku sudoku);
        string GetMimeType();
        string GetFileExtension();
    }
}
