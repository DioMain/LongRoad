using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class NotificationUI : LongRoadUIElement
    {
        public enum WarnLevel
        {
            Regular, Warning
        }

        [SerializeField]
        private VisualTreeAsset notificationItemPrefab;

        private VisualElement root;

        public override void Init()
        {
            root = UI.MainRoot.Q("notifications");
        }

        public void Notify(string text, WarnLevel warnLevel = WarnLevel.Regular)
        {
            var instance = notificationItemPrefab.Instantiate();
            instance.style.flexDirection = FlexDirection.Row;

            root.Insert(0, instance);

            instance.Q<Label>().text = text;

            var anim = DOTween.Sequence();

            var appendAnimation = DOTween.To(
                () => instance.style.marginBottom.value.value,
                (value) => instance.style.marginBottom = Length.Pixels(value), 0, 1f)
                .From(-76)
                .SetEase(Ease.OutQuart);

            var outAnimation1 = instance.DOMoveX(76f, .3f)
                .From(0)
                .SetEase(Ease.InCubic);

            var outAnimation2 = instance.DOMoveX(-3000f, 1f);

            anim.Append(appendAnimation);
            anim.AppendInterval(2);
            anim.Append(outAnimation1);
            anim.Append(outAnimation2);

            anim.onComplete = () =>
            {
                instance.RemoveFromHierarchy();
            };

            if (warnLevel == WarnLevel.Warning)
            {
                var root = instance.Children().First();

                DOTween.To(
                    () => root.style.backgroundColor.value,
                    (value) => root.style.backgroundColor = value, new Color(1, 0, 0, 0), 1f)
                    .From(Color.red)
                    .Play();
            }

            anim.Play();
        }
    }
}
