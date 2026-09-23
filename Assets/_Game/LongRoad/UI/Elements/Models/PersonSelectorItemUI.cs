using LongRoad.Core;
using LongRoad.Core.Scriptables;
using System;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements.Models
{
    public class PersonSelectorItemUI
    {
        private readonly PersonEntity _person;

        public event Action<PersonEntity> OnClick;

        public PersonSelectorItemUI(VisualElement root, PersonEntity person)
        {
            _person = person;

            var button = root as Button;

            button.text = person.Prototype.GetName(GameManager.Instance.Localization);
            button.Q<Image>().sprite = person.Prototype.Sprite;

            button.clicked += Button_clicked;
        }

        private void Button_clicked()
        {
            OnClick?.Invoke(_person);
        }
    }
}
