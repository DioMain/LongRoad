using System.Collections;
using UnityEngine;

namespace LongRoad.GamePipeline.GamePhaseStrategies
{
    public class PlayerGamePhaseStrategy : IGamePhaseStrategy
    {
        public int Order => 0;

        public void Init() { }

        public IEnumerator Invoke(GamePipeline pipeline, GamePhaseStrategyServices services, MonoBehaviour listener)
        {
            if (services.GameData.Car.Fuel <= 0 && services.CarModelManager.State != Core.CarModelState.Off)
            {
                LocalManager.Notification.Notify("Двигатель заглох!", UI.Elements.NotificationUI.WarnLevel.Warning); //TODO: Localization

                services.BackgroundManager.StopScroll(2f);

                yield return services.CarModelManager.Model.SetState(Core.CarModelState.Off);

                yield return new WaitForSeconds(3f);
            }

            if (services.GameData.CurrentLocation != null && services.CarModelManager.State != Core.CarModelState.Idle)
            {
                LocalManager.Notification.Notify($"Добро пожаловать в {services.GameData.CurrentLocation.GetName(GameManager.Instance.Localization)}"); //TODO: Localization

                services.BackgroundManager.StopScroll(services.CarModelManager.Model.StopRoadTime);

                yield return services.CarModelManager.Model.SetState(Core.CarModelState.Idle);
            }

            pipeline.Pause();

            yield return new WaitWhile(() => pipeline.PipelinePaused && pipeline.EndState == GameEndState.None);

            if (services.GameData.Car.Fuel > 0 && services.CarModelManager.State != Core.CarModelState.Drive)
            {
                services.BackgroundManager.StartScroll(services.CarModelManager.Model.StartRoadTime);

                yield return services.CarModelManager.Model.SetState(Core.CarModelState.Drive);
            }
        }
    }
}
