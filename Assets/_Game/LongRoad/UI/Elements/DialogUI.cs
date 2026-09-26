using LongRoad.UI.Elements.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class DialogUI : LongRoadUIElement
    {
        public struct Variant
        {
            /// <summary>
            /// Button text localization
            /// </summary>
            public string Tag;
            /// <summary>
            /// Additional metadata for choice item
            /// </summary>
            public object Meta;
        }

        private readonly List<DialogButtonUI> _buttons = new();

        [SerializeField]
        private VisualTreeAsset selectButtonPrefab;

        /// <summary>
        /// Contains last selected variant, if select isActive this value is null
        /// </summary>
        public Variant? SelectResult { get; private set; }
        public bool IsActive => dialogCoroutine != null;

        private VisualElement root;
        private VisualElement buttonSlot;
        private Label label;

        private Coroutine dialogCoroutine = null;

        public override void Init()
        {
            root = UI.Dialog;
                
            buttonSlot = root.Q("dialog-button-slot");
            label = root.Q<Label>("dialog-text");
        }

        /// <param name="dialogTag">Need for main text localization</param>
        /// <param name="variants">Button variants</param>
        public Coroutine StartDialog(string dialogTag, params Variant[] variants)
        {
            if (IsActive)
            {
                Debug.Log("Dialog already started!");
                return dialogCoroutine;
            }

            var actualVariants = variants.Length > 0 ? variants : new Variant[] {
                new() { Tag = "dialog_default_variant", Meta = null },
            };

            return dialogCoroutine = StartCoroutine(DialogPipeline(dialogTag, actualVariants));
        }

        private IEnumerator DialogPipeline(string dialogTag, Variant[] variants)
        {
            SelectResult = null;

            label.text = Game.Localization.GetEventString(dialogTag);

            foreach (var variant in variants)
            {
                var instance = selectButtonPrefab.Instantiate();
                instance.style.width = new StyleLength(Length.Percent(50));
                buttonSlot.Add(instance);

                var root = instance.Q<Button>("item-button");

                var item = new DialogButtonUI(root, variant, Game.Localization);

                item.OnClick += Item_OnClick;

                _buttons.Add(item);
            }

            root.style.display = DisplayStyle.Flex;

            yield return new WaitWhile(() => !SelectResult.HasValue);

            DisposeButtonList();

            root.style.display = DisplayStyle.None;

            dialogCoroutine = null;
        }

        private void Item_OnClick(Variant variant)
        {
            SelectResult = variant;
        }

        private void DisposeButtonList()
        {
            buttonSlot.Clear();
            _buttons.Clear();
        }

        public override void Dispose()
        {
            if (IsActive)
            {
                StopCoroutine(dialogCoroutine);
                dialogCoroutine = null;
            }

            DisposeButtonList();
        }
    }

    public static class DialogUIConstants
    {
        public static readonly DialogUI.Variant YES_VARIANT = new()
        {
            Tag = "dialog_yes",
            Meta = null
        };

        public static readonly DialogUI.Variant NO_VARIANT = new()
        {
            Tag = "dialog_no",
            Meta = null
        };
    }
}
