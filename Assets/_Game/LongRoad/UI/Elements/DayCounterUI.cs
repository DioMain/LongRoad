using UnityEngine;
using UnityEngine.UIElements;

namespace LongRoad.UI.Elements
{
    public class DayCounterUI : LongRoadUIElement
    {
        [SerializeField]
        private Sprite[] calendarSprites;

        private VisualElement dayContent;

        private Image dayImage;
        private Label dayCounter;
        private Label dayOfWeek;

        public override void Init()
        {
            dayContent = UI.Hud.Q("day-content");

            dayImage = dayContent.Q<Image>("day-image");
            dayCounter = dayContent.Q<Label>("day-text");
            dayOfWeek = dayContent.Q<Label>("week-day-text");

            Local.Time.OnDayChanged += OnDayChanged;
            OnDayChanged(1);
        }

        public void OnDayChanged(int day)
        {
            var calendar = calendarSprites[Random.Range(0, calendarSprites.Length)];
            var dayText = Game.Localization.GetMainString("hud_day", new[] { day.ToString() });
            var weekDayText = Game.Localization.GetMainString(GetWeekDayKey(day));

            dayImage.sprite = calendar;

            dayCounter.text = dayText; 
            dayOfWeek.text = weekDayText;
        }

        public string GetWeekDayKey(int day)
        {
            return (day - 1 % 7) switch
            {
                0 => "weakday_monday",
                1 => "weakday_tuesday",
                2 => "weakday_wednesday",
                3 => "weakday_thursday",
                4 => "weakday_friday",
                5 => "weakday_saturday",
                6 => "weakday_sunday",
                _ => "unknown_weekday"
            };
        }

        public override void Dispose()
        {
            Local.Time.OnDayChanged -= OnDayChanged;
        }
    }
}
