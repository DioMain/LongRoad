using LongRoad.GamePipeline.GamePhaseStrategies;
using LongRoad.Managers;
using LongRoad.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LongRoad.GamePipeline
{
    public enum GameEndState
    {
        None,
        Won,
        Lost
    }

    public class GamePipeline
    {
        private readonly IEnumerable<IGamePhaseStrategy> phases;

        private readonly PersonService _people;
        private readonly GameTimeService _time;
        private readonly TravelService _travel;
        private readonly BackgroundManager _background;
        private readonly GameData _gameData;

        private bool _pipelinePaused;
        public bool PipelinePaused => _pipelinePaused;

        public GameEndState EndState { get; private set; } = GameEndState.None;

        private IGamePhaseStrategy phase = null;
        public IGamePhaseStrategy Phase
        {
            get => phase;
            private set
            {
                phase = value;
                OnPhaseChanged(value);
            }
        }


        public event Action<GameEndState> OnEnded;
        public event Action<IGamePhaseStrategy> OnPhaseChanged;

        public GamePipeline(PersonService people, GameTimeService time, TravelService travel, BackgroundManager background, GameData gameData)
        {
            var phasesTypes = GetType().Assembly.GetTypes().Where(type => typeof(IGamePhaseStrategy).IsAssignableFrom(type) && !type.IsAbstract);
            phases = phasesTypes.Select(type => Activator.CreateInstance(type) as IGamePhaseStrategy).OrderBy(phase => phase.Order);

            foreach (var phase in phases)
            {
                phase.Init();
            }

            _people = people;
            _time = time;
            _travel = travel;
            _background = background;
            _gameData = gameData;
        }

        public IEnumerator Run(MonoBehaviour listener)
        {
            var maxPhaseOrder = phases.Max(phase => phase.Order);
            var minPhaseOrder = phases.Min(phase => phase.Order);

            var currentOrder = minPhaseOrder;

            while (EndState == GameEndState.None)
            {
                Phase = phases.First(phase => phase.Order == currentOrder);
                OnPhaseChanged?.Invoke(Phase);

                yield return phase.Invoke(this, new()
                {
                    GameData = _gameData,
                    Background = _background,
                    People = _people,
                    Time = _time,
                    Travel = _travel
                }, listener);

                if (currentOrder < maxPhaseOrder)
                {
                    currentOrder++;
                }
                else
                {
                    _time?.AdvanceTurn();
                    _travel?.AdvanceTravel();
                    currentOrder = minPhaseOrder;
                }
            }
        }

        public void Continue()
        {
            _pipelinePaused = false;
        }

        public void Pause()
        {
            _pipelinePaused = true;
        }

        public void Win()
        {
            End(GameEndState.Won);
        }

        public void Lose()
        {
            End(GameEndState.Lost);
        }

        private void End(GameEndState state)
        {
            if (EndState != GameEndState.None)
                return;

            EndState = state;
            _pipelinePaused = true;
            OnEnded?.Invoke(state);
        }
    }
}
