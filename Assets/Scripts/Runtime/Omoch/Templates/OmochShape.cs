using System.Collections.Generic;
using Omoch.Tools;
using UnityEngine;

namespace Omoch.Templates
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    internal class OmochShape : MonoBehaviour
    {
        [SerializeField] private Color color = Color.white;

        private void Start()
        {
            CreateMesh();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += CreateMesh;
        }
#endif

        private void CreateMesh()
        {
            // delayCall中に既に破棄されているかチェック
            if (this == null || gameObject == null || !TryGetComponent(out MeshFilter meshFilter))
            {
                return;
            }

            var mesh = new Mesh();

            var vertices = new List<Vector3>()
            {
                new(0f, 0f, 0f),
                new(0f, 10f, 0f),
                new(10f, 0f, 0f),
            };

            var triangles = new List<int>
            {
                0,1,2,
            };

            var colors = new Color[vertices.Count];
            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.colors = colors;

            if (meshFilter.sharedMesh != null)
            {
                GameObjectTools.SafeDestroy(meshFilter.sharedMesh);
            }
            meshFilter.sharedMesh = mesh;
        }
    }
}