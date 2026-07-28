using LongRoad.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class GasStatusUI : LongRoadUIElement
    {
        private VisualElement gasStatus;
        private Image gasBarCursor;

        private CarEntity car;

        public override void Init()
        {
            car = Local.Data.Car;
            if (car == null)
                return;

            gasStatus = UI.Hud.Q("gas-status");
            gasBarCursor = gasStatus.Q<Image>("gas-bar-cursor");

            car.OnFuelChanged += OnFuelChanged;
            OnFuelChanged(car, car.Fuel);
        }

        private void OnFuelChanged(CarEntity car, int fuel)
        {
            var max = car.Entity.DefaultFuel;
            var ratio = max / car.Fuel - .04f;

            gasBarCursor.style.left = Length.Percent(ratio * 100f);
        }

        public override void Dispose()
        {
            if (car != null)
                car.OnFuelChanged -= OnFuelChanged;
        }
    }
}
