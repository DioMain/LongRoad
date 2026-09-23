using LongRoad.Core.Scriptables.Abstractions;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace LongRoad.Core.Scriptables
{
    [CreateAssetMenu(fileName = "StartPreset", menuName = "Entities/StartPreset")]
    public class GameStartPreset : LongRoadScriptable
    {
        public Person[] StartingPersons;
        public Car SelectedCar;
        public Route StartingRoute;
        public float StartingMoney = 100f;

        [ReorderableList]
        public ItemPresetItem[] StartingItems;
    }

    [Serializable]
    public struct ItemPresetItem
    {
        public Item Item;
        public int Count;
    }
}
