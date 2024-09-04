using System;
using UnityEngine;

#nullable enable

namespace Omoch.Geom
{
    public class RectTools
    {
        /// <summary>
        /// 幅と高さのみの矩形をリサイズした結果を返す
        /// </summary>
        /// <param name="originSize">リサイズ元の矩形</param>
        /// <param name="frameSize">リサイズ時に基準とする枠の矩形</param>
        /// <param name="mode">リサイズモード</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Vector2 ResizeVector2(Vector2 originSize, Vector2 frameSize, ResizeMode mode)
        {
            switch (mode)
            {
                case ResizeMode.None:
                    return originSize;
                case ResizeMode.Fit:
                    return frameSize;
                case ResizeMode.Contain:
                    {
                        var aspOrigin = originSize.x / originSize.y;
                        var aspFrame = frameSize.x / frameSize.y;
                        if (aspOrigin > aspFrame)
                        {
                            return new Vector2(frameSize.x, frameSize.x / aspOrigin);
                        }
                        else
                        {
                            return new Vector2(frameSize.y * aspOrigin, frameSize.y);
                        }
                    }
                case ResizeMode.Full:
                    {
                        var aspOrigin = originSize.x / originSize.y;
                        var aspFrame = frameSize.x / frameSize.y;
                        if (aspOrigin > aspFrame)
                        {
                            return new Vector2(frameSize.y * aspOrigin, frameSize.y);
                        }
                        else
                        {
                            return new Vector2(frameSize.x, frameSize.x / aspOrigin);
                        }
                    }
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}