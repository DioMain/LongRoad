using LongRoad.UI.Elements.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class InventoryUI : LongRoadUIElement
    {
        private readonly List<InventoryItemUI> _inventoryItems = new();

        [SerializeField]
        private VisualTreeAsset inventoryItemPrefab;

        private Button inventoryButton;
        private VisualElement inventory;
        private VisualElement inventoryContent;
        private Label weightLabel;

        private bool isOpen = false;

        public override void Init()
        {
            inventoryButton = UI.Hud.Q<Button>("inventory-button");
            inventoryButton.clicked += InventoryButton_clicked;

            inventory = UI.Inventory;

            inventoryContent = inventory.Q("inventory-scroll-view");
            weightLabel = inventory.Q<Label>("inventory-weight");

            Local.Inventory.OnChanged += Inventory_OnChanged;
        }

        private void UpdateInventory()
        {
            if (!isOpen)
                return;

            ClearList();

            weightLabel.text = Game.Localization.GetMainString(
                "hud_inventory_weight_km", 
                new object[] { Local.Inventory.CurrentWeight, Local.Data.Car.MaxWeight }
                );

            foreach (var stack in Local.Inventory.Stacks)
            {
                var instance = inventoryItemPrefab.Instantiate();
                inventoryContent.Add(instance);

                var root = instance.Q("inventory-item");

                var item = new InventoryItemUI(root, stack, Local.Inventory);
                item.UpdateInformation();

                _inventoryItems.Add(item);
            }
        }

        private void Inventory_OnChanged()
        {
            UpdateInventory();
        }

        private void InventoryButton_clicked()
        {
            isOpen = !isOpen;

            if (isOpen)
            {
                inventory.style.display = DisplayStyle.Flex;

                UpdateInventory();
            }
            else
            {
                inventory.style.display = DisplayStyle.None;
            }
        }

        public void ClearList()
        {
            foreach (var item in _inventoryItems)
            {
                item.Dispose();
            }

            _inventoryItems.Clear();
            inventoryContent.Clear();
        }

        public override void Dispose()
        {
            inventoryButton.clicked -= InventoryButton_clicked;
            Local.Inventory.OnChanged -= Inventory_OnChanged;

            ClearList();
        }
    }
}
