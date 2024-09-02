using UnityEngine;
using UnityEngine.UI;

#nullable enable

namespace Omoch.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(RectTransform))]
    public class UiSimpleQuad : MaskableGraphic
    {
        [SerializeField] private Sprite? sprite = default;

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

            Rect rect = GetPixelAdjustedRect();

            vh.Clear();
            vh.AddVert(new Vector2(rect.xMin, rect.yMin), color, new Vector2(uMin, vMin));
            vh.AddVert(new Vector2(rect.xMax, rect.yMin), color, new Vector2(uMax, vMin));
            vh.AddVert(new Vector2(rect.xMax, rect.yMax), color, new Vector2(uMax, vMax));
            vh.AddVert(new Vector2(rect.xMin, rect.yMax), color, new Vector2(uMin, vMax));
            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }
    }
}