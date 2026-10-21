using LongRoad.Core.Scriptables;
using LongRoad.GamePipeline.GamePhaseStrategies;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class LocationUI : LongRoadUIElement
    {
        private VisualElement root;

        public override void Init()
        {
            root = UI.MainRoot.Q("location");

            Local.Travel.OnArrived += Travel_OnArrived;
            Local.Pipeline.OnPhaseChanged += Pipeline_OnPhaseChanged;
        }

        private void Pipeline_OnPhaseChanged(IGamePhaseStrategy phase)
        {
            if (phase is not PlayerGamePhaseStrategy)
            {
                root.style.display = DisplayStyle.None;
            }
        }

        private void Travel_OnArrived(Location loc)
        {
            root.style.display = DisplayStyle.Flex;
        }

        public override void Dispose()
        {
            Local.Travel.OnArrived -= Travel_OnArrived;
            Local.Pipeline.OnPhaseChanged -= Pipeline_OnPhaseChanged;
        }
    }
}
