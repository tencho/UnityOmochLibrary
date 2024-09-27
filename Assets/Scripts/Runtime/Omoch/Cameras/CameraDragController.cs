#nullable enable

using Omoch.Animations;
using Omoch.Inputs;
using UnityEngine;

namespace Omoch.Cameras
{
    /// <summary>
    /// 対象オブジェクトをドラッグやホイール操作で色々な角度から見れるようにカメラ操作する
    /// ほぼデバッグ用
    /// 
    /// マウス右ボタンドラッグ: カメラ回転
    /// マウスホイール: カメラズーム
    /// マウス中ボタンドラッグ: カメラ平行移動
    /// </summary>
    public class CameraDragController : MonoBehaviour
    {
        [SerializeField] private GameObject? lookAtObject = default;
        [SerializeField] private Vector3 offset = new(0f, 0f, 0f);
        [SerializeField] private float rotation = 0f;
        [SerializeField, Range(-AngleLimit, AngleLimit)] private float angle = 0f;
        [SerializeField, Min(0)] private float distance = 1f;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float panSpeed = 1f;

        private const float AngleLimit = 89.9f;
        private const float WheelDistanceLevelStep = 0.2f;

        private float initialDistance = 0f;
        private Vector3 pressOffset = new(0f, 0f, 0f);
        private bool isMouseDragging = false;
        private bool isWheelDragging = false;
        private Vector2 pressMousePosition = new(0f, 0f);
        private Vector2 pressWheelPosition = new(0f, 0f);
        private float pressAngle = 0f;
        private float pressRotation = 0f;
        private readonly FloatTracker rotationTracker = new(0.2f, 10f, 4f);
        private readonly FloatTracker angleTracker = new(0.2f, 10f, 4f);
        private readonly FloatTracker distanceLevelTracker = new(0.2f, 1f, 4f);

#if UNITY_EDITOR
        private void OnValidate()
        {
            angleTracker.JumpTo(angle);
            rotationTracker.JumpTo(rotation);
            initialDistance = distance;
            distanceLevelTracker.JumpTo(0f);
            UpdateDistance();
            ApplyTransform();
        }
#endif

        private void Start()
        {
            angleTracker.JumpTo(angle);
            rotationTracker.JumpTo(rotation);
            initialDistance = distance;
            distanceLevelTracker.JumpTo(0f);
            UpdateDistance();
            ApplyTransform();
        }

        private void UpdateDistance()
        {
            distance = initialDistance * Mathf.Pow(2f, distanceLevelTracker.Current);
        }

        private void LateUpdate()
        {
            UpdateDistance();

            var touchPositions = GameInputSystem.GetTouchPositions();
            if (GameInputSystem.IsWheelDown())
            {
                isWheelDragging = true;
                pressWheelPosition = GameInputSystem.GetMousePosition();
                pressOffset = offset;
            }

            if (GameInputSystem.IsWheelUp())
            {
                isWheelDragging = false;
            }

            if (isWheelDragging)
            {
                Vector2 dragDistance = GameInputSystem.GetMousePosition() - pressWheelPosition;
                offset = pressOffset + 0.002f * distance * panSpeed * (transform.up * -dragDistance.y + transform.right * -dragDistance.x);
            }

            if (GameInputSystem.IsTouchDown())
            {
                isMouseDragging = true;
                pressMousePosition = touchPositions[0];
                pressRotation = rotation;
                pressAngle = angle;
            }
            if (GameInputSystem.IsTouchUp())
            {
                isMouseDragging = false;
            }

            if (isMouseDragging && touchPositions.Length > 0)
            {
                Vector2 dragDistance = touchPositions[0] - pressMousePosition;
                rotation = pressRotation - dragDistance.x * rotationSpeed;
                angle = Mathf.Clamp(pressAngle - dragDistance.y * rotationSpeed, -AngleLimit, AngleLimit);
            }

            float wheelScroll = GameInputSystem.GetWheelScroll();
            if (wheelScroll != 0)
            {
                distanceLevelTracker.Destination += (wheelScroll > 0) ? -WheelDistanceLevelStep : WheelDistanceLevelStep;
            }

            angleTracker.MoveTo(angle);
            rotationTracker.MoveTo(rotation);
            angleTracker.AdvanceTime(Time.deltaTime);
            rotationTracker.AdvanceTime(Time.deltaTime);
            distanceLevelTracker.AdvanceTime(Time.deltaTime);

            ApplyTransform();
        }

        private void ApplyTransform()
        {
            float x = Mathf.Cos(angleTracker.Current * Mathf.Deg2Rad) * Mathf.Cos(rotationTracker.Current * Mathf.Deg2Rad);
            float z = Mathf.Cos(angleTracker.Current * Mathf.Deg2Rad) * Mathf.Sin(rotationTracker.Current * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleTracker.Current * Mathf.Deg2Rad);
            Vector3 cameraPosition = new(x, y, z);
            Vector3 lookAtPosition = lookAtObject == null ? new Vector3(0f, 0f, 0f) : lookAtObject.transform.position;
            Vector3 targetPosition = lookAtPosition + offset;
            transform.position = targetPosition + cameraPosition * distance;
            transform.LookAt(targetPosition);
        }
    }
}
