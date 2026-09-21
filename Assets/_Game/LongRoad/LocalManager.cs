using LongRoad.Core;
using LongRoad.Core.Inventory;
using LongRoad.Core.Scriptables;
using LongRoad.Domain.Interfaces;
using LongRoad.Services;
using LongRoad.UI;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace LongRoad
{
    [Serializable]
    public struct ItemPresetItem
    {
        public Item Item;
        public int Count;
    }


    public class LocalManager : MonoBehaviour, IInit
    {
        public static LocalManager Instance;

        [SerializeField]
        private Person startingPerson;
        [SerializeField]
        private Car selectedCar;
        [SerializeField]
        private Route startingRoute;
        [SerializeField]
        private float startingMoney = 100f;
        [SerializeField]
        [ReorderableList]
        private ItemPresetItem[] startingItems;
        [Space]
        [SerializeField]
        private GameUIManager ui;
        [SerializeField]
        private CarModelManager carModel;

        public GameData Data { get; private set; }
        public GamePipeline Pipeline { get; private set; }
        public PersonService Persons { get; private set; }
        public InventoryService Inventory { get; private set; }
        public GameTimeService Time { get; private set; }
        public TravelService Travel { get; private set; }
        public Services.LocationService Locations { get; private set; }
        public MoneyService Money { get; private set; }
        public GameUIManager UI => ui;
        public CarModelManager CarModel => carModel;

        private void Start()
        {
            Instance = this;

            Init();
        }

        public void Init()
        {
            Data = new GameData
            {
                Money = startingMoney,
                Route = startingRoute
            };

            if (selectedCar != null)
                Data.Car = new CarEntity(selectedCar);

            Persons = new PersonService();
            if (startingPerson != null)
                Persons.LoadRoster(new[] { startingPerson });

            Inventory = new InventoryService(Data);
            Time = new GameTimeService(Data);
            Travel = new TravelService(Data);
            Money = new MoneyService(Data);
            Locations = new Services.LocationService(Data, Persons, Inventory, Travel, Money);

            foreach (var item in startingItems)
            {
                Inventory.TryAdd(item.Item, item.Count);
            }

            Pipeline = new GamePipeline(Persons, Time, Travel);

            carModel.Init();

            ui.Init();

            StartCoroutine(Pipeline.Run(this));
        }

        public void Continue()
        {
            Pipeline?.Continue();
        }

        public void Win()
        {
            Pipeline?.Win();
        }

        public void Lose()
        {
            Pipeline?.Lose();
        }
    }
}
