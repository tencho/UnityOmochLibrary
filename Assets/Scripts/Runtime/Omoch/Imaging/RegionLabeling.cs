using System;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace Omoch
{
    /// <summary>
    /// 連結成分ラベリングで繋がった領域ごとにラベル付けする
    /// </summary>
    public class RegionLabeling
    {
        private readonly int width;
        private readonly int height;
        private readonly int[,] labels;
        private LabeledRegion[] separatedRegions;

        /// <summary>
        /// 領域ごとにラベリングされたセル座標ごとのラベル値
        /// 0: 背景
        /// 1以上: 領域のラベル値
        /// </summary>
        public int[,] Labels => labels;

        /// <summary>
        /// 分割された領域ごとのピクセル座標リストなどの情報
        /// </summary>
        public LabeledRegion[] SeparatedRegions => separatedRegions;

        public RegionLabeling(int width, int height)
        {
            this.width = width;
            this.height = height;

            labels = new int[width, height];
            separatedRegions = new LabeledRegion[0];
        }

        /// <summary>
        /// 分割された領域を小さい順にソートして取得する
        /// </summary>
        /// <param name="includeBackground">背景領域を含むか</param>
        public LabeledRegion[] GetSortedRegions(bool includeBackground)
        {
            int offset = includeBackground ? 0 : 1;
            var copiedRegions = new LabeledRegion[separatedRegions.Length - offset];
            Array.Copy(separatedRegions, offset, copiedRegions, 0, copiedRegions.Length);
            Array.Sort(copiedRegions, (a, b) => a.Positions.Count.CompareTo(b.Positions.Count));
            return copiedRegions;
        }

        /// <summary>
        /// 画像内で繋がった領域ごとにラベリングする
        /// </summary>
        /// <param name="getPixel">(int x, int y)の座標に対応するピクセルの有無を返す関数</param>
        public void Execute(Func<int, int, bool> getPixel)
        {
            Dictionary<int, int> connection = new();
            connection[0] = 0;
            var labelCount = 0;
            var rawPixels = new bool[width, height];
            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = 0; iy < height; iy++)
                {
                    rawPixels[ix, iy] = getPixel(ix, iy);
                    labels[ix, iy] = 0;
                }
            }

            // 全ピクセルを捜査し各セルが左と上と繋がっているかチェックしラベルを貼りつつ接続情報を作る
            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = 0; iy < height; iy++)
                {
                    bool rawPixel = rawPixels[ix, iy];
                    if (rawPixel)
                    {
                        int left = GetLabel(ix - 1, iy);
                        int top = GetLabel(ix, iy - 1);
                        int label = (left == 0) ? top : (top == 0) ? left : Mathf.Min(left, top);
                        if (label == 0)
                        {
                            labelCount++;
                            labels[ix, iy] = labelCount;
                            connection[labelCount] = labelCount;
                        }
                        else
                        {
                            labels[ix, iy] = label;
                        }
                        if (left != 0 && top != 0 && left != top)
                        {
                            connection[Mathf.Max(left, top)] = Mathf.Min(left, top);
                        }
                    }
                }
            }

            // ラベルの接続情報をもとにラベルを貼りなおす
            Remap(connection);

            // 全ピクセルを捜査し各セルが右と下と繋がっているかチェックし接続情報を更新する
            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = 0; iy < height; iy++)
                {
                    int current = GetLabel(ix, iy);
                    if (current == 0)
                    {
                        continue;
                    }
                    int right = GetLabel(ix + 1, iy);
                    int bottom = GetLabel(ix, iy + 1);
                    if (right != 0 && current != right)
                    {
                        connection[Mathf.Max(current, right)] = Mathf.Min(current, right);
                    }
                    if (bottom != 0 && current != bottom)
                    {
                        connection[Mathf.Max(current, bottom)] = Mathf.Min(current, bottom);
                    }
                }
            }

            // ラベルの接続情報をもとにラベルを貼りなおす
            Remap(connection);

            // ラベルIndexを圧縮して再マップ
            var normalizedLabelMap = new Dictionary<int, int>();
            var normalizedCount = 0;
            foreach (int key in connection.Keys)
            {
                if (connection[key] == key)
                {
                    normalizedLabelMap[key] = normalizedCount++;
                }
            }

            separatedRegions = new LabeledRegion[normalizedCount];
            for (int i = 0; i < separatedRegions.Length; i++)
            {
                separatedRegions[i] = new LabeledRegion
                {
                    LabelIndex = i,
                    IsBackground = i == 0,
                    Positions = new List<Vector2Int>(),
                };
            }

            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = 0; iy < height; iy++)
                {
                    int label = normalizedLabelMap[labels[ix, iy]];
                    labels[ix, iy] = label;
                    separatedRegions[label].Positions.Add(new Vector2Int(ix, iy));
                }
            }
        }

        private int GetLabel(int x, int y)
        {
            return (x < 0 || x >= width) ? 0 : (y < 0 || y >= height) ? 0 : labels[x, y];
        }

        /// <summary>
        /// ラベルの接続情報をもとにラベルを貼りなおす
        /// </summary>
        private void Remap(Dictionary<int, int> connection)
        {
            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = 0; iy < height; iy++)
                {
                    int label = GetLabel(ix, iy);
                    while (label != connection[label])
                    {
                        label = connection[label];
                    }
                    labels[ix, iy] = label;
                }
            }
        }
    }

    public class LabeledRegion
    {
        public int LabelIndex;
        public bool IsBackground;
        public List<Vector2Int> Positions;

        public LabeledRegion()
        {
            Positions = new();
        }
    }
}
