using System;
using System.Drawing;

namespace textured_raycast.maze.texture
{
    /// <summary>
    /// A simple, rgb (0-255) based color class, used for the textures.
    /// </summary>
    class TexColor {
        private int r;
        private int g;
        private int b;

        public int R { get => Math.Min(r, 255); set => r = value; }
        public int G { get => Math.Min(g, 255); set => g = value; }
        public int B { get => Math.Min(b, 255); set => b = value; }

        public int realR { get => r; set => r = value; }
        public int realG { get => g; set => g = value; }
        public int realB { get => b; set => b = value; }

        public TexColor(int r, int g, int b) {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public TexColor() {
            this.R = 0;
            this.G = 0;
            this.B = 0;
        }

        public TexColor(int r, int g, int b, int max) {
            setColorWithMax(r, g, b, max);
        }

        /// <summary>
        /// Sets color, based on a clamping of (0-<c>max</c>).
        /// </summary>
        public void setColorWithMax(int r, int g, int b, int max) {
            int rel = 255 / max;
            this.R = r * rel;
            this.G = g * rel;
            this.B = b * rel;
        }

        public void setColorHSV(int hue, float saturation, float value) {
            float chroma = saturation * value;
            float HuePrime = hue / 60f;
            float X = chroma * (1- Math.Abs(HuePrime % 2 - 1));
            if(0 <= HuePrime && HuePrime < 1) {
                this.R = Convert.ToInt32(chroma * 255);
                this.G = Convert.ToInt32(X      * 255);
                this.B = 0;
            }
            else if(1 <= HuePrime && HuePrime < 2) {
                this.R = Convert.ToInt32(X      * 255);
                this.G = Convert.ToInt32(chroma * 255);
                this.B = 0;
            }
            else if(2 <= HuePrime && HuePrime < 3) {
                this.R = 0;
                this.G = Convert.ToInt32(chroma * 255);
                this.B = Convert.ToInt32(X      * 255);
            }
            else if(3 <= HuePrime && HuePrime < 4) {
                this.R = 0;
                this.G = Convert.ToInt32(X      * 255);
                this.B = Convert.ToInt32(chroma * 255);
            }
            else if(4 <= HuePrime && HuePrime < 5) {
                this.R = Convert.ToInt32(X      * 255);
                this.G = 0;
                this.B = Convert.ToInt32(chroma * 255);
            }
            else if(5 <= HuePrime && HuePrime < 6) {
                this.R = Convert.ToInt32(chroma * 255);
                this.G = 0;
                this.B = Convert.ToInt32(X      * 255);
            }
        }

        /// <summary>
        /// Gets a <c>System.Drawing</c> based color object from a <c>TexColor</c>
        /// </summary>
        public Color getSysColor() {
            return Color.FromArgb(R, G, B);
        }

        public static TexColor operator *(TexColor vec1, float scalar) {
            return new TexColor(
                Math.Max(0, Convert.ToInt32(vec1.realR * scalar)),
                Math.Max(0, Convert.ToInt32(vec1.realG * scalar)),
                Math.Max(0, Convert.ToInt32(vec1.realB * scalar)));
        }

        public static TexColor operator +(TexColor col1, TexColor col2) {
            return new TexColor(
                Math.Max(col1.realR + col2.realR, 0),
                Math.Max(col1.realG + col2.realG, 0),
                Math.Max(col1.realB + col2.realB, 0));
        }

        public static TexColor unitMult(TexColor col1, TexColor col2) {
            return new TexColor(
                Convert.ToInt32(col1.R * ((float)col2.R / 255.0f)),
                Convert.ToInt32(col1.G * ((float)col2.G / 255.0f)),
                Convert.ToInt32(col1.B * ((float)col2.B / 255.0f))
            );
        }

        public static TexColor unitMultReal(TexColor col1, TexColor col2) {
            return new TexColor(
                Convert.ToInt32(col1.realR * ((float)col2.realR / 255.0f)),
                Convert.ToInt32(col1.realG * ((float)col2.realG / 255.0f)),
                Convert.ToInt32(col1.realB * ((float)col2.realB / 255.0f))
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

            return col1.R == col2.R &&
                   col1.G == col2.G &&
                   col1.B == col2.B;
        }
        public static bool operator !=(TexColor col1, TexColor col2) {
            return !(col1 == col2);
        }
    }
}
