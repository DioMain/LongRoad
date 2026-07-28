using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI
{
    public class GameUIManager : LongRoadBehaviour
    {
        [SerializeField]
        private UIDocument document;

        public VisualElement Root { get; private set; }

        public VisualElement Hud { get; private set; }
        public VisualElement Inventory { get; private set; }
        public VisualElement Dialog { get; private set; }

        public override void Init()
        {
            if (document == null)
            {
                Debug.LogError($"{nameof(GameUIManager)}: {nameof(document)} is not assigned.", this);
                return;
            }

            var elements = GetComponentsInChildren<LongRoadUIElement>();

            Root = document.rootVisualElement;
            
            Hud = Root.Q("hud");
            Inventory = Root.Q("inventory");
            Dialog = Root.Q("dialog");

            if (elements == null)
                return;

            foreach (var item in elements)
            {
                item.Init();
            }
        }
    }
}
