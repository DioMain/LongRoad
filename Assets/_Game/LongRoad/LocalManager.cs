using LongRoad.Core;
using LongRoad.Domain.Interfaces;
using LongRoad.Services;
using LongRoad.UI;
using UnityEngine;
using LongRoad.Managers;
using LongRoad.UI.Elements;

namespace LongRoad
{
    public class LocalManager : MonoBehaviour, IInit
    {
        public static LocalManager Instance;

        [SerializeField]
        private GameUIManager ui;
        [SerializeField]
        private CarModelManager carModel;
        [SerializeField]
        private BackgroundManager background;

        public GameData Data { get; private set; }
        public GamePipeline.GamePipeline Pipeline { get; private set; }
        public PersonService Persons { get; private set; }
        public InventoryService Inventory { get; private set; }
        public GameTimeService Time { get; private set; }
        public TravelService Travel { get; private set; }
        public Services.LocationService Locations { get; private set; }
        public MoneyService Money { get; private set; }
        public GameUIManager UI => ui;
        public CarModelManager CarModel => carModel;
        public BackgroundManager Background => background;

        public static NotificationUI Notification => Instance.UI.Notification;

        private void Start()
        {
            Instance = this;

            Init();
        }

        public void Init()
        {
            var preset = GameManager.Instance.StartPreset;

            Data = new GameData
            {
                Money = preset.StartingMoney,
                Route = preset.StartingRoute
            };

            if (preset.SelectedCar != null)
                Data.Car = new CarEntity(preset.SelectedCar);

            Persons = new PersonService();
            if (preset.StartingPersons.Length > 0)
                Persons.LoadRoster(preset.StartingPersons);

            Time = new GameTimeService(Data);
            Travel = new TravelService(Data);
            Money = new MoneyService(Data);
            Inventory = new InventoryService(Data, Persons);
            Locations = new Services.LocationService(Data, Persons, Inventory, Travel, Money);

            foreach (var item in preset.StartingItems)
            {
                Inventory.TryAdd(item.Item, item.Count);
            }

            background.Init();
            carModel.Init();

            Pipeline = new GamePipeline.GamePipeline(Persons, Time, Travel, background, carModel, Data);

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
