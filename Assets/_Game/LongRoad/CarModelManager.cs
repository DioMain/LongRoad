using LongRoad.Core;
using LongRoad.Core.Scriptables;
using UnityEngine;

namespace LongRoad
{
    public class CarModelManager : LongRoadBehaviour
    {
        [SerializeField]
        private Transform CarContainer;

        private CarEntity Car => Local?.Data?.Car;

        private CarModel model;
        private GamePhase _phase = GamePhase.Player;
        private bool _subscribed;

        public CarModel Model => model;

        public CarModelState State => model != null ? model.State : CarModelState.Off;

        public override void Init()
        {
            if (Car == null)
            {
                Debug.LogError($"{nameof(CarModelManager)}: no car in session data.", this);
                return;
            }

            model = Car.SpawnModel(CarContainer);
            Subscribe();
            RefreshState();
        }

        public void SetState(CarModelState state)
        {
            model?.SetState(state);
        }

        public void RefreshState()
        {
            if (model == null || Local?.Data == null)
                return;

            var car = Car;
            if (car == null || car.Fuel <= 0)
            {
                SetState(CarModelState.Off);
                return;
            }

            if (_phase == GamePhase.Player || Local.Data.CurrentLocation != null)
            {
                SetState(CarModelState.Idle);
                return;
            }

            if (_phase == GamePhase.Modifiers || _phase == GamePhase.Event)
            {
                SetState(CarModelState.Drive);
                return;
            }

            SetState(CarModelState.Idle);
        }

        public override void Dispose()
        {
            Unsubscribe();
            model?.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Subscribe()
        {
            if (_subscribed)
                return;

            if (Local?.Pipeline != null)
                Local.Pipeline.OnPhaseChanged += HandlePhaseChanged;

            if (Local?.Travel != null)
            {
                Local.Travel.OnArrived += HandleArrived;
                Local.Travel.OnDeparted += HandleDeparted;
            }

            if (Car != null)
                Car.OnFuelChanged += HandleFuelChanged;

            _subscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_subscribed)
                return;

            if (Local?.Pipeline != null)
                Local.Pipeline.OnPhaseChanged -= HandlePhaseChanged;

            if (Local?.Travel != null)
            {
                Local.Travel.OnArrived -= HandleArrived;
                Local.Travel.OnDeparted -= HandleDeparted;
            }

            if (Car != null)
                Car.OnFuelChanged -= HandleFuelChanged;

            _subscribed = false;
        }

        private void HandlePhaseChanged(GamePhase phase)
        {
            _phase = phase;
            RefreshState();
        }

        private void HandleArrived(Location _)
        {
            RefreshState();
        }

        private void HandleDeparted(Location _)
        {
            RefreshState();
        }

        private void HandleFuelChanged(CarEntity _, int __)
        {
            RefreshState();
        }
    }
}
