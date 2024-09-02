using System.Diagnostics.CodeAnalysis;
using UnityEngine;

#nullable enable

namespace Omoch.Geom
{
    public class Line2
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        public float Length { get => (Start - End).magnitude; }
        public Line2(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// 0(Start)～1(End)の間の座標を求める
        /// </summary>
        public Vector2 Lerp(float t)
        {
            return Start + (End - Start) * t;
        }
        public void Translate(float x, float y)
        {
            Start += new Vector2(x, y);
            End += new Vector2(x, y);
        }
        public void Scale(float x, float y)
        {
            Start *= new Vector2(x, y);
            End *= new Vector2(x, y);
        }
        public void Rotate(float radian, Vector2 pivot)
        {
            var cos = Mathf.Cos(radian);
            var sin = Mathf.Sin(radian);
            Start -= pivot;
            Start = new Vector2(
                Start.x * cos - Start.y * sin,
                Start.x * sin + Start.y * cos
            ) + pivot;
            End -= pivot;
            End = new Vector2(
                End.x * cos - End.y * sin,
                End.x * sin + End.y * cos
            ) + pivot;
        }

        /// <summary>
        /// 2線分の交差点を取得
        /// </summary>
        public static bool TryGetIntersection(Line2 lineAB, Line2 lineCD, [NotNullWhen(true)] out Vector2? intersection)
        {
            Vector2 pointA = lineAB.Start;
            Vector2 pointB = lineAB.End;
            Vector2 pointC = lineCD.Start;
            Vector2 pointD = lineCD.End;
            Vector2 ab = pointB - pointA;
            Vector2 cd = pointD - pointC;

            float denom = ab.x * cd.y - ab.y * cd.x;
            if (Mathf.Abs(denom) < Mathf.Epsilon)
            {
                // 2線分がほぼ平行
                intersection = null;
                return false;
            }

            Vector2 ac = pointC - pointA;
            float t = (ac.x * cd.y - ac.y * cd.x) / denom;
            float u = (ac.x * ab.y - ac.y * ab.x) / denom;
            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                // 交差している
                intersection = pointA + t * ab;
                return true;
            }
            else
            {
                // 交差していない
                intersection = null;
                return false;
            }
        }
    }
}
