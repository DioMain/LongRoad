using LongRoad.Core.GameEvent.Abstractions;
using System.Collections;
using UnityEngine;

namespace LongRoad.GameEvents
{
    [UseGameEvent(0.35f)]
    public class SupplyFindEvent : GameEventBase
    {
        public override IEnumerator Event()
        {
            int aidAmount = Random.Range(1, 3);

            LocalManager.Instance.UI.Notification.Notify(@$"Finded {aidAmount} <sprite name=""aid""> aids!");

            LocalManager.Instance.Inventory.TryAdd(GameManager.Instance.ItemService.GetItemByTag("big_medicine"), aidAmount);

            yield return new WaitForSeconds(1);
        }
    }
}
