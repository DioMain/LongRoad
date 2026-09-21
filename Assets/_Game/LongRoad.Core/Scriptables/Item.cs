using LongRoad.Core.Scriptables.Abstractions;
using NaughtyAttributes;
using UnityEngine;

namespace LongRoad.Core.Scriptables
{
    public enum ItemTargetKind
    {
        None, Person, AllPersons
    }

    [CreateAssetMenu(fileName = "Item", menuName = "Entities/Item")]
    public class Item : LongRoadScriptable
    {
        [ShowAssetPreview]
        public Sprite Icon;
        [Space]
        public float Weight = 1f;
        [Space]
        public bool IsFood;
        public float HungerResotre;
        [Space]
        public bool IsMedicine;
        public float MedicineResotre;
        [Space]
        public bool IsFuel;
        public float FuelResotre;
    }
}
