using LongRoad.Core.Inventory;
using LongRoad.Managers;
using LongRoad.Services;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements.Models
{
    public class InventoryItemUI : IDisposable
    {
        private readonly InventoryStack _stack;
        private readonly VisualElement _root;
        private readonly InventoryService _invetoryService;

        private Image icon;
        private Label nameText;
        private Label counterText;

        private Button deleteButton;

        public Action<InventoryStack> OnItemClicked;

        public InventoryItemUI(
            VisualElement root, 
            InventoryStack stack, 
            InventoryService inventoryService)
        {
            _root = root;
            _stack = stack;
            _invetoryService = inventoryService;

            root.RegisterCallback<ClickEvent>(OnItemClickedCallback);

            icon = root.Q<Image>("item-icon");

            nameText = root.Q<Label>("item-name");
            counterText = root.Q<Label>("item-counter");

            deleteButton = root.Q<Button>("item-delete-button");
            deleteButton.RegisterCallback<ClickEvent>(OnDeleteButtonClicked);
        }

        public void UpdateInformation()
        {
            icon.sprite = _stack.Item.Icon;

            nameText.text = _stack.Item.GetName(GameManager.Instance.Localization);
            counterText.text = GameManager.Instance.Localization.GetMainString(
                "hud_inventory_item_count", 
                new object[] { _stack.Count }
                );
        }

        private void OnDeleteButtonClicked(ClickEvent clickEvent)
        {
            GameManager.Instance.Audio.PlaySfx(AudioManager.CommonSfxClip.Click);

            if (!_invetoryService.TryRemove(_stack.Item, 1))
            {
                Debug.LogError($"Error item deletetion: {_stack.Item.name}");
            }

            if (!clickEvent.isPropagationStopped)
            {
                clickEvent.StopPropagation();
            }
        }

        private void OnItemClickedCallback(ClickEvent clickEvent)
        {
            OnItemClicked?.Invoke(_stack);
        }

        public void Dispose()
        {
            deleteButton.UnregisterCallback<ClickEvent>(OnDeleteButtonClicked);
            _root.UnregisterCallback<ClickEvent>(OnItemClickedCallback);
        }
    }
}
