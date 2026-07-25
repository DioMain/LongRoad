using LongRoad.Core.Scriptables;
using LongRoad.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LongRoad.Services
{
    public class TravelService : IService
    {
        private readonly GameData _data;
        private readonly List<RouteStop> _stops;
        private int _nextStopIndex;

        public event Action<float> OnTravelProgress;
        public event Action<Location> OnArrived;
        public event Action<Location> OnDeparted;

        public TravelService(GameData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _nextStopIndex = 0;

            if (_data.Route?.Stops == null)
            {
                _stops = new List<RouteStop>();
                return;
            }

            _stops = _data.Route.Stops
                .Where(s => s.Location != null)
                .OrderBy(s => s.DistanceFromStartKm)
                .ToList();
        }

        public void AdvanceTravel()
        {
            if (_data.Route == null || _data.Car == null)
                return;

            if (_data.CurrentLocation != null)
                return;

            if (_data.Car.Fuel <= 0)
                return;

            _data.TravelledKm += _data.Car.DistancePerTurn;
            _data.Car.SetFuel(_data.Car.Fuel - _data.Car.FuelConsumption);
            OnTravelProgress?.Invoke(_data.TravelledKm);

            TryArriveAtReachedStops();
        }

        public void LeaveLocation()
        {
            if (_data.CurrentLocation == null)
                return;

            var left = _data.CurrentLocation;
            _data.CurrentLocation = null;
            OnDeparted?.Invoke(left);
        }

        private void TryArriveAtReachedStops()
        {
            while (_nextStopIndex < _stops.Count)
            {
                var stop = _stops[_nextStopIndex];
                if (_data.TravelledKm < stop.DistanceFromStartKm)
                    break;

                _nextStopIndex++;
                _data.CurrentLocation = stop.Location;
                OnArrived?.Invoke(stop.Location);
                break;
            }
        }
    }
}
