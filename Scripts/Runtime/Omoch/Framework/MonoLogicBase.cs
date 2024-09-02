using System;
using Omoch.Tools;
using UnityEngine;

#nullable enable

namespace Omoch.Framework
{
    public class MonoLogicBase<ViewOrderData>
        : MonoBehaviour
        , ILogicBase<ViewOrderData>
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

        public virtual void Initialized()
        {
        }

        public virtual void ViewInitialized()
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