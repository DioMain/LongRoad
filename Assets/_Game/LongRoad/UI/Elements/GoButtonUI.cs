using LongRoad.GamePipeline;
using LongRoad.GamePipeline.GamePhaseStrategies;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class GoButtonUI : LongRoadUIElement
    {
        private Button goButton;

        public override void Init()
        {
            goButton = UI.Hud.Q<Button>("go-button");

            Local.Pipeline.OnPhaseChanged += Pipeline_OnPhaseChanged;

            goButton.style.display = DisplayStyle.Flex;

            goButton.clicked += GoButton_clicked;
        }

        private void Pipeline_OnPhaseChanged(IGamePhaseStrategy phase)
        {
            if (phase is PlayerGamePhaseStrategy)
            {
                goButton.style.display = DisplayStyle.Flex;
            }
            else
            {
                goButton.style.display = DisplayStyle.None;
            }
        }

        private void GoButton_clicked()
        {
            Local.Pipeline.Continue();
        }

        public override void Dispose()
        {
            goButton.clicked -= GoButton_clicked;
            Local.Pipeline.OnPhaseChanged -= Pipeline_OnPhaseChanged;
        }
    }
}
