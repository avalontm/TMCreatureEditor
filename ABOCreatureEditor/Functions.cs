using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABOCreatureEditor
{
    public static class Functions
    {
        public static Stream ToStream(this Image image)
        {
            Stream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            stream.Position = 0;
            return stream;
        }

        public static Image ToImage(this byte[] byteArrayIn)
        {
            try
            {
                using (var ms = new MemoryStream(byteArrayIn))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
            }
        }

        public static Image ToChangeColor(Bitmap src1)
        {
            Bitmap diffBM = new Bitmap(src1.Width, src1.Height, PixelFormat.Format32bppArgb);

            for (int y = 0; y < src1.Height; y++)
            {
                for (int x = 0; x < src1.Width; x++)
                {
                    //Get Both Colours at the pixel point
                    Color col1 = src1.GetPixel(x, y);
                    if (col1.R == 255 && col1.G == 0 && col1.B == 255)
                    {
                        // Create new grayscale RGB colour
                        Color newcol = Color.Transparent;
                        diffBM.SetPixel(x, y, newcol);
                    }
                    else
                    {
                        diffBM.SetPixel(x, y, col1);
                    }

                }
            }

            return diffBM;
        }
    }
}
