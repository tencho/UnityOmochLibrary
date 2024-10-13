using System;
using UnityEditor;
using UnityEngine;

namespace Omoch.Framework
{
    [CustomEditor(typeof(OmochBinder))]
    public class OmochBinderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("未処理Logic/Viewの出力"))
            {
                LogReady();
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void LogReady()
        {
            var binder = target as OmochBinder ?? throw new NullReferenceException();

            foreach (var key in binder.ReadyLogics.Keys)
            {
                var logic = binder.ReadyLogics[key];
                Debug.Log($"未処理Logic: key={key.Name}, logic={logic}");
            }

            foreach (var key in binder.ReadyViews.Keys)
            {
                var view = binder.ReadyViews[key];
                Debug.Log($"未処理View: key={key.Name}, view={view}");
            }
        }
    }
}