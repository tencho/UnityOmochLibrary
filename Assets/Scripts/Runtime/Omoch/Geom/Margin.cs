namespace Omoch.Geom
{
    /// <summary>
    /// 余白データ(float)
    /// </summary>
    public struct Margin
    {
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }

        public Margin(float top, float right, float bottom, float left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public static Margin All(float margin)
        {
            return new Margin(margin, margin, margin, margin);
        }

        public override string ToString()
        {
            return $"Margin(Top:{Top}, Right:{Right}, Bottom:{Bottom}, Left:{Left})";
        }
    }
}
