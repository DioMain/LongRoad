using LongRoad.Core;
using LongRoad.Domain.Interfaces;
using UnityEngine;

namespace LongRoad {
    public class GameManager : MonoBehaviour, IInit
    {
        public static GameManager Instance;

        public PlayerInput Input { get; private set; }

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