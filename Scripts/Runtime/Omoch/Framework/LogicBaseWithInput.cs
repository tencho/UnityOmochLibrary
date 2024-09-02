using System;

#nullable enable

namespace Omoch.Framework
{
    public abstract class LogicBaseWithInput<ViewOrderData, InputData>
        : ILogicBaseWithInput<ViewOrderData, InputData>
    {
        private ViewOrderData? viewOrder;
        public ViewOrderData ViewOrder
        {
            get => viewOrder ?? throw new NullReferenceException("Initialized()前にViewOrderにアクセスすることはできません");
            set => viewOrder = value;
        }

        private InputDispatcherForLogic<InputData>? input;
        public InputDispatcherForLogic<InputData> Input
        {
            get => input ?? throw new NullReferenceException("Initialized()前にInputにアクセスすることはできません");
            set => input = value;
        }

        public bool IsInitialized { get; set; } = false;
        public bool IsDisposed { get; set; } = false;

        public event Action? OnDispose;

        public LogicBaseWithInput() { }

        public virtual void Initialized() { }

        public virtual void ViewInitialized() { }

        public void Dispose()
        {
            IsDisposed = true;
            OnDispose?.Invoke();
        }
    }
}