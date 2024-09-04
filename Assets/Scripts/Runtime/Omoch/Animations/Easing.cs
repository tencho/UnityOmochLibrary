namespace Omoch.Animations
{
    public class Easing
    {
        public static float InOutCubic(float t)
        {
            t *= 2f;
            if (t < 1f)
            {
                return 0.5f * t * t * t;
            }
            else
            {
                t -= 2f;
                return 0.5f * (t * t * t + 2f);
            }
        }
    }
}
