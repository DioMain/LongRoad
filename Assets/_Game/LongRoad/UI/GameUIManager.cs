using LongRoad.UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI
{
    public class GameUIManager : LongRoadBehaviour
    {
        [SerializeField]
        private PanelRenderer mainPanelRenderer;

        public VisualElement MainRoot { get; private set; }

        public VisualElement Hud { get; private set; }
        public VisualElement Inventory { get; private set; }
        public VisualElement Dialog { get; private set; }

        public PersonSelectorUI PersonSelector;
        public DialogUI DialogUI;
        public NotificationUI Notification;

        bool mainUiInited = false;

        public override void Init()
        {
            if (mainPanelRenderer == null)
            {
                Debug.LogError($"{nameof(GameUIManager)}: {nameof(mainPanelRenderer)} is not assigned.", this);
                return;
            }

            mainPanelRenderer.RegisterUIReloadCallback((render, root, version) =>
            {
                if (mainUiInited)
                    return;

                var elements = GetComponentsInChildren<LongRoadUIElement>();

                MainRoot = root;

                Hud = MainRoot.Q("hud");
                Inventory = MainRoot.Q("inventory");
                Dialog = MainRoot.Q("dialog");

                if (elements == null)
                    return;

                foreach (var item in elements)
                {
                    item.Init();
                }

                mainUiInited = true;
            });
        }
    }
}
