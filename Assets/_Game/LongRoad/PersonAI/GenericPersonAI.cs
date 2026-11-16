using LongRoad.Core;
using LongRoad.Core.Localization;
using LongRoad.Core.PersonAI.Abstractions;
using LongRoad.UI.Elements;
using System.Collections;

namespace LongRoad.PersonAI
{
    public class GenericPersonAI : PersonAIBase
    {
        public NotificationUI Notification => LocalManager.Notification;
        public LocalizationManager Localization => GameManager.L;

        public GenericPersonAI(PersonEntity currentPerson) : base(currentPerson)
        {
        }

        public override IEnumerator Think(GameData gameData)
        {
            Notification.Notify($"{Person.Prototype.GetName(Localization)} подумал!");

            yield break;
        }
    }
}
