using LongRoad.Domain.Interfaces;
using UnityEngine;

namespace LongRoad
{
    public class LocalManager : MonoBehaviour, IInit
    {
        public static LocalManager Instance;

        private void Start()
        {
            Instance = this;

            Init();
        }

        public void Init()
        {
            throw new System.NotImplementedException();
        }
    }
}
