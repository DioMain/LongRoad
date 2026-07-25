using System;
using UnityEngine;

namespace LongRoad.Core.GameEvent
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class UseGameEventAttribute : Attribute
    {
        public float Probability { get; }

        public UseGameEventAttribute(float probability = 1f)
        {
            Probability = Mathf.Clamp01(probability);
        }
    }
}
