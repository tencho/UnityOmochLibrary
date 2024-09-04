using UnityEngine;

namespace Omoch.Tools
{
    public class GameObjectTools
    {
        /// <summary>
        /// アプリ上でもエディタ上でも使えるDestroy
        /// </summary>
        public static void SafeDestroy(Object obj)
        {
            if (Application.isPlaying)
            {
                GameObject.Destroy(obj);
            }
            else
            {
                GameObject.DestroyImmediate(obj);
            }
        }
    }
}
