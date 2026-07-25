using LongRoad.Domain.Interfaces;
using System;
using UnityEngine;

namespace LongRoad.Core {
    public abstract class LongRoadBehaviourCore : MonoBehaviour, IInit, IDisposable
    {
        public virtual void Init() { }
        public virtual void Dispose() { }
    }
}