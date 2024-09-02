using System;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace Omoch.Randoms
{
    /// <summary>
    /// 要素に重みをつけてランダム選択する
    /// </summary>
    public class WeightedRandomSelector<T>
    {
        private readonly List<float> weights;
        private readonly List<T> elements;

        public WeightedRandomSelector()
        {
            weights = new();
            elements = new();
        }

        /// <summary>
        /// 要素を重みをつけて追加
        /// </summary>
        /// /// <param name="weight">重みは0以上</param>
        public void Add(T element, float weight)
        {
            if (weight < 0f)
            {
                throw new ArgumentOutOfRangeException($"weightが0未満です({weight})");
            }

            elements.Add(element);
            if (weights.Count == 0)
            {
                weights.Add(weight);
            }
            else
            {
                weights.Add(weights[^1] + weight);
            }
        }

        public void Clear()
        {
            elements.Clear();
            weights.Clear();
        }

        /// <summary>
        /// 設定した重みでランダムに要素を取得する
        /// </summary>
        /// <param name="randomValue">0<=randomValue<1の乱数</param>
        public T GetRandom(float randomValue)
        {
            if (randomValue < 0f || randomValue >= 1f)
            {
                throw new Exception($"乱数が0<=n<1の範囲を超えています({randomValue})");
            }

            int count = weights.Count;
            if (count == 0)
            {
                throw new Exception("選択可能な要素が1つもありません");
            }

            if (weights[^1] == 0f)
            {
                throw new Exception("重みが全て0になっています");
            }

            float threshold = randomValue * weights[^1];
            for (var i = 0; i < count; i++)
            {
                if (weights[i] > threshold)
                {
                    return elements[i];
                }
            }

            return elements[^1];
        }
    }
}
