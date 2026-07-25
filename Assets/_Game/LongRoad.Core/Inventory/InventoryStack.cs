using LongRoad.Core.Scriptables;

namespace LongRoad.Core.Inventory
{
    public class InventoryStack
    {
        public Item Item { get; }
        public int Count { get; set; }

        public float TotalWeight => Item != null ? Item.Weight * Count : 0f;

        public InventoryStack(Item item, int count)
        {
            Item = item;
            Count = count;
        }
    }
}
