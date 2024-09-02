#nullable enable

using System;

namespace Omoch.Framework
{
    public interface ILogicBase<ViewOrderData> : ILogicCore
    {
        public ViewOrderData ViewOrder { get; set; }
    }

    public interface ILogicBaseWithInput<ViewOrderData, InputData> : ILogicCore
    {
        public InputDispatcherForLogic<InputData> Input { get; set; }
        public ViewOrderData ViewOrder { get; set; }
    }

    public interface ILogicCore
    {
        public event Action? OnDispose;
        public void Initialized();
        public void ViewInitialized();
        public void Dispose();
        public bool IsInitialized { get; set; }
        public bool IsDisposed { get; }
    }
}