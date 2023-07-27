using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class SudokuSolver : ISudokuSolver
    {
        public bool Solve(Sudoku sudoku)
        {
            Sudoku original = (Sudoku)sudoku.Clone();
            List<(int x, int y, int[] possibleNums)> possibilities = new List<(int, int, int[])>();
            List<(int x, int y)> free = sudoku.getFreePlaces();
            bool found = false;
            // try to logically infer
            do
            {
                possibilities.Clear();
                for (int i = 0; i < free.Count; i++)
                {
                    possibilities.Add((free[i].x, free[i].y, GetPossibleNumbers(sudoku, free[i].x, free[i].y)));
                }
                found = false;
                for (int i = 0; i < possibilities.Count; i++)
                {
                    if (possibilities[i].possibleNums.Length == 1)
                    {
                        sudoku[possibilities[i].x, possibilities[i].y] = possibilities[i].possibleNums[0];
                        free.RemoveAt(i);
                        possibilities.RemoveAt(i);
                        i--;
                        found = true;
                    }
                }
            } while (found);

            if (free.Count == 0) return true;

            // try to brute force

            for(int i = 0; i < possibilities[0].possibleNums.Length; i++)
            {
                Sudoku newTry = (Sudoku)sudoku.Clone();
                newTry[possibilities[0].x, possibilities[0].y] = possibilities[0].possibleNums[i];
                bool success = Solve(newTry);
                if (success)
                {
                    for(int j = 0; j < sudoku.gameArea.Length; j++)
                    {
                        sudoku.gameArea[j] = newTry.gameArea[j];
                    }
                    return true;
                }
            }

            for (int j = 0; j < sudoku.gameArea.Length; j++)
            {
                sudoku.gameArea[j] = original.gameArea[j];
            }

            return false;
        }

        private int[] GetPossibleNumbers(Sudoku sudoku, int x, int y)
        {
            List<int> nums = new List<int>();
            for(int i = 1; i < 10; i++)
            {
                if (sudoku.fitsNumber(x, y, i)) nums.Add(i);
            }
            return nums.ToArray();
        }
    }
}
