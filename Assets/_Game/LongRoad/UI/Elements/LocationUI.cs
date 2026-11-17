using LongRoad.Core.Scriptables;
using LongRoad.GamePipeline.GamePhaseStrategies;
using LongRoad.Managers;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class LocationUI : LongRoadUIElement
    {
        private VisualElement root;

        private Button shopButton;
        private Button hospitalButton;
        private Button hotelButton;

        public override void Init()
        {
            root = UI.MainRoot.Q("location");

            shopButton = root.Q<Button>("shop-button");
            hospitalButton = root.Q<Button>("hospital-button");
            hotelButton = root.Q<Button>("hotel-button");

            Local.Travel.OnArrived += Travel_OnArrived;
            Local.Pipeline.OnPhaseChanged += Pipeline_OnPhaseChanged;

            shopButton.clicked += ShopButton_clicked;
            hospitalButton.clicked += HospitalButton_clicked;
            hotelButton.clicked += HotelButton_clicked;
        }

        private void HotelButton_clicked()
        {
            GameManager.Instance.Audio.PlaySfx(AudioManager.CommonSfxClip.Click);
        }

        private void HospitalButton_clicked()
        {
            GameManager.Instance.Audio.PlaySfx(AudioManager.CommonSfxClip.Click);
        }

        private void ShopButton_clicked()
        {
            GameManager.Instance.Audio.PlaySfx(AudioManager.CommonSfxClip.Click);
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

            shopButton.clicked -= ShopButton_clicked;
            hospitalButton.clicked -= HospitalButton_clicked;
            hotelButton.clicked -= HotelButton_clicked;
        }
    }
}
