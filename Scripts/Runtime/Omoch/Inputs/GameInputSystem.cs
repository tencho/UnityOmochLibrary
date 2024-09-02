#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine;
using System.Linq;

namespace Omoch.Inputs
{
        /// <summary>
        /// マウス操作とタッチ操作を統合して情報を取れるようにしたもの
        /// タッチとマウス両方対応しているデバイスではタッチが優先される
        /// </summary>
        public class GameInputSystem
        {
                public static bool IsTouchDown()
                {
#if ENABLE_INPUT_SYSTEM
                        if (
                                Touchscreen.current is not null
                                && Touchscreen.current.touches.Any(it => it.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
                        )
                        {
                                return true;
                        }
                        else if (Mouse.current is not null)
                        {
                                return Mouse.current.leftButton.wasPressedThisFrame;
                        }
                        else
                        {
                                return false;
                        }
#else
                        if (Input.touchCount > 0 && Input.touches.Any(it => it.phase == TouchPhase.Began))
                        {
                                return true;
                        }
                        else
                        {
                                return Input.GetMouseButtonDown(0);
                        }
#endif
                }

                public static bool IsTouchUp()
                {
#if ENABLE_INPUT_SYSTEM
                        if (
                                Touchscreen.current is not null
                                && Touchscreen.current.touches.All(it => {
                                        var phase = it.phase.ReadValue();
                                        return phase == UnityEngine.InputSystem.TouchPhase.Ended
                                        || phase == UnityEngine.InputSystem.TouchPhase.None
                                        || phase == UnityEngine.InputSystem.TouchPhase.Canceled;
                                })
                        )
                        {
                                return true;
                        }
                        else if (Mouse.current is not null)
                        {
                                return Mouse.current.leftButton.wasReleasedThisFrame;
                        }
                        else
                        {
                                return false;
                        }
#else
                        if (
                                Input.touchCount > 0
                                && Input.touches.All(it =>
                                {
                                        return it.phase == TouchPhase.Ended
                                        || it.phase == TouchPhase.Canceled;
                                })
                        )
                        {
                                return true;
                        }
                        else
                        {
                                return Input.GetMouseButtonUp(0);
                        }
#endif
                }

                public static Vector2[] GetTouchPositions()
                {
#if ENABLE_INPUT_SYSTEM
                        if (Touchscreen.current is not null)
                        {
                                return Touchscreen.current.touches.Where(it =>
                                {
                                        var phase = it.phase.ReadValue();
                                        return phase == UnityEngine.InputSystem.TouchPhase.Began || phase == UnityEngine.InputSystem.TouchPhase.Moved;
                                }).Select(it => it.position.ReadValue()).ToArray();
                        }
                        else if (Mouse.current is not null)
                        {
                                Vector2 position = Mouse.current.position.ReadValue();
                                if (Mouse.current.leftButton.isPressed)
                                {
                                        return new Vector2[] { position };
                                }
                                else
                                {
                                        return new Vector2[0];
                                }
                        }
                        else
                        {
                                return new Vector2[0];
                        }
#else
                        if (Input.touchSupported)
                        {

                                var touches = new Vector2[Input.touchCount];
                                for (int i = 0; i < Input.touchCount; i++)
                                {
                                        touches[i] = Input.GetTouch(i).position;
                                }
                                return touches;
                        }
                        else
                        {
                                var mousePosition = (Vector2)Input.mousePosition;
                                if (
                                        float.IsInfinity(mousePosition.x)
                                        || float.IsNegativeInfinity(mousePosition.x)
                                        || float.IsInfinity(mousePosition.y)
                                        || float.IsNegativeInfinity(mousePosition.y)
                                )
                                {
                                        return new Vector2[0];
                                }
                                if (Input.GetMouseButton(0))
                                {
                                        return new Vector2[] { mousePosition };
                                }
                                else
                                {
                                        return new Vector2[0];
                                }
                        }
#endif
                }

                public static Vector2 GetMousePosition()
                {
#if ENABLE_INPUT_SYSTEM
                        if (Mouse.current is null)
                        {
                              throw new System.Exception("マウスがありません");  
                        }
                        return Mouse.current.position.ReadValue();
#else
                        return (Vector2)Input.mousePosition;
#endif
                }

                public static bool IsWheelDown()
                {
#if ENABLE_INPUT_SYSTEM
                        if (Mouse.current is null)
                        {
                                return false;
                        }
                        return Mouse.current.middleButton.wasPressedThisFrame;
#else
                        return Input.GetMouseButtonDown(2);
#endif
                }

                public static bool IsWheelUp()
                {
#if ENABLE_INPUT_SYSTEM
                        if (Mouse.current is null)
                        {
                                return false;
                        }
                        return Mouse.current.middleButton.wasReleasedThisFrame;
#else
                        return Input.GetMouseButtonUp(2);
#endif
                }

                public static float GetWheelScroll()
                {
#if ENABLE_INPUT_SYSTEM
                        if (Mouse.current is null)
                        {
                                return 0f;
                        }
                        return Mouse.current.scroll.ReadValue().y;
#else
                        return Input.GetAxis("Mouse ScrollWheel");
#endif
                }
        }
}