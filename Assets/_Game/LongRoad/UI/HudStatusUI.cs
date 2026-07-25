using UnityEngine.UIElements;

namespace LongRoad
{
    public class HudStatusUI : GameUIElement
    {
        private Label _turnLabel;
        private Label _dayLabel;
        private Label _periodLabel;
        private Button _goButton;
        private bool _subscribed;

        public override void Init()
        {
            var hud = UI?.Hud;
            if (hud == null)
                return;

            _turnLabel = hud.Q<Label>("turn-label");
            _dayLabel = hud.Q<Label>("day-label");
            _periodLabel = hud.Q<Label>("period-label");
            _goButton = hud.Q<Button>("go-button");

            if (_goButton != null)
            {
                _goButton.text = Game.Localization.GetMainString("hud_go");
                _goButton.clicked += HandleGoClicked;
            }

            Subscribe();
            RefreshLabels();
            RefreshGoButton(GamePhase.Player);
        }

        public override void Dispose()
        {
            Unsubscribe();

            if (_goButton != null)
                _goButton.clicked -= HandleGoClicked;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Subscribe()
        {
            if (_subscribed || Local == null)
                return;

            if (Local.Time != null)
            {
                Local.Time.OnTurnChanged += HandleTurnChanged;
                Local.Time.OnDayChanged += HandleDayChanged;
                Local.Time.OnDayNightChanged += HandleDayNightChanged;
            }

            if (Local.Pipeline != null)
                Local.Pipeline.OnPhaseChanged += HandlePhaseChanged;

            _subscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_subscribed || Local == null)
                return;

            if (Local.Time != null)
            {
                Local.Time.OnTurnChanged -= HandleTurnChanged;
                Local.Time.OnDayChanged -= HandleDayChanged;
                Local.Time.OnDayNightChanged -= HandleDayNightChanged;
            }

            if (Local.Pipeline != null)
                Local.Pipeline.OnPhaseChanged -= HandlePhaseChanged;

            _subscribed = false;
        }

        private void HandleGoClicked()
        {
            Local?.Continue();
        }

        private void HandleTurnChanged(int _)
        {
            RefreshLabels();
        }

        private void HandleDayChanged(int _)
        {
            RefreshLabels();
        }

        private void HandleDayNightChanged(bool _)
        {
            RefreshLabels();
        }

        private void HandlePhaseChanged(GamePhase phase)
        {
            RefreshGoButton(phase);
        }

        private void RefreshLabels()
        {
            var data = Local?.Data;
            if (data == null)
                return;

            var localization = Game.Localization;

            if (_turnLabel != null)
                _turnLabel.text = localization.GetMainString("hud_turn", data.Turn);

            if (_dayLabel != null)
                _dayLabel.text = localization.GetMainString("hud_day", data.Day);

            if (_periodLabel != null)
            {
                _periodLabel.text = data.IsDaytime
                    ? localization.GetMainString("hud_daytime")
                    : localization.GetMainString("hud_night");
            }
        }

        private void RefreshGoButton(GamePhase phase)
        {
            if (_goButton == null)
                return;

            _goButton.SetEnabled(phase == GamePhase.Player);
        }
    }
}
