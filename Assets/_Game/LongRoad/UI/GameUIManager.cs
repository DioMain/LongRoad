using LongRoad.UI.Abstractions;
using LongRoad.UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI
{
    public class GameUIManager : GameUIManagerBase
    {
        public VisualElement MainRoot => Root;

        public VisualElement Hud { get; private set; }
        public VisualElement Inventory { get; private set; }
        public VisualElement Dialog { get; private set; }

        public PersonSelectorUI PersonSelector;
        public DialogUI DialogUI;
        public NotificationUI Notification;

        public override void OnElementsInit()
        {
            Hud = MainRoot.Q("hud");
            Inventory = MainRoot.Q("inventory");
            Dialog = MainRoot.Q("dialog");
        }

        public void Hide()
        {

        }

        public void Show()
        {

        }
    }
}
