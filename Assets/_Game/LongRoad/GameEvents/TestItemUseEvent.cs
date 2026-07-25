using LongRoad.Core.GameEvent;
using System.Collections;
using UnityEngine;

namespace LongRoad.GameEvents
{
    [BoundGameEvent(BoundGameEventKind.Item, "test_item")]
    public class TestItemUseEvent : ContextualGameEventBase
    {
        public override IEnumerator Event()
        {
            var targetInfo = HasTarget ? Target.Entity.Tag : "none";
            var sourceInfo = Source != null ? Source.Entity.Tag : "none";
            Debug.Log($"TestItemUseEvent: used test_item (source={sourceInfo}, target={targetInfo})");

            yield return new WaitForSeconds(0.5f);

            Debug.Log("TestItemUseEvent: ended");
        }
    }
}
