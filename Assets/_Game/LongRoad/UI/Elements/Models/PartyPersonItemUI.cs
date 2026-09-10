using LongRoad.Core;
using LongRoad.Core.Scriptables;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements.Models
{
    public class PartyPersonItemUI : IDisposable
    {
        private readonly PersonEntity _person;

        public string PersonTag => _person.Prototype.Tag;

        private Image image;
        private VisualElement healBar;
        private VisualElement hungerBar;
        private VisualElement moodBar;

        public PartyPersonItemUI(VisualElement root, PersonEntity person)
        {
            _person = person;

            image = root.Q<Image>("person-image");

            healBar = root.Q("person-heal-bar");
            hungerBar = root.Q("person-hunger-bar");
            moodBar = root.Q("person-mood-bar");

            person.OnStatsChanged += Person_OnStatsChanged;

            UpdateStats();
        }

        private void Person_OnStatsChanged(PersonEntity person)
        {
            UpdateStats();
        }

        public void UpdateStats()
        {
            image.sprite = _person.Prototype.Sprite;

            var healRadio = _person.Heal / (float)_person.Prototype.DefaultHeal;
            healBar.style.width = Length.Percent(healRadio * 100f);

            var hungerRadio = _person.Hunger / (float)_person.Prototype.DefaultHunger;
            hungerBar.style.width = Length.Percent(hungerRadio * 100f);

            var moodRadio = _person.Mood / (float)_person.Prototype.DefaultMood;
            moodBar.style.width = Length.Percent(moodRadio * 100f);
        }

        public void Dispose()
        {
            _person.OnStatsChanged -= Person_OnStatsChanged;
        }
    }
}
