namespace Omoch.Geom
{
    public enum ResizeMode : int
    {
        /// <summary>リサイズしない</summary>
        None = 1,
        /// <summary>アスペクト比を無視して枠と同じサイズにリサイズする</summary>
        Fit = 2,
        /// <summary>アスペクト比を保ちつつ枠の中に収まるようにリサイズする</summary>
        Contain = 3,
        /// <summary>アスペクト比を保ちつつ枠の隙間を完全に埋めるようにリサイズする</summary>
        Full = 4,
    }
}