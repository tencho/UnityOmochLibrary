#nullable enable

using System;

namespace Omoch.Framework
{
    public interface IViewBase<PeekData> : IViewCore
    {
        public PeekData Peek { get; set; }
    }

    public interface IViewBaseWithInput<PeekData, InputData> : IViewCore
    {
        public PeekData Peek { get; set; }
        public InputDispatcherForView<InputData> Input { get; set; }
    }

    public interface IViewCore
    {
        public event Action? OnDispose;
        public void Initialized();
        public void Dispose();
        public bool IsInitialized { get; set; }
        public bool IsDisposed { get; }
    }
}