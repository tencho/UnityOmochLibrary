using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Omoch.Framework.ButtonGroup
{
    public class MonoButtonView : MonoBehaviour, IButtonViewBase
    {
        public event Action OnClick;
        public event Action OnDown;
        public event Action OnUp;

        public virtual void Draw(IButtonPeek peek)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnUp?.Invoke();
        }
    }
}
