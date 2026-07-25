using LongRoad.Core.Scriptables.Abstractions;
using UnityEngine;

namespace LongRoad.Core.Scriptables
{
    [CreateAssetMenu(fileName = "Item", menuName = "Entities/Item")]
    public class Item : LongRoadScriptable
    {
        public float Weight = 1f;
        public Sprite Icon;
    }
}
