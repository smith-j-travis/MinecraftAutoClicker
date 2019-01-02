using System.Drawing;
using System.Runtime.InteropServices;

namespace AutoClicker
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public Rect(Rect rectangle) : this(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
        {
        }
        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int X
        {
            get => Left;
            set => Left = value;
        }
        public int Y
        {
            get => Top;
            set => Top = value;
        }
        public int Left { get; set; }

        public int Top { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public int Height
        {
            get => Bottom - Top;
            set => Bottom = value + Top;
        }
        public int Width
        {
            get => Right - Left;
            set => Right = value + Left;
        }
        public Point Location
        {
            get => new Point(Left, Top);
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }
        public Size Size
        {
            get => new Size(Width, Height);
            set
            {
                Right = value.Width + Left;
                Bottom = value.Height + Top;
            }
        }

        public static implicit operator Rectangle(Rect rectangle)
        {
            return new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
        }
        public static implicit operator Rect(Rectangle rectangle)
        {
            return new Rect(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        }
        public static bool operator ==(Rect rectangle1, Rect rectangle2)
        {
            return rectangle1.Equals(rectangle2);
        }
        public static bool operator !=(Rect rectangle1, Rect rectangle2)
        {
            return !rectangle1.Equals(rectangle2);
        }

        public override string ToString()
        {
            return "{Left: " + Left + "; " + "Top: " + Top + "; Right: " + Right + "; Bottom: " + Bottom + "}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public bool Equals(Rect rectangle)
        {
            return rectangle.Left == Left && rectangle.Top == Top && rectangle.Right == Right && rectangle.Bottom == Bottom;
        }

        public override bool Equals(object Object)
        {
            if (Object is Rect)
            {
                return Equals((Rect)Object);
            }
            else if (Object is Rectangle)
            {
                return Equals(new Rect((Rectangle)Object));
            }

            return false;
        }
    }
}
