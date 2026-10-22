using LongRoad.Core.Scriptables;
using LongRoad.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LongRoad.Services
{
    public class TravelService : IService
    {
        private readonly GameData _data;
        private readonly List<Route.RouteStop> _stops;

        public event Action<float> OnTravelProgress;
        public event Action<Location> OnArrived;
        public event Action<Location> OnDeparted;
        public event Action OnTranslateToEndlessMode;

        private int nextStopIndex;
        private float distanceToNextStop;

        public TravelService(GameData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            nextStopIndex = 0;

            if (_data.Route?.Stops == null)
            {
                _stops = new List<Route.RouteStop>();
                SetEndlessMode();
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

            if (_data.Car.Fuel <= 0)
                return;

            if (_data.CurrentLocation != null)
                LeaveLocation();

            var reachedDistance = Mathf.RoundToInt(_data.Car.DistancePerTurn * UnityEngine.Random.Range(0.75f, 1.25f));

            _data.TravelledKm += reachedDistance;
            distanceToNextStop -= reachedDistance;
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

        public void SetEndlessMode()
        {
            _data.IsEndlessMode = true;

            distanceToNextStop = 
                UnityEngine.Random.Range(_data.Route.MinAvarangeStopDistance, _data.Route.MaxAvarangeStopDistance); 

            OnTranslateToEndlessMode?.Invoke();
        }

        private void TryArriveAtReachedStops()
        {
            if (_data.IsEndlessMode)
            {
                if (distanceToNextStop <= 0)
                {
                    distanceToNextStop =
                        UnityEngine.Random.Range(_data.Route.MinAvarangeStopDistance, _data.Route.MaxAvarangeStopDistance);

                    Route.EndlessModeRouteStop stop;
                    while (true)
                    {
                        int locId = UnityEngine.Random.Range(0, _data.Route.EndlessModeStops.Count);

                        stop = _data.Route.EndlessModeStops[locId];

                        float probability = UnityEngine.Random.Range(0f, 1f);
                        if (probability < stop.Probability)
                        {
                            _data.CurrentLocation = stop.Location;
                            OnArrived?.Invoke(stop.Location);
                            break;
                        }
                    }
                }
            }
            else
            {
                while (nextStopIndex < _stops.Count)
                {
                    var stop = _stops[nextStopIndex];
                    if (_data.TravelledKm < stop.DistanceFromStartKm)
                        break;

                    nextStopIndex++;
                    _data.CurrentLocation = stop.Location;
                    OnArrived?.Invoke(stop.Location);

                    if (_stops.Count >= nextStopIndex)
                    {
                        SetEndlessMode();
                    }

                    break;
                }
            }
        }
    }
}
