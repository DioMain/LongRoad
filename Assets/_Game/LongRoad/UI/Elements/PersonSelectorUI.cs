using LongRoad.UI.Elements.Models;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using LongRoad.Core;

namespace LongRoad.UI.Elements
{
    public class PersonSelectorUI : LongRoadUIElement
    {
        private readonly List<PersonSelectorItemUI> _items = new();

        [SerializeField]
        private VisualTreeAsset selectorItemPrefab;

        public PersonEntity SelectResult { get; private set; }

        public bool IsCanceled { get; private set; } = false;

        private bool isSelecting = false;
        public bool IsSelecting => selectorCoroutine != null || isSelecting;

        private Button closeButton;
        private VisualElement content;
        private VisualElement root;

        private Coroutine selectorCoroutine = null;

        public override void Init()
        {
            root = UI.MainRoot.Q("person-selector");

            closeButton = UI.MainRoot.Q<Button>("close-modal-button");
            closeButton.clicked += CloseButton_clicked;

            content = UI.MainRoot.Q("person-selector-content");
        }

        private void CloseButton_clicked()
        {
            isSelecting = false;
            IsCanceled = true;
        }

        public Coroutine StartSelect()
        {
            if (IsSelecting)
            {
                Debug.LogWarning("Person selecting is started now");
                return null;
            }

            return selectorCoroutine = StartCoroutine(SelectPipeline());
        }

        private IEnumerator SelectPipeline()
        {
            SelectResult = null;
            isSelecting = true;
            IsCanceled = false;

            content.Clear();
            _items.Clear();

            root.style.display = DisplayStyle.Flex;

            foreach (var person in Local.Persons)
            {
                var instance = selectorItemPrefab.Instantiate();

                content.Add(instance);

                var root = instance.Q("person-select-item-button");

                var item = new PersonSelectorItemUI(root, person);

                item.OnClick += (person) =>
                {
                    SelectResult = person;
                    isSelecting = false;
                };

                _items.Add(item);
            }

            yield return new WaitWhile(() => isSelecting);

            root.style.display = DisplayStyle.None;

            selectorCoroutine = null;
        }

        public override void Dispose()
        {
            content.Clear();
            _items.Clear();
        }
    }
}
