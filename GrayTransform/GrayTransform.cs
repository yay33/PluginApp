using PluginInterface;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace _9PartsTransform
{
    [Version(1, 1)]
    public class GrayTransform : IPlugin
    {
        public string Name
        {
            get
            {
                return "Оттенки серого";
            }
        }

        public string Author
        {
            get
            {
                return "Me";
            }
        }

        public Bitmap Transform(Bitmap bitmap)
        {
            for (int i = 0; i < bitmap.Width; ++i)
                for (int j = 0; j < bitmap.Height; ++j)
                {
                    Color color = bitmap.GetPixel(i,j);
                    int gray = (color.B + color.R + color.G) / 3;
                    bitmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            return bitmap;
        }
    }
}
