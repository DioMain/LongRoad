using LongRoad.Core.Scriptables;
using System.Collections.Generic;

namespace LongRoad.Core.Inventory
{
    public class Inventory
    {
        private const float WeightEpsilon = 0.0001f;

        private readonly List<InventoryStack> _stacks = new();

        public IReadOnlyList<InventoryStack> Stacks => _stacks;

        public float CurrentWeight
        {
            get
            {
                var weight = 0f;
                for (var i = 0; i < _stacks.Count; i++)
                    weight += _stacks[i].TotalWeight;
                return weight;
            }
        }

        public bool CanAdd(Item item, int count, float maxWeight)
        {
            if (item == null || count <= 0)
                return false;

            return CurrentWeight + item.Weight * count <= maxWeight + WeightEpsilon;
        }

        public bool TryAdd(Item item, int count, float maxWeight)
        {
            if (!CanAdd(item, count, maxWeight))
                return false;

            var stack = FindStack(item.Tag);
            if (stack != null)
                stack.Count += count;
            else
                _stacks.Add(new InventoryStack(item, count));

            return true;
        }

        public bool TryRemove(Item item, int count)
        {
            if (item == null)
                return false;

            return TryRemove(item.Tag, count);
        }

        public bool TryRemove(string tag, int count)
        {
            if (string.IsNullOrEmpty(tag) || count <= 0)
                return false;

            var stack = FindStack(tag);
            if (stack == null || stack.Count < count)
                return false;

            stack.Count -= count;
            if (stack.Count == 0)
                _stacks.Remove(stack);

            return true;
        }

        public int GetCount(Item item)
        {
            return item == null ? 0 : GetCount(item.Tag);
        }

        public int GetCount(string tag)
        {
            var stack = FindStack(tag);
            return stack?.Count ?? 0;
        }

        private InventoryStack FindStack(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return null;

            for (var i = 0; i < _stacks.Count; i++)
            {
                if (_stacks[i].Item != null && _stacks[i].Item.Tag == tag)
                    return _stacks[i];
            }

            return null;
        }
    }
}
