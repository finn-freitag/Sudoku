using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public interface ISudokuImporter
    {
        Sudoku Import(byte[] bytes);
        string GetMimeType();
        string GetFileExtension();
    }
}
