using UnityEngine;

namespace Omoch.Noises
{
    /// <summary>
    /// 2Dパーリンノイズ生成
    /// </summary>
    public class PerlinNoise2
    {
        private readonly float offsetX;
        private readonly float offsetY;
        private readonly int octaves;
        private readonly float warpScale;

        /// <summary>
        /// </summary>
        /// <param name="octaves">ノイズの詳細度が決まるオクターブ数</param>
        /// <param name="warpScale">ドメインワープでノイズを歪ませる際のスケール値。0でドメインワープ無し</param>
        /// <param name="seed">ランダムシード</param>
        public PerlinNoise2(int octaves, float warpScale, int seed)
        {
            this.octaves = octaves;
            this.warpScale = warpScale;

            offsetX = Xorshift32((uint)seed) * 20000f - 10000f;
            offsetY = Xorshift32((uint)seed + 1) * 20000f - 10000f;
        }

        public float GetNoise(float x, float y)
        {
            var noise = 0f;
            var scale = 1f;
            var power = 1f;
            var totalPower = 0f;

            for (int i = 0; i < octaves; i++)
            {
                float warpX = 0f;
                float warpY = 0f;
                // ドメインワープでノイズを歪ませる
                if (warpScale != 0)
                {
                    warpX = Mathf.PerlinNoise((x + offsetX) * scale * warpScale, (y + offsetY) * scale * warpScale);
                    warpY = Mathf.PerlinNoise((y + offsetY) * scale * warpScale, (x + offsetX) * scale * warpScale);
                }
                noise += Mathf.PerlinNoise((x + offsetX + warpX) * scale, (y + offsetY + warpY) * scale) * power;
                totalPower += power;
                power *= 0.5f;
                scale *= 2f;
            }

            return noise / totalPower;
        }

        private float Xorshift32(uint value)
        {
            value ^= value << 13;
            value ^= value >> 17;
            value ^= value << 15;
            return (float)value / uint.MaxValue;
        }
    }
}
