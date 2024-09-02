using System;
using Omoch.Tools;
using UnityEngine;

#nullable enable

namespace Omoch.Primitives
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CustomPlane : MonoBehaviour
    {
        [SerializeField] private float width = 1;
        [SerializeField] private float height = 1;
        [SerializeField, Range(1, 200)] private int segmentsW = 1;
        [SerializeField, Range(1, 200)] private int segmentsH = 1;

        private void Start()
        {
            UpdateMesh();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += UpdateMesh;
        }
#endif

        private void UpdateMesh()
        {
            // delayCall中に既に破棄されているかチェック
            if (this == null || gameObject == null || !gameObject.TryGetComponent(out MeshFilter meshFilter))
            {
                return;
            }

            Mesh mesh = CreateMesh(width, height, segmentsW, segmentsH);
            if (meshFilter.sharedMesh != null)
            {
                GameObjectTools.SafeDestroy(meshFilter.sharedMesh);
            }
            meshFilter.sharedMesh = mesh;

            if (gameObject.TryGetComponent(out MeshCollider meshCollider))
            {
                meshCollider.sharedMesh = mesh;
            }
        }

        private Mesh CreateMesh(float width, float height, int segmentsW, int segmentsH)
        {
            Mesh mesh = new()
            {
                name = "CustomPlane",
                vertices = CreateVertices(width, height, segmentsW, segmentsH),
                uv = CreateUVs(segmentsW, segmentsH),
                triangles = CreateTriangles(segmentsW, segmentsH),
                normals = CreateNormals(segmentsW, segmentsH),
            };
            //mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
            return mesh;
        }

        private Vector2[] CreateUVs(int segmentsW, int segmentsH)
        {
            Vector2[] uvs = new Vector2[(segmentsW + 1) * (segmentsH + 1)];
            int i = 0;
            for (int iz = 0; iz <= segmentsH; iz++)
            {
                for (int ix = 0; ix <= segmentsW; ix++)
                {
                    float u = 1f / segmentsW * ix;
                    float v = 1f - (1f / segmentsH * iz);
                    uvs[i++] = new Vector2(u, v);
                }
            }
            return uvs;
        }

        private Vector3[] CreateVertices(float width, float height, int segmentsW, int segmentsH)
        {
            Vector3[] vertices = new Vector3[(segmentsW + 1) * (segmentsH + 1)];
            int i = 0;
            for (int iz = 0; iz <= segmentsH; iz++)
            {
                for (int ix = 0; ix <= segmentsW; ix++)
                {
                    float x = width * ((float)ix / segmentsW - 0.5f);
                    float z = height * ((float)iz / segmentsH - 0.5f);
                    vertices[i++] = new Vector3(-x, 0, z);
                }
            }
            return vertices;
        }

        private Vector3[] CreateNormals(int segmentsW, int segmentsH)
        {
            int numNormals = (segmentsW + 1) * (segmentsH + 1);
            Vector3[] normals = new Vector3[numNormals];
            for (int i = 0; i < numNormals; i++)
            {
                normals[i] = new Vector3(0, 1, 0);
            }
            return normals;
        }

        private int[] CreateTriangles(int segmentsW, int segmentsH)
        {
            int numTriangles = segmentsW * segmentsH * 2;
            int[] triangles = new int[numTriangles * 3];
            int i = 0;
            for (int iz = 0; iz < segmentsH; iz++)
            {
                for (int ix = 0; ix < segmentsW; ix++)
                {
                    int t0 = iz * (segmentsW + 1) + ix;
                    int t1 = iz * (segmentsW + 1) + ix + 1;
                    int t2 = (iz + 1) * (segmentsW + 1) + ix;
                    int t3 = (iz + 1) * (segmentsW + 1) + ix + 1;
                    triangles[i++] = t0;
                    triangles[i++] = t1;
                    triangles[i++] = t2;
                    triangles[i++] = t1;
                    triangles[i++] = t3;
                    triangles[i++] = t2;

                }
            }
            return triangles;
        }
    }
}
