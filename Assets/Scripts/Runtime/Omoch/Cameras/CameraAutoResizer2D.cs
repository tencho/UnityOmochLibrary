using System;
using UnityEngine;

namespace Omoch.Cameras
{
    /// <summary>
    /// 非Canvas要素を写す2DカメラをCanvasのScaleWithScreenSize/Expandと同じ挙動で描画範囲が自動リサイズされるようにする
    /// </summary>
    [ExecuteAlways]
    public class CameraAutoResizer2D : MonoBehaviour
    {
        [SerializeField] private int width = 1080;
        [SerializeField] private int height = 1920;

        private Camera targetCamera;
        private int previousWidth;
        private int previousHeight;

        private void Awake()
        {
            previousWidth = -1;
            previousHeight = -1;

            if (!TryGetComponent(out targetCamera))
            {
                throw new Exception("CameraAutoResizer2DコンポーネントはCameraにアタッチしてください");
            }
        }

        private void Update()
        {
            if (Screen.width == previousWidth && Screen.height == previousHeight)
            {
                return;
            }

            previousWidth = Screen.width;
            previousHeight = Screen.height;

            float screenAspect = (float)Screen.width / Screen.height;
            float cameraAspect = (float)width / height;
            if (screenAspect >= cameraAspect)
            {
                // 指定アスペクトより横長の場合
                targetCamera.orthographicSize = height / 2f;
            }
            else
            {
                // 指定アスペクトより縦長の場合
                targetCamera.orthographicSize = width / screenAspect / 2;
            }
        }
    }
}

