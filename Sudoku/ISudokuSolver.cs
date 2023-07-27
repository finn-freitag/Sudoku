using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public interface ISudokuSolver
    {
        bool Solve(Sudoku sudoku);
    }
}
