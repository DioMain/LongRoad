using LongRoad.Core;
using LongRoad.Core.Scriptables;
using LongRoad.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace LongRoad.Services
{
    public class LocationService : IService
    {
        private readonly GameData _data;
        private readonly PersonService _people;
        private readonly InventoryService _inventory;
        private readonly TravelService _travel;
        private readonly MoneyService _money;

        private readonly Dictionary<int, int> _shopStock = new();

        public event Action OnChanged;

        public LocationService(
            GameData data,
            PersonService people,
            InventoryService inventory,
            TravelService travel,
            MoneyService money)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _people = people ?? throw new ArgumentNullException(nameof(people));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _travel = travel ?? throw new ArgumentNullException(nameof(travel));
            _money = money ?? throw new ArgumentNullException(nameof(money));

            _travel.OnArrived += HandleArrived;
            _travel.OnDeparted += HandleDeparted;

            if (_data.CurrentLocation != null)
                RefreshShopStock(_data.CurrentLocation);
        }

        public bool TryBuyGas(int units)
        {
            if (!CanInteract() || units <= 0)
                return false;

            var location = _data.CurrentLocation;
            if (!location.HasGasStation)
                return false;

            var cost = location.PriceForGas * units;
            if (_data.Car == null || !_money.TrySpend(cost))
                return false;

            _data.Car.SetFuel(_data.Car.Fuel + units);
            OnChanged?.Invoke();
            return true;
        }

        public bool TryBuyEntertainment()
        {
            if (!CanInteract())
                return false;

            var location = _data.CurrentLocation;
            if (!location.HasEntertainment)
                return false;

            if (!_money.TrySpend(location.PriceForEntertainment))
                return false;

            for (var i = 0; i < _people.People.Count; i++)
            {
                var person = _people.People[i];
                person.SetMood(person.Mood + location.EntertainmentMoodBonus);
            }

            OnChanged?.Invoke();
            return true;
        }

        public bool TryUseHospital(PersonEntity person)
        {
            if (!CanInteract() || person == null)
                return false;

            var location = _data.CurrentLocation;
            if (!location.HasHospital)
                return false;

            if (!_money.TrySpend(location.PriceForHospital))
                return false;

            person.SetHeal(person.Entity.DefaultHeal);
            OnChanged?.Invoke();
            return true;
        }

        public bool TryBuyShopItem(int index, int count)
        {
            if (!CanInteract() || count <= 0)
                return false;

            var location = _data.CurrentLocation;
            if (index < 0 || index >= location.ShopItems.Count)
                return false;

            var offer = location.ShopItems[index];
            if (offer.Item == null)
                return false;

            if (!_shopStock.TryGetValue(index, out var stock) || stock < count)
                return false;

            var cost = offer.Price * count;
            if (!_money.CanAfford(cost) || !_inventory.CanAdd(offer.Item, count))
                return false;

            if (!_money.TrySpend(cost))
                return false;

            if (!_inventory.TryAdd(offer.Item, count))
            {
                _money.Add(cost);
                return false;
            }

            _shopStock[index] = stock - count;
            OnChanged?.Invoke();
            return true;
        }

        public bool LeaveLocation()
        {
            if (_data.CurrentLocation == null)
                return false;

            _travel.LeaveLocation();
            return true;
        }

        public int GetShopStock(int index)
        {
            return _shopStock.TryGetValue(index, out var stock) ? stock : 0;
        }

        private bool CanInteract()
        {
            return _data.CurrentLocation != null;
        }

        private void HandleArrived(Location location)
        {
            RefreshShopStock(location);
            OnChanged?.Invoke();
        }

        private void HandleDeparted(Location _)
        {
            _shopStock.Clear();
            OnChanged?.Invoke();
        }

        private void RefreshShopStock(Location location)
        {
            _shopStock.Clear();
            if (location?.ShopItems == null)
                return;

            for (var i = 0; i < location.ShopItems.Count; i++)
                _shopStock[i] = location.ShopItems[i].Limit;
        }
    }
}
