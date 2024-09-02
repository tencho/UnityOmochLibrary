namespace Omoch.Framework.ButtonGroup
{
    public struct ButtonState
    {
        public ButtonStateKind Kind { get; set; }
        public float Progress { get; set; }
        
        public static ButtonState Up() => new()
        {
            Kind = ButtonStateKind.Up,
        };

        public static ButtonState Down() => new()
        {
            Kind = ButtonStateKind.Down,
        };

        public static ButtonState UpToDown(float progress) => new()
        {
            Kind = ButtonStateKind.UpToDown,
            Progress = progress,
        };

        public static ButtonState DownToUp(float progress) => new()
        {
            Kind = ButtonStateKind.DownToUp,
            Progress = progress,
        };
    }

    public enum ButtonStateKind : int
    {
        Up = 1,
        Down = 2,
        DownToUp = 3,
        UpToDown = 4,
    }
}
