using LongRoad.Managers;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class GoButtonUI : LongRoadUIElement
    {
        private Button goButton;

        public override void Init()
        {
            goButton = UI.Hud.Q<Button>("go-button");

            Local.Pipeline.OnPause += Pipeline_OnPause;

            goButton.style.display = DisplayStyle.Flex;

            goButton.clicked += GoButton_clicked;
        }

        private void Pipeline_OnPause()
        {
            goButton.style.display = DisplayStyle.Flex;
        }

        private void GoButton_clicked()
        {
            Local.Pipeline.Continue();

            Game.Audio.PlaySfx(AudioManager.CommonSfxClip.Click);

            goButton.style.display = DisplayStyle.None;
        }

        public override void Dispose()
        {
            goButton.clicked -= GoButton_clicked;
            Local.Pipeline.OnPause -= Pipeline_OnPause;
        }
    }
}
