using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class PngExporter : ISudokuExporter
    {
        public Brush Foreground = Brushes.Black;
        public Brush Background = Brushes.White;

        public byte[] Export(Sudoku sudoku)
        {
            Bitmap bmp = new Bitmap(500, 500);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Background, 0, 0, 500, 500);

            g.FillRectangle(Foreground, 0, 0, 5, 500);
            g.FillRectangle(Foreground, 495, 0, 5, 500);
            g.FillRectangle(Foreground, 0, 0, 500, 5);
            g.FillRectangle(Foreground, 0, 495, 500, 5);

            g.FillRectangle(Foreground, 165, 0, 5, 500);
            g.FillRectangle(Foreground, 331, 0, 5, 500);
            g.FillRectangle(Foreground, 0, 165, 500, 5);
            g.FillRectangle(Foreground, 0, 331, 500, 5);

            g.FillRectangle(Foreground, 55, 0, 2, 500);
            g.FillRectangle(Foreground, 111, 0, 2, 500);
            g.FillRectangle(Foreground, 222, 0, 2, 500);
            g.FillRectangle(Foreground, 278, 0, 2, 500);
            g.FillRectangle(Foreground, 389, 0, 2, 500);
            g.FillRectangle(Foreground, 444, 0, 2, 500);

            g.FillRectangle(Foreground, 0, 55, 500, 2);
            g.FillRectangle(Foreground, 0, 111, 500, 2);
            g.FillRectangle(Foreground, 0, 222, 500, 2);
            g.FillRectangle(Foreground, 0, 278, 500, 2);
            g.FillRectangle(Foreground, 0, 389, 500, 2);
            g.FillRectangle(Foreground, 0, 444, 500, 2);

            Font font = new Font(FontFamily.GenericSansSerif, 30, FontStyle.Bold);
            
            int offsetX = 10;
            int offsetY = 7;
            int factorX = 55;
            int factorY = 55;

            for(int y = 0; y < 9; y++)
            {
                for(int x = 0; x < 9; x++)
                {
                    if (sudoku[x, y] != Sudoku.EMPTYSLOT) g.DrawString(Convert.ToString(sudoku[x, y]), font, Foreground, offsetX + x * factorX, offsetY + y * factorY);
                }
            }

            g.Flush();
            g.Dispose();

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public string GetFileExtension()
        {
            return "png";
        }

        public string GetMimeType()
        {
            return "image/png";
        }
    }
}
