using DG.Tweening;
using UnityEngine;

namespace LongRoad.Misc
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InfiniteSpriteScroll : LongRoadBehaviour
    {
        [SerializeField]
        private float defaultSpeed = 1f;
        [SerializeField]
        private float easeTime = 3f;
        [SerializeField]
        private InfiniteSpriteScroll nextChain;

        public bool IsEasing => speedTranslateTween != null;

        private Transform cameraTransform;
        private float spriteWidth;
        private SpriteRenderer spriteRenderer;
        private SpriteRenderer nextChainSpriteRenderer;

        private float currentOffsetSpeed = 0f;

        private Tween speedTranslateTween = null;

        public override void Init()
        {
            cameraTransform = Camera.main.transform;

            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteWidth = spriteRenderer.bounds.size.x;

            nextChainSpriteRenderer = nextChain.GetComponent<SpriteRenderer>();
        }

        void LateUpdate()
        {
            if (cameraTransform == null)
            {
                return;
            }

            float cameraPositionX = cameraTransform.transform.position.x;
            float cameraHalfSizeX = Camera.main.orthographicSize * Camera.main.aspect;
            float cameraLeftX = cameraPositionX - cameraHalfSizeX;

            if (spriteRenderer.bounds.max.x < cameraLeftX)
            {
                Vector2 current = transform.position;
                current.x = nextChainSpriteRenderer.bounds.max.x + spriteWidth / 2f;

                transform.position = current;
            }

            transform.Translate(Vector2.left * currentOffsetSpeed * Time.deltaTime);
        }

        public void StartScroll(float speedFactor = 1f)
        {
            Dispose();

            speedTranslateTween = DOTween.To(
                () => currentOffsetSpeed, 
                (newValue) => currentOffsetSpeed = newValue, 
                defaultSpeed * speedFactor, easeTime)
                .SetEase(Ease.OutCubic)
                .Play();

            speedTranslateTween.onComplete = () =>
            {
                speedTranslateTween = null;
            };
        }

        public void StopScroll()
        {
            Dispose();

            speedTranslateTween = DOTween.To(
                () => currentOffsetSpeed,
                (newValue) => currentOffsetSpeed = newValue,
                0, easeTime)
                .SetEase(Ease.InCubic)
                .Play();

            speedTranslateTween.onComplete = () =>
            {
                speedTranslateTween = null;
            };
        }

        public override void Dispose()
        {
            if (speedTranslateTween != null)
            {
                speedTranslateTween.Kill();
                speedTranslateTween = null;
            }
        }
    }
}
