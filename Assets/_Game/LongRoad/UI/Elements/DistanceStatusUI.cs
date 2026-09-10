using System.Collections.Generic;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class DistanceStatusUI : LongRoadUIElement
    {
        private Label distanceStatus;

        public override void Init()
        {
            distanceStatus = UI.Hud.Q<Label>("reach-distance-text");

            Local.Travel.OnTravelProgress += UpdateDistance;
            UpdateDistance(Local.Data.TravelledKm);
        }

        public void UpdateDistance(float distance)
        {
            distanceStatus.text = Game.Localization.GetMainString("hud_distance_km", new object[] { distance });
        }

        public override void Dispose()
        {
            Local.Travel.OnTravelProgress -= UpdateDistance;
        }
    }
}
