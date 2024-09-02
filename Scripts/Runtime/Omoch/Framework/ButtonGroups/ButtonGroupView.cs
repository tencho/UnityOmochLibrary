using System.Collections.Generic;

#nullable enable

namespace Omoch.Framework.ButtonGroup
{
    public class ButtonGroupView<T>
        : ViewBaseWithInput<IButtonGroupPeek<T>
        , ButtonGroupInput<T>>
        , IButtonGroupViewOrder<T>
        , IUpdatableView
    {
        private readonly Dictionary<T, IButtonViewBase> buttons;

        public ButtonGroupView()
        {
            buttons = new();
        }

        public void Add(IButtonViewBase button, T key)
        {
            button.OnClick += () => Input.Invoke(ButtonGroupInput<T>.Click(key));
            button.OnDown += () => Input.Invoke(ButtonGroupInput<T>.Down(key));
            button.OnUp += () => Input.Invoke(ButtonGroupInput<T>.Up(key));
            buttons.Add(key, button);
        }

        public void UpdateView()
        {
            foreach (T key in buttons.Keys)
            {
                buttons[key].Draw(Peek.GetButtonPeek(key));
            }
        }
    }

    public interface IButtonGroupViewOrder<T>
    {
    }
}
