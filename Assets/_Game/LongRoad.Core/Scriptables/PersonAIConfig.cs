using System;
using UnityEngine;

namespace LongRoad.Core.Scriptables
{
    [CreateAssetMenu(fileName = "AIConfig", menuName = "Entities/AIConfig")]
    public class PersonAIConfig : ScriptableObject
    {
        public string AITypeName;

        public float Kindness;
        public float Generosity;
        public float Bravery;
        public float Patience;
    }
}
