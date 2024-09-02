using System.Collections.Generic;
using UnityEngine;

namespace Omoch.Stats
{
    [ExecuteAlways]
    public class SimpleStats : MonoBehaviour
    {
        private List<float> deltaTimes;
        private float fps;
        private void Start()
        {
            deltaTimes = new();
            fps = 0f;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            deltaTimes.Add(deltaTime);
            if (deltaTimes.Count > 10)
            {
                deltaTimes.RemoveAt(0);
            }
            float total = 0f;
            foreach (float time in deltaTimes)
            {
                total += time;
            }
            total /= deltaTimes.Count;
            fps = 1f / total;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            string text = Mathf.RoundToInt(fps).ToString();
            UnityEditor.Handles.Label(transform.position, text);
        }
#endif
    }
}

