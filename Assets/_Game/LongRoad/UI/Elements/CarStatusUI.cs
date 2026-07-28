using LongRoad.Core;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class CarStatusUI : LongRoadUIElement
    {
        private VisualElement carStatus;
        private Label carName;
        private VisualElement healBarFront;

        private CarEntity car;

        public override void Init()
        {
            car = Local.Data.Car;
            if (car == null)
                return;

            carStatus = UI.Hud.Q("car-status");
            carName = carStatus.Q<Label>("car-name");
            healBarFront = carStatus.Q("car-heal-bar-front");

            carName.text = car.Entity.GetName(Game.Localization);

            car.OnDurabilityChanged += OnDurabilityChanged;
            OnDurabilityChanged(car, car.Durability);
        }

        private void OnDurabilityChanged(CarEntity _, int durability)
        {
            var max = car.Entity.Durability;
            var ratio = max > 0 ? (float)durability / max : 0f;
            healBarFront.style.width = Length.Percent(ratio * 100f);
        }

        public override void Dispose()
        {
            if (car != null)
                car.OnDurabilityChanged -= OnDurabilityChanged;
        }
    }
}
