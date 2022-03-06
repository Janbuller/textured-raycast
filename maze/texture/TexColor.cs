using System;
using System.Drawing;

namespace textured_raycast.maze.texture
{
    /// <summary>
    /// A simple, rgb (0-255) based color class, used for the textures.
    /// </summary>
    class TexColor {
        public int r, g, b;

        public TexColor(int r, int g, int b) {
            this.r = Math.Min(255, r);
            this.g = Math.Min(255, g);
            this.b = Math.Min(255, b);
        }

        public TexColor(int r, int g, int b, int max) {
            setColorWithMax(r, g, b, max);
        }

        /// <summary>
        /// Sets color, based on a clamping of (0-<c>max</c>).
        /// </summary>
        public void setColorWithMax(int r, int g, int b, int max) {
            int rel = 255 / max;
            this.r = r * rel;
            this.g = g * rel;
            this.b = b * rel;
        }

        public void setColorHSV(int hue, float saturation, float value) {
            float chroma = saturation * value;
            float HuePrime = hue / 60f;
            float X = chroma * (1- Math.Abs(HuePrime % 2 - 1));
            if(0 <= HuePrime && HuePrime < 1) {
                this.r = Convert.ToInt32(chroma * 255);
                this.g = Convert.ToInt32(X      * 255);
                this.b = 0;
            }
            else if(1 <= HuePrime && HuePrime < 2) {
                this.r = Convert.ToInt32(X      * 255);
                this.g = Convert.ToInt32(chroma * 255);
                this.b = 0;
            }
            else if(2 <= HuePrime && HuePrime < 3) {
                this.r = 0;
                this.g = Convert.ToInt32(chroma * 255);
                this.b = Convert.ToInt32(X      * 255);
            }
            else if(3 <= HuePrime && HuePrime < 4) {
                this.r = 0;
                this.g = Convert.ToInt32(X      * 255);
                this.b = Convert.ToInt32(chroma * 255);
            }
            else if(4 <= HuePrime && HuePrime < 5) {
                this.r = Convert.ToInt32(X      * 255);
                this.g = 0;
                this.b = Convert.ToInt32(chroma * 255);
            }
            else if(5 <= HuePrime && HuePrime < 6) {
                this.r = Convert.ToInt32(chroma * 255);
                this.g = 0;
                this.b = Convert.ToInt32(X      * 255);
            }
        }

        /// <summary>
        /// Gets a <c>System.Drawing</c> based color object from a <c>TexColor</c>
        /// </summary>
        public Color getSysColor() {
            return Color.FromArgb(r, g, b);
        }

        public static TexColor operator *(TexColor vec1, float scalar) {
            return new TexColor(
                Convert.ToInt32(vec1.r * scalar),
                Convert.ToInt32(vec1.g * scalar),
                Convert.ToInt32(vec1.b * scalar));
        }

        public static TexColor operator +(TexColor col1, TexColor col2) {
            return new TexColor(
                col1.r + col2.r,
                col1.g + col2.g,
                col1.b + col2.b
            );
        }

        public static TexColor unitMult(TexColor col1, TexColor col2) {
            return new TexColor(
                Convert.ToInt32(col1.r * ((float)col2.r / 255.0f)),
                Convert.ToInt32(col1.g * ((float)col2.g / 255.0f)),
                Convert.ToInt32(col1.b * ((float)col2.b / 255.0f))
            );
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
