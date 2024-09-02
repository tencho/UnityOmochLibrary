using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace Omoch.Geom
{
    /// <summary>
    /// 複数の線分をまとめたもの
    /// </summary>
    public class Line2Group<T> where T : Line2
    {
        public List<T> Lines { get; }
        public Line2Group()
        {
            Lines = new List<T>();
        }

        /// <summary>
        /// 線分を追加
        /// </summary>
        public void Add(T line)
        {
            Lines.Add(line);
        }

        /// <summary>
        /// 全ての線分をpivotを中心に回転
        /// </summary>
        public void Rotate(float radian, Vector2 pivot)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                var line = Lines[i];
                line.Rotate(radian, pivot);
                Lines[i] = line;
            }
        }

        /// <summary>
        /// 全ての線分をスケーリング
        /// </summary>
        public void Scale(float x, float y)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                var line = Lines[i];
                line.Scale(x, y);
                Lines[i] = line;
            }
        }

        /// <summary>
        /// 全ての線分を平行移動
        /// </summary>
        public void Translate(float x, float y)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                var line = Lines[i];
                line.Translate(x, y);
                Lines[i] = line;
            }
        }

        /// <summary>
        /// 引数線分との交差点を引数線分の開始点に近い順ですべて取得。
        /// </summary>
        public Vector2[] TryGetIntersections(Line2 value)
        {
            var intersections = new List<Vector2>();

            foreach (var line in Lines)
            {
                if (Line2.TryGetIntersection(line, value, out var result))
                {
                    intersections.Add(result.Value);
                }
            }
            // 線分の開始点に近い順でソート
            intersections.Sort((a, b) =>
            {
                var valueA = (a - value.Start).sqrMagnitude;
                var valueB = (b - value.Start).sqrMagnitude;
                return valueA.CompareTo(valueB);
            });

            return intersections.ToArray();
        }
    }
}
