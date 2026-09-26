using LongRoad.Core.GameEvent.Abstractions;
using LongRoad.UI.Elements;
using System.Collections;
using UnityEngine;

namespace LongRoad.GameEvents
{
    [UseGameEvent]
    public class TestEvent : GameEventBase
    {
        public override IEnumerator Event()
        {
            var dialogUI = LocalManager.Instance.UI.DialogUI;

            yield return dialogUI.StartDialog("event_test_dialog_test", 
                DialogUIConstants.YES_VARIANT, DialogUIConstants.NO_VARIANT);

            if (dialogUI.SelectResult.Value.Tag == DialogUIConstants.YES_VARIANT.Tag)
            {
                Debug.Log("Selected 'yes' variant!");
            }
            else if (dialogUI.SelectResult.Value.Tag == DialogUIConstants.NO_VARIANT.Tag)
            {
                Debug.Log("Selected 'no' variant!");
            }
        }
    }
}
