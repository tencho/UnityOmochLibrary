using UnityEngine;

#nullable enable

namespace Omoch.Animations
{
    /// <summary>
    /// FloatTrackerのQuaternion版
    /// </summary>
    public class QuaternionTracker
    {
        private float duration;
        private float speed;
        private float easing;
        private Quaternion current;
        private Quaternion destination;

        /// <summary>現在の角度</summary>
        public Quaternion Current
        {
            get => current;
            set => current = value;
        }

        /// <summary>目的の角度</summary>
        public Quaternion Destination
        {
            get => destination;
            set => destination = value;
        }

        /// <summary>イージングの単位時間。目的角度に到達する最後のduration秒間にspeed分回転する。</summary>
        public float Duration
        {
            get => duration;
            set => duration = value;
        }

        /// <summary>イージングの単位速度。目的角度に到達する最後のduration秒間にspeed分回転する。</summary>
        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        /// <summary>イージング係数。1以上。1で等速回転になる。大きいほど重くなるので注意。</summary>
        public float Easing
        {
            get => easing;
            set => easing = value;
        }

        /// <summary>
        /// 現在角度が目的角度と一致しているか
        /// </summary>
        public bool Completed => current == destination;

        /// <summary>
        /// </summary>
        /// <param name="duration">イージングの単位時間。目的角度に到達する最後のduration秒間にspeed分回転する。</param>
        /// <param name="speed">イージングの単位速度。目的角度に到達する最後のduration秒間にspeed分回転する。</param>
        /// <param name="easing">イージング係数。1以上。1で等速回転になる。大きいほど重くなるので注意。</param>
        public QuaternionTracker(float duration, float speed, float easing)
        {
            this.duration = duration;
            this.speed = speed;
            this.easing = easing;

            current = Quaternion.identity;
            destination = Quaternion.identity;
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

            float speedRatioPerSec = speedPerSec / 180;
            float angleRatio = Quaternion.Angle(current, destination) / 180;
            float t = angleRatio / speedRatioPerSec;
            float g = Mathf.Pow(t, 1 / easing) - elapsedSeconds;
            if (g <= 0)
            {
                current = destination;
                return;
            }

            float currentRatio = 1f - angleRatio;
            float destinationRatio = 1f;
            // -end(0)からend(1)間でイージングした時の移動後の割合
            float totalRatio = (currentRatio - destinationRatio) / t * Mathf.Pow(g, easing) + destinationRatio;
            // start(current)からend(1)間でイージングした時の割合に変換
            float slerpRatio = Mathf.InverseLerp(currentRatio, destinationRatio, totalRatio);
            current = Quaternion.Slerp(current, destination, slerpRatio);
        }

        public static bool Slerp(Quaternion start, Quaternion end, float easingPower, float duration, float rotateSpeed, float elapsedSeconds, out Quaternion result)
        {
            if (start == end)
            {
                result = end;
                return true;
            }

            float rotateSpeedPerSec = rotateSpeed / Mathf.Pow(duration, easingPower);

            float ratioSpeedPerSec = rotateSpeedPerSec / 180;
            float angleRatio = Quaternion.Angle(start, end) / 180;
            float t = angleRatio / ratioSpeedPerSec;
            float g = Mathf.Pow(t, 1 / easingPower) - elapsedSeconds;
            if (g <= 0)
            {
                result = end;
                return true;
            }

            float current = 1f - angleRatio;
            float destination = 1f;
            // -end(0)からend(1)間でイージングした時の移動後の割合
            float totalRatio = (current - destination) / t * Mathf.Pow(g, easingPower) + destination;
            // start(current)からend(1)間でイージングした時の割合に変換
            float slertpRatio = Mathf.InverseLerp(current, destination, totalRatio);
            result = Quaternion.Slerp(start, end, slertpRatio);
            return false;
        }
    }
}