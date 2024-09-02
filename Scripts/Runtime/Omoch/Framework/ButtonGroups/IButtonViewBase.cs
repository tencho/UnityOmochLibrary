using System;
using Omoch.Framework.ButtonGroup;
using UnityEngine.EventSystems;

namespace Omoch
{
    public interface IButtonViewBase : IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnClick;
        public event Action OnDown;
        public event Action OnUp;
        public void Draw(IButtonPeek peek);
    }
}
