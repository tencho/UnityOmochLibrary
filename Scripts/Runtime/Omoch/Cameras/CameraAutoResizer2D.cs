using UnityEngine;

namespace Omoch.Cameras
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
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
            TryGetComponent(out targetCamera);
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

