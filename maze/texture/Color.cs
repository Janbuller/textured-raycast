using System;
using System.Drawing;

namespace textured_raycast.maze.texture
{
    class TexColor {
        public int r, g, b;

        public TexColor(int r, int g, int b) {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public TexColor(int r, int g, int b, int max) {
            setColorWithMax(r, g, b, max);
        }

        public void setColorWithMax(int r, int g, int b, int max) {
            int rel = 255 / max;
            this.r = r * rel;
            this.g = g * rel;
            this.b = b * rel;
        }

        public Color getSysColor() {
            return Color.FromArgb(r, g, b);
        }

        public static TexColor operator *(TexColor vec1, float scalar) {
            return new TexColor(
                Convert.ToInt32(vec1.r * scalar),
                Convert.ToInt32(vec1.g * scalar),
                Convert.ToInt32(vec1.b * scalar));
        }

        public static bool operator ==(TexColor col1, TexColor col2) {
            if (System.Object.ReferenceEquals(col1, null))
            {
                if (System.Object.ReferenceEquals(col2, null))
                {
                    return true;
                }

                return false;
            }
            return col1.r == col2.r &&
                   col1.g == col2.g &&
                   col1.b == col2.b;
        }
        public static bool operator !=(TexColor col1, TexColor col2) {
            return !(col1 == col2);
        }
    }
}
