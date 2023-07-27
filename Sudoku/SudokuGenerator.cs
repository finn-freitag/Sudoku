using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class SudokuGenerator : ISudokuGenerator
    {
        public Difficulty Difficulty = Difficulty.Easy;

        public Sudoku Generate()
        {
            return Generate(DateTime.Now.GetHashCode());
        }

        public Sudoku Generate(int seed)
        {
            Random r = new Random(seed);
            Sudoku basePattern = GenerateValidBasePattern(r);

            List<(int, int)> remaining = new List<(int, int)>();
            for (int x = 0; x < 9; x++) for (int y = 0; y < 9; y++) remaining.Add((x, y));

            return MakeHarder(basePattern, r, remaining);
        }

        public Sudoku MakeHarder(Sudoku sudoku, Random r, List<(int x, int y)> remaining)
        {
            int n = r.Next(remaining.Count);
            Sudoku clone = (Sudoku)sudoku.Clone();
            clone[remaining[n].x, remaining[n].y] = Sudoku.EMPTYSLOT;
            ExamineResult res = examine((Sudoku)clone.Clone());
            remaining.RemoveAt(n);
            if (res == ExamineResult.MultipleSolutions || (res == ExamineResult.OneSolutionNotLogicalInferable && Difficulty == Difficulty.Easy))
            {
                if (remaining.Count == 0) return sudoku;
                return MakeHarder(sudoku, r, remaining);
            }
            if (remaining.Count == 0) return clone;
            return MakeHarder(clone, r, remaining);
        }

        public Sudoku GenerateValidBasePattern(Random r)
        {
            Sudoku sudoku = Generate(new Sudoku(), r);
            while (!sudoku.isValidSolved()) sudoku = Generate(new Sudoku(), r);
            return sudoku;
        }

        private Sudoku Generate(Sudoku sudoku, Random r)
        {
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

            if (free.Count == 0) return sudoku;

            // try to brute force

            var possibility = possibilities[r.Next(possibilities.Count)];
            if (possibility.possibleNums.Length == 0) return Generate(new Sudoku(), r);
            sudoku[possibility.x, possibility.y] = possibility.possibleNums[r.Next(possibility.possibleNums.Length)];

            return Generate(sudoku, r);
        }

        private int[] GetPossibleNumbers(Sudoku sudoku, int x, int y)
        {
            List<int> nums = new List<int>();
            for (int i = 1; i < 10; i++)
            {
                if (sudoku.fitsNumber(x, y, i)) nums.Add(i);
            }
            return nums.ToArray();
        }

        private ExamineResult examine(Sudoku original) // Condition: Just call this function if you're sure that the sudoku is solvable!
        {
            List<(int x, int y, int[] possibleNums)> possibilities = new List<(int, int, int[])>();
            List<(int x, int y)> free = original.getFreePlaces();
            bool found = false;
            // try to logically infer
            do
            {
                possibilities.Clear();
                for (int i = 0; i < free.Count; i++)
                {
                    possibilities.Add((free[i].x, free[i].y, GetPossibleNumbers(original, free[i].x, free[i].y)));
                }
                found = false;
                for (int i = 0; i < possibilities.Count; i++)
                {
                    if (possibilities[i].possibleNums.Length == 1)
                    {
                        original[possibilities[i].x, possibilities[i].y] = possibilities[i].possibleNums[0];
                        free.RemoveAt(i);
                        possibilities.RemoveAt(i);
                        i--;
                        found = true;
                    }
                }
            } while (found);

            if (free.Count == 0) return ExamineResult.LogicalInferable;

            // try to brute force

            List<Sudoku> possibleEndings = new List<Sudoku>();

            for (int i = 0; i < possibilities[0].possibleNums.Length; i++)
            {
                Sudoku newTry = (Sudoku)original.Clone();
                newTry[possibilities[0].x, possibilities[0].y] = possibilities[0].possibleNums[i];
                ExamineResult res = examine(newTry);
                if (res == ExamineResult.MultipleSolutions) return ExamineResult.MultipleSolutions;
                for(int j = 0; j < possibleEndings.Count; j++)
                {
                    if (possibleEndings[j].Equals(newTry)) return ExamineResult.MultipleSolutions;
                }
                possibleEndings.Add(newTry);
            }

            return ExamineResult.OneSolutionNotLogicalInferable;
        }

        private enum ExamineResult
        {
            MultipleSolutions,
            LogicalInferable,
            OneSolutionNotLogicalInferable
        }
    }

    public enum Difficulty : byte
    {
        /// <summary>
        /// You can solve it, through logical inferring.
        /// </summary>
        Easy = 0,
        /// <summary>
        /// You have to try and retry and retry...
        /// </summary>
        Hard = 1
    }
}
