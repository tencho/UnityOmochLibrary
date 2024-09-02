using System;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace Omoch.Framework.ButtonGroup
{
    public class ButtonGroupLogic<T>
        : LogicBaseWithInput<IButtonGroupViewOrder<T>
        , ButtonGroupInput<T>>
        , IButtonGroupPeek<T>
        , IUpdatableLogic
    {
        public event Action<T>? OnClick;

        private readonly Dictionary<T, ButtonLogic> buttons;

        public ButtonGroupLogic()
        {
            buttons = new();
        }

        public override void Initialized()
        {
            Input.OnInput += OnInputHandler;
        }

        public ButtonLogic Add(T kind)
        {
            var button = new ButtonLogic();
            buttons.Add(kind, button);
            return button;
        }

        public void UpdateLogic()
        {
            foreach (ButtonLogic button in buttons.Values)
            {
                button.Update(Time.deltaTime);
            }
        }

        private void OnInputHandler(ButtonGroupInput<T> data)
        {
            if (!buttons.ContainsKey(data.ButtonKind))
            {
                throw new Exception($"ボタン({data.ButtonKind})が登録されていません");
            }

            buttons[data.ButtonKind].ApplyInput(data.Kind);
            switch (data.Kind)
            {
                case ButtonGroupInputKind.Click:
                    OnClick?.Invoke(data.ButtonKind);
                    break;
                case ButtonGroupInputKind.Up:
                    break;
                case ButtonGroupInputKind.Down:
                    break;
            }
        }

        public IButtonPeek GetButtonPeek(T kind) => buttons[kind];
    }

    public interface IButtonGroupPeek<T>
    {
        public IButtonPeek GetButtonPeek(T kind);
    }
}
