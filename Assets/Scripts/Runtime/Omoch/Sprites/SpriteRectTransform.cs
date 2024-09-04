using UnityEngine;

namespace Omoch.Sprites
{
    /// <summary>
    /// 2DSpriteをwidthとheightでサイズ指定できる
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRectTransform : MonoBehaviour
    {
        [SerializeField] private float width = 100;
        [SerializeField] private float height = 100;

        private void Start()
        {
            ApplySize();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += ApplySize;
        }
#endif

        public float Width
        {
            get => width;
            set
            {
                width = value;
                ApplyWidth();
            }
        }

        public float Height
        {
            get => height;
            set
            {
                height = value;
                ApplyHeight();
            }
        }

        public void SetSize(Vector2 size)
        {
            Width = size.x;
            Height = size.y;
        }

        private void ApplySize()
        {
            // delayCall中に既に破棄されているかチェック
            if (this == null || gameObject == null)
            {
                return;
            }

            ApplyWidth();
            ApplyHeight();
        }

        private void ApplyWidth()
        {
            TryGetComponent(out SpriteRenderer spriteRenderer);
            var sprite = spriteRenderer.sprite;
            if (sprite is null)
            {
                return;
            }

            if (spriteRenderer.drawMode == SpriteDrawMode.Simple)
            {
                var localScale = spriteRenderer.transform.localScale;
                localScale.x = width / sprite.textureRect.width * sprite.pixelsPerUnit;
                spriteRenderer.transform.localScale = localScale;
            }
            else
            {
                var size = spriteRenderer.size;
                size.x = width;
                spriteRenderer.size = size;
            }
        }

        private void ApplyHeight()
        {
            TryGetComponent(out SpriteRenderer spriteRenderer);
            var sprite = spriteRenderer.sprite;
            if (sprite is null)
            {
                return;
            }

            if (spriteRenderer.drawMode == SpriteDrawMode.Simple)
            {
                var localScale = spriteRenderer.transform.localScale;
                localScale.y = height / sprite.textureRect.height * sprite.pixelsPerUnit;
                spriteRenderer.transform.localScale = localScale;
            }
            else
            {
                var size = spriteRenderer.size;
                size.y = height;
                spriteRenderer.size = size;
            }
        }
    }
}
