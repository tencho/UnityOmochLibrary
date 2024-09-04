using System;
using Omoch.Tools;
using UnityEngine;

#nullable enable

namespace Omoch.Framework
{
    public class MonoViewBase<PeekData>
        : MonoBehaviour
        , IViewBase<PeekData>
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

        public virtual void Initialized()
        {
        }

        public void Dispose()
        {
            GameObjectTools.SafeDestroy(gameObject);
        }

        private void OnDestroy()
        {
            IsDisposed = true;
            OnDispose?.Invoke();
        }
    }
}