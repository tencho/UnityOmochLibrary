using System;

#nullable enable

namespace Omoch.Framework.ButtonGroup
{
    public class ButtonLogic : IButtonPeek
    {
        /// <summary>ボタンを押すのにかかる時間</summary>
        private const float DownDuration = 0.2f;
        /// <summary>ボタンを離すのにかかる時間</summary>
        private const float UpDuration = 0.2f;

        private ButtonState state;
        public float DownPercent { get; private set; }

        public ButtonLogic()
        {
            state = ButtonState.Up();
        }

        public void ApplyInput(ButtonGroupInputKind kind)
        {
            switch (kind)
            {
                case ButtonGroupInputKind.Click:
                    break;

                case ButtonGroupInputKind.Down:
                    state = ButtonState.UpToDown(0f);
                    break;

                case ButtonGroupInputKind.Up:
                    state = ButtonState.DownToUp(0f);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void Update(float seconds)
        {
            switch (state.Kind)
            {
                case ButtonStateKind.Up:
                case ButtonStateKind.Down:
                    break;

                case ButtonStateKind.DownToUp:
                    state.Progress += seconds / UpDuration;
                    if (state.Progress >= 1f)
                    {
                        state = ButtonState.Up();
                    }
                    break;

                case ButtonStateKind.UpToDown:
                    state.Progress += seconds / DownDuration;
                    if (state.Progress >= 1f)
                    {
                        state = ButtonState.Down();
                    }
                    break;

                default:
                    throw new NotImplementedException();
            };

            DownPercent = state.Kind switch
            {
                ButtonStateKind.Up => 0f,
                ButtonStateKind.Down => 1f,
                ButtonStateKind.DownToUp => 1f - state.Progress,
                ButtonStateKind.UpToDown => state.Progress,
                _ => throw new NotImplementedException(),
            };
        }
    }

    public interface IButtonPeek
    {
        public float DownPercent { get; }
    }
}
