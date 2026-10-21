using LongRoad.Core.Scriptables;
using LongRoad.Domain.Interfaces;
using System.Linq;
using UnityEngine;

namespace LongRoad.Core
{
    public class ItemService : IService
    {
        public Item[] Items { get; private set; }

        public ItemService()
        {
            Items = Resources.LoadAll<Item>("Items");
        }

        public Item GetItemByTag(string tag)
        {
            return Items.FirstOrDefault(i => i.Tag == tag);
        }
    }
}
