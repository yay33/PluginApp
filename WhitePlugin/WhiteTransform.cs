using PluginInterface;
using System;
using System.Drawing;

namespace WhitePlugin
{
    [Version (1,0)]
    public class WhiteTransform : IPlugin
    {
        public string Name
        {
            get
            {
                return "Заливка белым";
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
                    Color color = Color.White;
                    bitmap.SetPixel(i,j, color);
                }
            return bitmap;
        }
    }
}
