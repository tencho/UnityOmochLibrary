namespace Omoch.Framework.ButtonGroup
{
    public struct ButtonGroupInput<T>
    {
        public ButtonGroupInputKind Kind { get; set; }
        public T ButtonKind { get; set; }

        public static ButtonGroupInput<T> Click(T buttonKind) => new()
        {
            Kind = ButtonGroupInputKind.Click,
            ButtonKind = buttonKind,
        };

        public static ButtonGroupInput<T> Up(T buttonKind) => new()
        {
            Kind = ButtonGroupInputKind.Up,
            ButtonKind = buttonKind,
        };

        public static ButtonGroupInput<T> Down(T buttonKind) => new()
        {
            Kind = ButtonGroupInputKind.Down,
            ButtonKind = buttonKind,
        };
    }

    public enum ButtonGroupInputKind
    {
        Click,
        Down,
        Up,
    }
}
