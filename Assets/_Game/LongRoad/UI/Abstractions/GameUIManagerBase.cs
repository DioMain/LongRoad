using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI.Abstractions
{
    public abstract class GameUIManagerBase : LongRoadBehaviour
    {
        [SerializeField]
        private PanelRenderer panelRenderer;

        public VisualElement Root { get; private set; }

        public bool Inited { get; private set; } = false;

        public override void Init()
        {
            if (panelRenderer == null)
            {
                Debug.LogError($"{nameof(GameUIManager)}: {nameof(panelRenderer)} is not assigned.", this);
                return;
            }

            panelRenderer.RegisterUIReloadCallback((render, root, version) =>
            {
                if (Inited)
                    return;

                var elements = GetComponentsInChildren<LongRoadUIElement>();

                Root = root;

                OnElementsInit();

                if (elements == null)
                    return;

                foreach (var item in elements)
                {
                    item.Init();
                }

                Inited = true;
            });
        }

        public virtual void OnElementsInit() { }
    }
}
