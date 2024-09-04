using System;
using UnityEngine;

#nullable enable

namespace Omoch.Geom
{
    [Serializable]
    public struct Size2Int
    {
        [field: SerializeField] public int Width { get; set; }
        [field: SerializeField] public int Height { get; set; }

        public Size2Int(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public override bool Equals(object value) => value is Size2Int size && size == this;
        public override int GetHashCode() => unchecked((Width * 397) ^ Height);
        public override string ToString() => $"({Width},{Height})";

        public static bool operator ==(Size2Int a, Size2Int b) => a.Width == b.Width && a.Height == b.Height;
        public static bool operator !=(Size2Int a, Size2Int b) => !(a == b);
    }
}
