using System;
using System.Collections.Generic;

#nullable enable

namespace Omoch.Randoms
{
    /// <summary>
    /// System.Randomをstaticに使えるようにしたもの
    /// FIXME: スレッドセーフにする
    /// </summary>
    public class Randomizer
    {
        private static Random random = new Random();
        public static Random Random { get => random; }

        public static void Init(int seed)
        {
            random = new Random(seed);
        }

        /// <summary>
        /// [0 <= n < int.MaxValue]の範囲でランダムなintを返す
        /// </summary>
        public static int Next()
        {
            return random.Next();
        }

        /// <summary>
        /// [0 <= n < max]の範囲でランダムなintを返す
        /// </summary>
        public static int Next(int max)
        {
            return random.Next(max);
        }

        /// <summary>
        /// [min <= n < max]の範囲でランダムなintを返す
        /// </summary>
        public static int Next(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// ランダムなtrue/falseを返す
        /// </summary>
        public static bool NextBool()
        {
            return Next(2) == 0;
        }

        /// <summary>
        /// [0f <= n < 1f]の範囲でランダムなfloatを返す
        /// </summary>
        public static float NextFloat()
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        /// [0f <= n < max]の範囲でランダムなfloatを返す
        /// </summary>
        public static float NextFloat(float max)
        {
            return (float)(random.NextDouble() * max);
        }

        /// <summary>
        /// [min <= n < max]の範囲でランダムなfloatを返す
        /// </summary>
        public static float NextFloat(float min, float max)
        {
            return (float)(min + random.NextDouble() * (max - min));
        }

        public static T Pick<T>(T[] items)
        {
            return items[Next(items.Length)];
        }

        public static T Pick<T>(List<T> items)
        {
            return items[Next(items.Count)];
        }
    }
}