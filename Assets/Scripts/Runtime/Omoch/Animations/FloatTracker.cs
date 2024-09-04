using UnityEngine;

#nullable enable

namespace Omoch.Animations
{
    /// <summary>
    /// 現在地点(Current)を目的地点(Destination)に近づけるアニメーション用
    /// `current += (destination - current) * easing` に近い処理を時間ベースで使えるようにしたもの
    /// </summary>
    public class FloatTracker
    {
        private float easing;
        private float duration;
        private float speed;
        private float current;
        private float destination;

        /// <summary>イージングの単位時間。目的地点に到達する最後のduration秒間にspeed分移動する。</summary>
        public float Duration
        {
            get => duration;
            set => duration = value;
        }

        /// <summary>イージングの単位速度。目的地点に到達する最後のduration秒間にspeed分移動する。</summary>
        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        /// <summary>イージング係数。1以上。1で等速運動になる。大きいほど重くなるので注意。</summary>
        public float Easing
        {
            get => easing;
            set => easing = value;
        }

        /// <summary>現在地点</summary>
        public float Current
        {
            get => current;
            set => current = value;
        }

        /// <summary>目的地点</summary>
        public float Destination
        {
            get => destination;
            set => destination = value;
        }

        /// <summary>現在地点が目的地点と一致しているか</summary>
        public bool Completed => current == destination;

        /// <summary>
        /// </summary>
        /// <param name="duration">イージングの単位時間。目的地点に到達する最後のduration秒間にspeed分移動する。</param>
        /// <param name="speed">イージングの単位速度。目的地点に到達する最後のduration秒間にspeed分移動する。</param>
        /// <param name="easing">イージング係数。1以上。1で等速運動になる。大きいほど重くなるので注意。</param>
        public FloatTracker(float duration, float speed, float easing)
        {
            this.duration = duration;
            this.speed = speed;
            this.easing = easing;

            current = 0f;
            destination = 0f;
        }

        /// <summary>
        /// 時間を指定秒数進める
        /// </summary>
        public void AdvanceTime(float elapsedSeconds)
        {
            if (current == destination || speed == 0 || duration == 0)
            {
                return;
            }

            float speedPerSec = speed / Mathf.Pow(duration, easing);
            float t = Mathf.Abs(current - destination) / speedPerSec;
            float g = Mathf.Pow(t, 1 / easing) - elapsedSeconds;

            if (g <= 0)
            {
                current = destination;
                return;
            }

            current = (current - destination) / t * Mathf.Pow(g, easing) + destination;
        }

        public void JumpTo(float value)
        {
            current = destination = value;
        }

        public void MoveTo(float value)
        {
            destination = value;
        }

        public void MoveFromTo(float from, float to)
        {
            current = from;
            destination = to;
        }
    }
}