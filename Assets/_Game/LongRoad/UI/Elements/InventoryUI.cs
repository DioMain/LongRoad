using LongRoad.Core;
using LongRoad.Core.Inventory;
using LongRoad.Core.Scriptables;
using LongRoad.Managers;
using LongRoad.UI.Elements.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class InventoryUI : LongRoadUIElement
    {
        private readonly List<InventoryItemUI> _inventoryItems = new();

        [SerializeField]
        private VisualTreeAsset inventoryItemPrefab;

        public bool IsItemUsing => useItemCoroutine != null;

        private Button inventoryButton;
        private VisualElement inventory;
        private VisualElement inventoryContent;
        private Label weightLabel;

        private bool isOpen = false;

        private Coroutine useItemCoroutine;

        public override void Init()
        {
            inventoryButton = UI.Hud.Q<Button>("inventory-button");
            inventoryButton.clicked += InventoryButton_clicked;

            inventory = UI.Inventory;

            inventoryContent = inventory.Q("inventory-scroll-view");
            weightLabel = inventory.Q<Label>("inventory-weight");

            Local.Inventory.OnChanged += Inventory_OnChanged;
        }

        public void UseItem(InventoryStack stack)
        {
            if (IsItemUsing)
            {
                Debug.LogWarning("Some item using now");
                return;
            }

            useItemCoroutine = StartCoroutine(UseItemPipeline(stack));
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

                item.OnItemClicked += (stack) =>
                {
                    UseItem(stack);
                };

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

            Game.Audio.PlaySfx(AudioManager.CommonSfxClip.Click);

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

        private IEnumerator UseItemPipeline(InventoryStack stack)
        {
            PersonEntity[] persons;
            bool canceled = false;

            Game.Audio.PlaySfx(AudioManager.CommonSfxClip.Click);

            switch (stack.Item.TargetKind)
            {
                case ItemTargetKind.Person:
                    yield return UI.PersonSelector.StartSelect();

                    persons = new PersonEntity[1] { UI.PersonSelector.SelectResult };
                    canceled = UI.PersonSelector.IsCanceled;
                    break;
                case ItemTargetKind.AllPersons:
                    persons = Local.Persons.People.ToArray();
                    break;
                case ItemTargetKind.None:
                default:
                    persons = Array.Empty<PersonEntity>();
                    break;
            }


            if (!canceled)
            {
                yield return Local.Inventory.UseItem(stack.Item, this, persons);
            }

            useItemCoroutine = null;
        }
    }
}
