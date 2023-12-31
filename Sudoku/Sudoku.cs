﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Sudoku : ICloneable
    {
        public const int EMPTYSLOT = -1;

        public int[] gameArea = new int[81];

        public Sudoku()
        {
            for(int i = 0; i < gameArea.Length; i++)
            {
                gameArea[i] = EMPTYSLOT;
            }
        }

        public int this[int x, int y]
        {
            get
            {
                return Get(x, y);
            }
            set
            {
                Set(x, y, value);
            }
        }

        public int Get(int x, int y)
        {
            return gameArea[x + y * 9];
        }

        public void Set(int x, int y, int value)
        {
            gameArea[x + y * 9] = value;
        }

        public bool isFree(int x, int y)
        {
            return Get(x, y) == EMPTYSLOT;
        }

        public bool fitsNumber(int x, int y, int number)
        {
            for (int yb = y - (y % 3); yb < 3 + y - (y % 3); yb++)
            {
                for (int xb = x - (x % 3); xb < 3 + x - (x % 3); xb++)
                {
                    if (this[xb, yb] == number) return false;
                }
            }

            for(int xr = 0; xr < 9; xr++)
            {
                if (this[xr, y] == number) return false;
            }

            for(int yc = 0; yc < 9; yc++)
            {
                if (this[x, yc] == number) return false;
            }

            return true;
        }

        public List<(int x, int y)> getFreePlaces()
        {
            List<(int, int)> freePlaces = new List<(int, int)>();
            for(int i = 0; i < gameArea.Length; i++)
            {
                if (gameArea[i] == EMPTYSLOT) freePlaces.Add((i % 9, i / 9));
            }
            return freePlaces;
        }

        public bool isValidSolved()
        {
            // Blocks:
            // 123
            // 456
            // 789

            // Init lists and arrays
            List<int>[] blocks = new List<int>[9];
            List<int>[] rows = new List<int>[9];
            List<int>[] columns = new List<int>[9];
            for(int i = 0; i < 9; i++)
            {
                blocks[i] = new List<int>();
                rows[i] = new List<int>();
                columns[i] = new List<int>();
            }

            // enumerate every place in the sudoku
            for(int y = 0; y < 9; y++)
            {
                for(int x = 0; x < 9; x++)
                {
                    // check validity
                    if (this[x, y] == EMPTYSLOT) return false;
                    if (blocks[(x / 3) + (y / 3) * 3].Contains(this[x, y])) return false; else blocks[(x / 3) + (y / 3) * 3].Add(this[x, y]);
                    if (rows[y].Contains(this[x, y])) return false; else rows[y].Add(this[x, y]);
                    if (columns[x].Contains(this[x, y])) return false; else columns[x].Add(this[x, y]);
                }
            }
            return true;
        }

        public object Clone()
        {
            Sudoku clone = new Sudoku();
            for(int i = 0; i < gameArea.Length; i++)
            {
                clone.gameArea[i] = this.gameArea[i];
            }
            return clone;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Sudoku)) return false;
            Sudoku s = (Sudoku)obj;
            for (int i = 0; i < gameArea.Length; i++)
            {
                if (s.gameArea[i] != gameArea[i]) return false;
            }
            return true;
        }
    }
}
