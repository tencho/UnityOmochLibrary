using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

#nullable enable

namespace Omoch.Geom
{
    /// <summary>
    /// 領域内に重ならないように色々なサイズの矩形をランダムに配置する
    /// </summary>
    public class UniqueTilePlacer
    {
        public int GridX { get; }
        public int GridY { get; }
        public int GridWidth { get; }
        public int GridHeight { get; }
        private readonly List<TileRect> tiles;

        public UniqueTilePlacer(int x, int y, int width, int height)
        {
            GridX = x;
            GridY = y;
            GridWidth = width;
            GridHeight = height;

            tiles = new List<TileRect>();
        }

        /// <summary>
        /// 指定位置に指定サイズのタイルを配置する。ignoreOverlap=trueで重なりを無視して配置できる。
        /// </summary>
        public bool TryAddTileAt(int x, int y, int width, int height, bool ignoreOverlap, [NotNullWhen(true)] out TileRect? output)
        {
            if (width <= 0 || height <= 0)
            {
                throw new Exception("サイズは1以上である必要があります");
            }

            if (!ignoreOverlap)
            {
                var isOverlap = false;
                foreach (TileRect tile in tiles)
                {
                    if (x > tile.X - width && x < tile.X + tile.Width && y > tile.Y - height && y < tile.Y + tile.Height)
                    {
                        isOverlap = true;
                        break;
                    }
                }
                if (isOverlap)
                {
                    output = null;
                    return false;
                }
            }

            output = new TileRect(x, y, width, height);
            tiles.Add(output);
            return true;
        }

        /// <summary>
        /// 指定サイズのタイルを他タイルに重ならないように領域内に配置する
        /// </summary>
        /// <param name="width">追加するタイルの幅</param>
        /// <param name="height">追加するタイルの高さ</param>
        /// <param name="weight">追加するタイルの位置を0<=n<1で指定(nが範囲外でも自動でclampされます)</param>
        /// <param name="output"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool TryAddTile(int width, int height, float weight, [NotNullWhen(true)] out TileRect? output)
        {
            if (width <= 0 || height <= 0)
            {
                throw new Exception("サイズは1以上である必要があります");
            }

            var points = new List<Vector2Int>();
            // 既に追加済みのタイルに重ならないグリッド座標を全て取得する
            for (int ix = GridX; ix <= GridWidth - width; ix++)
            {
                for (int iy = GridY; iy <= GridHeight - height; iy++)
                {
                    var isOverlap = false;
                    foreach (TileRect tile in tiles)
                    {
                        if (ix > tile.X - width && ix < tile.X + tile.Width && iy > tile.Y - height && iy < tile.Y + tile.Height)
                        {
                            isOverlap = true;
                            break;
                        }
                    }
                    if (!isOverlap)
                    {
                        points.Add(new Vector2Int(ix, iy));
                    }
                }
            }

            // 重ならず配置可能な場所が存在しないなら終了
            if (points.Count == 0)
            {
                output = null;
                return false;
            }

            // 重ならず配置可能な場所をランダムに取得してタイルを追加
            var pointIndex = Math.Clamp((int)(weight * points.Count), 0, points.Count - 1);
            var point = points[pointIndex];
            output = new TileRect(point.x, point.y, width, height);
            tiles.Add(output);
            return true;
        }
    }

    public class TileRect
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public TileRect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
