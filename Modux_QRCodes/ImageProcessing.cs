using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modux_QRCodes
{
    internal class ImageProcessing
    {
        public static bool[][] ImageToQR(Image img)
        {
            Bitmap Bmp = new Bitmap(img);
            /*
            Color colour = Bmp.GetPixel(44, 44);
            Debug.WriteLine(colour.ToArgb().ToString("X8"));
            Debug.WriteLine(colour == Color.FromArgb(0, 0, 0));
            colour = Bmp.GetPixel(43, 44);
            Debug.WriteLine(colour.ToArgb().ToString("X8"));
            Debug.WriteLine(colour == Color.FromArgb(0, 0, 0));
            colour = Bmp.GetPixel(44, 43);
            Debug.WriteLine(colour.ToArgb().ToString("X8"));
            Debug.WriteLine(colour == Color.FromArgb(0, 0, 0));
            */
            bool isSide = false;
            bool isTop = false;
            int i = 0;
            while (Bmp.GetPixel(i, i) != Color.FromArgb(0, 0, 0))
            {
                i++;
            }
            if (i == 0)
            {
                isSide = true;
                isTop = true;
            }
            else
            {
                if (Bmp.GetPixel(i - 1, i) != Color.FromArgb(0, 0, 0))
                {
                    isSide = true;
                }
                if (Bmp.GetPixel(i, i - 1) != Color.FromArgb(0, 0, 0))
                {
                    isTop = true;
                }
            }

            int j1 = i;
            int j2 = i;
            int left = 0;
            int top = 0;
            int pixelSize = 1;
            if (isSide)
            {
                while (Bmp.GetPixel(i, j1) == Color.FromArgb(0, 0, 0))
                {
                    j1--;
                    if (j1 == -1)
                    {
                        break;
                    }
                }
                j1++;
                while (Bmp.GetPixel(i, j2) == Color.FromArgb(0, 0, 0))
                {
                    j2++;
                    if (j2 == Bmp.Height)
                    {
                        break;
                    }
                }
                pixelSize = (j2 - j1) / 7;
                left = i;
                top = j1;
            }
            else if (isTop)
            {
                while (Bmp.GetPixel(j1, i) == Color.FromArgb(0, 0, 0))
                {
                    j1--;
                    if (j1 == -1)
                    {
                        break;
                    }
                }
                j1++;
                while (Bmp.GetPixel(j2, i) == Color.FromArgb(0, 0, 0))
                {
                    j2++;
                    if (j2 == Bmp.Height)
                    {
                        break;
                    }
                }
                pixelSize = (j2 - j1) / 7;
                left = j1;
                top = i;
            }

            int right = Bmp.Width - 1;
            int bottom = Bmp.Height - 1;
            while (Bmp.GetPixel(right, top) != Color.FromArgb(0, 0, 0))
            {
                //Debug.WriteLine(right);
                right--;
            }
            right++;
            while (Bmp.GetPixel(left, bottom) != Color.FromArgb(0, 0, 0))
            {
                bottom--;
            }
            bottom++;

            int xStart = left + pixelSize / 2;
            int y = top + pixelSize / 2;

            IEnumerable<bool[]> result = [];
            while (y < bottom)
            {
                int x = xStart;
                IEnumerable<bool> row = [];
                while (x < right)
                {
                    if (Bmp.GetPixel(x, y) == Color.FromArgb(0, 0, 0))
                    {
                        row = row.Append(true);
                    }
                    else
                    {
                        row = row.Append(false);
                    }
                    x += pixelSize;
                }
                result = result.Append(row.ToArray());
                y += pixelSize;
            }

            Debug.WriteLine(top);
            Debug.WriteLine(bottom);
            Debug.WriteLine(left);
            Debug.WriteLine(right);
            Debug.WriteLine(pixelSize);

            Debug.WriteLine(j2);

            return result.ToArray();
        }

        public static Bitmap ImageToBW(Bitmap bmp, int threshold)
        {
            // Source: https://stackoverflow.com/questions/5000673/what-is-the-fastest-way-to-convert-an-image-to-pure-black-and-white-in-c
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color c = bmp.GetPixel(x, y);
                    int gs = (Int32)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    if (gs > threshold)
                    {
                        bmp.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        bmp.SetPixel(x, y, Color.White);
                    }
                }

            return bmp;
        }
    }
}
