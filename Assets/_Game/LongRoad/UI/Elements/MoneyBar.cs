using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class MoneyBar : LongRoadUIElement
    {
        private VisualElement moneyBar;

        private Label label;

        public override void Init()
        {
            moneyBar = UI.Hud.Q("money");

            label = moneyBar.Q<Label>(className: "text");

            Local.Money.OnMoneyChanged += OnMoneyChanged;
            OnMoneyChanged(Local.Money.Balance);
        }

        private void OnMoneyChanged(float balance)
        {
            label.text = balance.ToString();
        }

        public override void Dispose()
        {
            Local.Money.OnMoneyChanged -= OnMoneyChanged;
        }
    }
}
