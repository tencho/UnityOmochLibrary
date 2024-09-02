using System;

#nullable enable

namespace Omoch.Framework
{
    public abstract class ViewBase<PeekData> : IViewBase<PeekData>
    {
        private PeekData? peek;
        public PeekData Peek
        {
            get => peek ?? throw new NullReferenceException("Initialized()前にPeekにアクセスすることはできません");
            set => peek = value;
        }

        public bool IsInitialized { get; set; } = false;
        public bool IsDisposed { get; set; } = false;

        public event Action? OnDispose;

        public ViewBase()
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