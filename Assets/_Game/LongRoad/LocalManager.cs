using LongRoad.Core;
using LongRoad.Core.Scriptables;
using LongRoad.Domain.Interfaces;
using LongRoad.Services;
using UnityEngine;

namespace LongRoad
{
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
        private GameUIManager ui;

        public GameData Data { get; private set; }
        public GamePipeline Pipeline { get; private set; }
        public PersonService People { get; private set; }
        public InventoryService Inventory { get; private set; }
        public GameTimeService Time { get; private set; }
        public TravelService Travel { get; private set; }
        public Services.LocationService Locations { get; private set; }
        public MoneyService Money { get; private set; }
        public GameUIManager UI => ui;

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

            People = new PersonService();
            if (startingPerson != null)
                People.LoadRoster(new[] { startingPerson });

            Inventory = new InventoryService(Data);
            Time = new GameTimeService(Data);
            Travel = new TravelService(Data);
            Money = new MoneyService(Data);
            Locations = new Services.LocationService(Data, People, Inventory, Travel, Money);

            Pipeline = new GamePipeline(People, Time, Travel);
            StartCoroutine(Pipeline.Run(this));

            ui?.Init();
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
