using LongRoad.Core.Scriptables.Abstractions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LongRoad.Core.Scriptables
{
    [CreateAssetMenu(fileName = "Location", menuName = "Entities/Location")]
    public class Location : LongRoadScriptable
    {
        public GameObject Background;

        public bool HasGasStation;
        public int PriceForGas;

        public bool HasEntertainment;
        public int PriceForEntertainment;
        public int EntertainmentMoodBonus = 10;

        public bool HasHospital;
        public int PriceForHospital;

        public List<LocationShopItem> ShopItems = new();
    }

    [Serializable]
    public struct LocationShopItem
    {
        public int Price;
        public int Limit;
        public Item Item;
    }
}
