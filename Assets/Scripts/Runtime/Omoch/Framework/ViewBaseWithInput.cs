using System;

#nullable enable

namespace Omoch.Framework
{
    public abstract class ViewBaseWithInput<PeekData, InputData>
        : IViewBaseWithInput<PeekData, InputData>
    {
        private PeekData? peek;
        public PeekData Peek
        {
            get => peek ?? throw new NullReferenceException("Initialized()前にPeekにアクセスすることはできません");
            set => peek = value;
        }

        private InputDispatcherForView<InputData>? input;
        public InputDispatcherForView<InputData> Input
        {
            get => input ?? throw new NullReferenceException("Initialized()前にInputにアクセスすることはできません");
            set => input = value;
        }

        public bool IsInitialized { get; set; } = false;
        public bool IsDisposed { get; set; } = false;

        public event Action? OnDispose;

        public ViewBaseWithInput()
        {
        }

        public virtual void Initialized()
        {
        }

        public void Dispose()
        {
            IsDisposed = true;
            OnDispose?.Invoke();
        }
    }
}