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
                LocalManager.Notification.Notify(GameManager.L.GetMainString("notification_fuel_is_gone"), UI.Elements.NotificationUI.WarnLevel.Warning);

                services.BackgroundManager.StopScroll(3f);

                yield return services.CarModelManager.Model.SetState(Core.CarModelState.Off);

                yield return new WaitForSeconds(3f);
            }

            if (services.GameData.CurrentLocation != null && services.CarModelManager.State != Core.CarModelState.Idle)
            {
                LocalManager.Notification.Notify(GameManager.L.GetMainString("notification_appear_to_location", services.GameData.CurrentLocation.GetName(GameManager.L)));

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
