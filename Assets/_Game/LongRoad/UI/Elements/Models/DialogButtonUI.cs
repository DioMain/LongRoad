using LongRoad.Core.Localization;
using System;
using UnityEngine;
using UnityEngine.UIElements;

using DialogVariant = LongRoad.UI.Elements.DialogUI.Variant;

namespace LongRoad.UI.Elements.Models
{
    public class DialogButtonUI : IDisposable
    {
        public DialogVariant CurrentVariant { get; private set; }

        public event Action<DialogVariant> OnClick;

        private Button root;

        public DialogButtonUI(Button root, DialogVariant variant, LocalizationManager localization)
        {
            this.root = root;

            CurrentVariant = variant;

            root.text = localization.GetEventString(variant.Tag);
            root.clicked += OnClickCallback;
        }

        private void OnClickCallback()
        {
            OnClick?.Invoke(CurrentVariant);
        }

        public void Dispose()
        {
            root.clicked -= OnClickCallback;
        }
    }
}
