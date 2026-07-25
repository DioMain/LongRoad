using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad
{
    public class GameUIManager : LongRoadBehaviour
    {
        public static GameUIManager Instance { get; private set; }

        [SerializeField]
        private UIDocument document;

        [SerializeField]
        private GameUIElement[] elements;

        public VisualElement Root { get; private set; }
        public VisualElement Hud { get; private set; }
        public VisualElement Content { get; private set; }
        public VisualElement Party { get; private set; }
        public VisualElement Inventory { get; private set; }
        public VisualElement Location { get; private set; }
        public VisualElement Overlay { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public override void Init()
        {
            if (document == null)
            {
                Debug.LogError($"{nameof(GameUIManager)}: {nameof(document)} is not assigned.", this);
                return;
            }

            Root = document.rootVisualElement;
            Hud = Root.Q("hud");
            Content = Root.Q("content");
            Party = Root.Q("party");
            Inventory = Root.Q("inventory");
            Location = Root.Q("location");
            Overlay = Root.Q("overlay");

            if (elements == null)
                return;

            for (var i = 0; i < elements.Length; i++)
                elements[i]?.Init();
        }

        public VisualElement GetSlot(string name)
        {
            return Root?.Q(name);
        }
    }
}
