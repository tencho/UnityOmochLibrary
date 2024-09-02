
namespace Omoch.Tools
{
    public static class BoolExtensions
    {
        /// <summary>
        /// trueを1、falseを-1に変換する
        /// </summary>
        public static int ToSign(this bool value)
        {
            return value ? 1 : -1;
        }
    }
}
