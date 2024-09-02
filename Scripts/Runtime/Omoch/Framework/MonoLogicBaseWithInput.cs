using System;
using Omoch.Tools;
using UnityEngine;

#nullable enable

namespace Omoch.Framework
{
    public class MonoLogicBaseWithInput<ViewOrderData, InputData>
        : MonoBehaviour
        , ILogicBaseWithInput<ViewOrderData, InputData>
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