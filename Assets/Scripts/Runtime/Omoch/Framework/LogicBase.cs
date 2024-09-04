using System;

#nullable enable

namespace Omoch.Framework
{
    public abstract class LogicBase<ViewOrderData> : ILogicBase<ViewOrderData>
    {
        private ViewOrderData? viewOrder;
        public ViewOrderData ViewOrder
        {
            get => viewOrder ?? throw new NullReferenceException("Initialized()前にViewOrderにアクセスすることはできません");
            set => viewOrder = value;
        }

        public bool IsInitialized { get; set; } = false;
        public bool IsDisposed { get; set; } = false;

        public event Action? OnDispose;

        public LogicBase() { }

        public virtual void Initialized() { }

        public virtual void ViewInitialized() { }

        public void Dispose()
        {
            IsDisposed = true;
            OnDispose?.Invoke();
        }
    }
}