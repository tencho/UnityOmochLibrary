#nullable enable

namespace Omoch.Framework
{
    public delegate void InputHandler<T>(T data);
    public class InputDispatcher<InputData> : InputDispatcherForLogic<InputData> , InputDispatcherForView<InputData>
    {
        public event InputHandler<InputData>? OnInput;
        public InputDispatcher()
        {
        }
        
        public void Invoke(InputData data)
        {
            OnInput?.Invoke(data);
        }
    }
    
    public interface InputDispatcherForLogic<InputData>
    {
        public event InputHandler<InputData> OnInput;
    }

    public interface InputDispatcherForView<InputData>
    {
        public void Invoke(InputData data);
    }
}