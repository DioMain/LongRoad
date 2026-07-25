using LongRoad.Domain.Interfaces;
using System;

namespace LongRoad.Services
{
    public class MoneyService : IService
    {
        private readonly GameData _data;

        public float Balance => _data.Money;

        public event Action<float> OnMoneyChanged;

        public MoneyService(GameData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public bool CanAfford(float amount)
        {
            return amount >= 0f && _data.Money >= amount;
        }

        public bool TrySpend(float amount)
        {
            if (!CanAfford(amount))
                return false;

            _data.Money -= amount;
            OnMoneyChanged?.Invoke(_data.Money);
            return true;
        }

        public void Add(float amount)
        {
            if (amount <= 0f)
                return;

            _data.Money += amount;
            OnMoneyChanged?.Invoke(_data.Money);
        }
    }
}
