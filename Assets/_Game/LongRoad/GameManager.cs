using LongRoad.Core;
using LongRoad.Core.GameEvent;
using LongRoad.Core.Localization;
using LongRoad.Core.Scriptables;
using LongRoad.Domain.Interfaces;
using LongRoad.Managers;
using UnityEngine;

namespace LongRoad {
    public class GameManager : MonoBehaviour, IInit
    {
        public static GameManager Instance;

        public GameStartPreset StartPreset;

        [SerializeField]
        private LocalizationManager localization;
        [SerializeField]
        private AudioManager audioManager;

        public PlayerInput Input { get; private set; }
        public LocalizationManager Localization => localization;
        public ItemService ItemService { get; private set; }
        public AudioManager Audio => audioManager;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);

                Init();
            }
            else
            {
                Destroy(this);
            }
        }

        public void Init()
        {
            BoundEventCatalog.EnsureBuilt();

            ItemService = new ItemService();
            Input = new PlayerInput();

            localization.Init();
            audioManager.Init();
        }
    }
}