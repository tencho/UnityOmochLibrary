using UnityEngine;

namespace Omoch.Noises
{
    /// <summary>
    /// 1Dパーリンノイズ生成
    /// </summary>
    public class PerlinNoise1
    {
        private readonly float offsetX;
        private readonly int octaves;

        /// <summary>
        /// </summary>
        /// <param name="octaves">ノイズの詳細度が決まるオクターブ数</param>
        /// <param name="seed">ランダムシード</param>
        public PerlinNoise1(int octaves, int seed)
        {
            this.octaves = octaves;

            offsetX = Xorshift32((uint)seed) * 20000f - 10000f;
        }

        public float GetNoise(float x)
        {
            var noise = 0f;
            var scale = 1f;
            var power = 1f;
            var totalPower = 0f;

            for (int i = 0; i < octaves; i++)
            {
                noise += Mathf.PerlinNoise1D((x + offsetX) * scale) * power;
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
