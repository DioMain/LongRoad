using LongRoad.Core;
using LongRoad.Core.Scriptables;

namespace LongRoad
{
    /// <summary>
    /// Session data store only. Logic and UI notifications live in services / GamePipeline.
    /// </summary>
    public class GameData
    {
        public CarEntity Car { get; set; }

        public int Turn { get; set; }

        public int Day { get; set; } = 1;

        public bool IsDaytime { get; set; } = true;

        public float Money { get; set; }

        public float TravelledKm { get; set; }

        public Location CurrentLocation { get; set; }

        public Route Route { get; set; }
    }
}
