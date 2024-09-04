using System;
using UnityEngine;
using UnityEngine.UI;

#nullable enable

namespace Omoch.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(RectTransform))]
    public class UiRing : MaskableGraphic
    {
        [SerializeField] private Sprite? sprite = default;
        [field: SerializeField] public float StartAngle { get; set; } = 0f;
        [field: SerializeField] public float EndAngle { get; set; } = 270f;
        [field: SerializeField] public float TileU { get; set; } = 1f;
        [field: SerializeField] public float TileV { get; set; } = 1f;
        [field: SerializeField, Min(1)] public int Segments { get; set; } = 90;
        [field: SerializeField, Min(0f)] public float Thickness { get; set; } = 20f;
        [field: SerializeField] public bool AutoSegments { get; set; } = true;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            SetVerticesDirty();
        }
#endif
        public void Refresh()
        {
            SetVerticesDirty();
        }

        protected override void UpdateMaterial()
        {
            base.UpdateMaterial();

            Texture texture = (sprite == null) ? Texture2D.whiteTexture : sprite.texture;
            canvasRenderer.SetTexture(texture);
        }
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            Rect rect = GetPixelAdjustedRect();
            float uMin, uMax, vMin, vMax;
            if (sprite == null)
            {
                uMin = vMin = 0f;
                uMax = vMax = 1f;
            }
            else
            {
                uMin = sprite.textureRect.xMin / sprite.texture.width;
                uMax = sprite.textureRect.xMax / sprite.texture.width;
                vMin = sprite.textureRect.yMin / sprite.texture.height;
                vMax = sprite.textureRect.yMax / sprite.texture.height;
            }

            float _startAngle = -(StartAngle - 90f);
            float _endAngle = -(this.EndAngle - 90f);

            int _segments;
            if (AutoSegments)
            {
                _segments = Mathf.Max(1, Mathf.CeilToInt(this.Segments * Math.Abs(StartAngle - this.EndAngle) / 360f));
            }
            else
            {
                _segments = this.Segments;
            }

            float radius0 = Math.Min(rect.width, rect.height) / 2;
            float radius1 = Mathf.Max(0f, radius0 - Thickness);

            vh.Clear();
            for (int i = 0; i <= _segments; i++)
            {
                float ratio = i / (float)_segments;
                float angle = _startAngle + (_endAngle - _startAngle) * ratio;
                float cos = Mathf.Cos(Mathf.PI / 180 * angle);
                float sin = Mathf.Sin(Mathf.PI / 180 * angle);
                float u = uMin + (uMax - uMin) * ratio;
                vh.AddVert(new Vector2(cos * radius0, sin * radius0), color, new Vector2(u * TileU, vMax * TileV));
                vh.AddVert(new Vector2(cos * radius1, sin * radius1), color, new Vector2(u * TileU, vMin * TileV));

                int index = (i - 1) * 2;
                if (i >= 1)
                {

                    vh.AddTriangle(0 + index, 2 + index, 3 + index);
                    if (radius1 > 0f)
                    {
                        vh.AddTriangle(0 + index, 3 + index, 1 + index);
                    }
                }
            }
        }
    }
}

