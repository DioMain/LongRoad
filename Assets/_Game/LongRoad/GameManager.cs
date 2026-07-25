using LongRoad.Core;
using LongRoad.Core.Localization;
using LongRoad.Domain.Interfaces;
using UnityEngine;

namespace LongRoad {
    public class GameManager : MonoBehaviour, IInit
    {
        public static GameManager Instance;

        [SerializeField]
        private LocalizationManager localization;

        public PlayerInput Input { get; private set; }
        public LocalizationManager Localization => localization;

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
            Input = new PlayerInput();
        }
    }
}