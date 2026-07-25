using LongRoad.Domain.Interfaces;
using System;

namespace LongRoad.Services
{
    public class GameTimeService : IService
    {
        public const int TurnsPerPeriod = 3;

        private readonly GameData _data;

        public event Action<int> OnTurnChanged;
        public event Action<bool> OnDayNightChanged;
        public event Action<int> OnDayChanged;

        public GameTimeService(GameData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));

            if (_data.Day <= 0)
                _data.Day = 1;
        }

        public void AdvanceTurn()
        {
            _data.Turn++;
            OnTurnChanged?.Invoke(_data.Turn);

            if (_data.Turn % TurnsPerPeriod != 0)
                return;

            _data.IsDaytime = !_data.IsDaytime;
            OnDayNightChanged?.Invoke(_data.IsDaytime);

            if (!_data.IsDaytime)
                return;

            _data.Day++;
            OnDayChanged?.Invoke(_data.Day);
        }
    }
}
