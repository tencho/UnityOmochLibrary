using System;
using Omoch.Geom;
using UnityEngine;
using UnityEngine.Events;

namespace Omoch.Display
{
    public class ScreenResizeDetector : MonoBehaviour
    {
        [SerializeField] private float frameWidth;
        [SerializeField] private float frameHeight;

        private Vector2Int previousScreenSize;

        public UnityEvent<ScreenInfo> OnResize;

        private ScreenInfo? screenInfo;
        public ScreenInfo ScreenInfo => screenInfo ?? throw new ArgumentNullException();

        private void Awake()
        {
            previousScreenSize = new Vector2Int(-1, -1);
            OnResize = new();
            screenInfo = null;
            Update();
        }

        private void Update()
        {
            if (Screen.width == previousScreenSize.x && Screen.height == previousScreenSize.y)
            {
                return;
            }

            previousScreenSize.Set(Screen.width, Screen.height);

            var screenSize = new Vector2(Screen.width, Screen.height);
            var displaySize = RectTools.ResizeVector2(screenSize, new Vector2(frameWidth, frameHeight), ResizeMode.Full);
            var scale = displaySize.x / screenSize.x;
            var unsafePadding = new Margin(
                (Screen.height - Screen.safeArea.yMax) * scale,
                (Screen.width - Screen.safeArea.xMax) * scale,
                Screen.safeArea.yMin * scale,
                Screen.safeArea.xMin * scale
            );

            screenInfo = new ScreenInfo
            {
                DisplaySize = displaySize,
                UnsafePadding = unsafePadding,
            };
            OnResize.Invoke(screenInfo.Value);
        }
    }

    public struct ScreenInfo
    {
        /// <summary>
        /// 画面サイズ
        /// </summary>
        public Vector2 DisplaySize;
        /// <summary>
        /// 非セーフエリアの画面端からの余白
        /// </summary>
        public Margin UnsafePadding;
    }
}
