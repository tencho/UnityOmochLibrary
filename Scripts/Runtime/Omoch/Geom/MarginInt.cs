#nullable enable

namespace Omoch.Geom
{
    public struct MarginInt
    {
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }

        public MarginInt(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public static MarginInt All(int margin)
        {
            return new MarginInt(margin, margin, margin, margin);
        }
    }
}
