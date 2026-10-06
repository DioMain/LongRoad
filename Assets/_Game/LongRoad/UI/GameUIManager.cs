using LongRoad.UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI
{
    public class GameUIManager : LongRoadBehaviour
    {
        [SerializeField]
        private PanelRenderer panelRenderer;

        public VisualElement Root { get; private set; }

        public VisualElement Hud { get; private set; }
        public VisualElement Inventory { get; private set; }
        public VisualElement Dialog { get; private set; }

        public PersonSelectorUI PersonSelector;
        public DialogUI DialogUI;


        bool uiInited = false;

        public override void Init()
        {
            if (panelRenderer == null)
            {
                Debug.LogError($"{nameof(GameUIManager)}: {nameof(panelRenderer)} is not assigned.", this);
                return;
            }

            panelRenderer.RegisterUIReloadCallback((render, root, version) =>
            {
                if (uiInited)
                    return;

                var elements = GetComponentsInChildren<LongRoadUIElement>();

                Root = root;

                Hud = Root.Q("hud");
                Inventory = Root.Q("inventory");
                Dialog = Root.Q("dialog");

                if (elements == null)
                    return;

                foreach (var item in elements)
                {
                    item.Init();
                }

                uiInited = true;
            });
        }
    }
}
