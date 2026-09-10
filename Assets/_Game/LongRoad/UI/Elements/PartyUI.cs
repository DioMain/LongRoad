using LongRoad.Core;
using LongRoad.UI.Elements.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class PartyUI : LongRoadUIElement
    {
        private readonly List<PartyPersonItemUI> _partyPersonItems = new();

        [SerializeField]
        private VisualTreeAsset personCardPrefab;

        private VisualElement content;

        public override void Init()
        {
            content = UI.Hud.Q("party-content");

            Local.Persons.OnPersonAdded += People_OnPersonAdded;
            Local.Persons.OnPersonRemoved += People_OnPersonRemoved;
            Local.Persons.OnModifiersApplied += People_OnModifiersApplied;

            EnsurePersonCards();
        }

        private void People_OnModifiersApplied()
        {
            foreach (var item in _partyPersonItems)
            {
                item.UpdateStats();
            }
        }

        private void People_OnPersonRemoved(PersonEntity _)
        {
            EnsurePersonCards();
        }

        private void People_OnPersonAdded(PersonEntity _)
        {
            EnsurePersonCards();
        }

        private void EnsurePersonCards()
        {
            if (_partyPersonItems.Count > 0)
                DisposeCards();

            foreach (var person in Local.Persons.People)
            {
                content.Add(personCardPrefab.Instantiate());
                var root = content.Children().Last().Q("person-card");

                var cardItem = new PartyPersonItemUI(root, person);

                cardItem.UpdateStats();

                _partyPersonItems.Add(cardItem);
            }
        }

        private void DisposeCards()
        {
            foreach (var item in _partyPersonItems)
            {
                item.Dispose();
            }

            _partyPersonItems.Clear();
            content.Clear();
        }

        public override void Dispose()
        {
            Local.Persons.OnPersonAdded -= People_OnPersonAdded;
            Local.Persons.OnPersonRemoved -= People_OnPersonRemoved;
            Local.Persons.OnModifiersApplied -= People_OnModifiersApplied;

            DisposeCards();
        }
    }
}
