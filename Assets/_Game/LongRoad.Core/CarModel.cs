using DG.Tweening;
using UnityEngine;

namespace LongRoad.Core
{
    public enum CarModelState
    {
        Off,
        Idle,
        Drive
    }

    public class CarModel : LongRoadBehaviourCore
    {
        [SerializeField]
        private Transform body;

        [SerializeField]
        private Transform wheelFront;

        [SerializeField]
        private Transform wheelRear;

        [SerializeField]
        private float idleBodyStrength = 0.02f;

        [SerializeField]
        private float driveBodyStrength = 0.035f;

        [SerializeField]
        private float wheelShakeStrength = 0.04f;

        [SerializeField]
        private float idleDuration = 0.08f;

        [SerializeField]
        private float driveDuration = 0.06f;

        private Vector3 _bodyRestPos;
        private Quaternion _bodyRestRot;
        private Vector3 _wheelFrontRestPos;
        private Quaternion _wheelFrontRestRot;
        private Vector3 _wheelRearRestPos;
        private Quaternion _wheelRearRestRot;

        public CarModelState State { get; private set; } = CarModelState.Off;

        public override void Init()
        {
            CacheRests();
            SetState(CarModelState.Off);
        }

        public void SetState(CarModelState state)
        {
            if (State == state)
                return;

            State = state;
            KillTweens();
            ResetTransforms();

            switch (state)
            {
                case CarModelState.Idle:
                    PlayIdle();
                    break;
                case CarModelState.Drive:
                    PlayDrive();
                    break;
            }
        }

        public override void Dispose()
        {
            KillTweens();
            ResetTransforms();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void CacheRests()
        {
            if (body != null)
            {
                _bodyRestPos = body.localPosition;
                _bodyRestRot = body.localRotation;
            }

            if (wheelFront != null)
            {
                _wheelFrontRestPos = wheelFront.localPosition;
                _wheelFrontRestRot = wheelFront.localRotation;
            }

            if (wheelRear != null)
            {
                _wheelRearRestPos = wheelRear.localPosition;
                _wheelRearRestRot = wheelRear.localRotation;
            }
        }

        private void KillTweens()
        {
            if (body != null)
                body.DOKill();
            if (wheelFront != null)
                wheelFront.DOKill();
            if (wheelRear != null)
                wheelRear.DOKill();
        }

        private void ResetTransforms()
        {
            if (body != null)
            {
                body.localPosition = _bodyRestPos;
                body.localRotation = _bodyRestRot;
            }

            if (wheelFront != null)
            {
                wheelFront.localPosition = _wheelFrontRestPos;
                wheelFront.localRotation = _wheelFrontRestRot;
            }

            if (wheelRear != null)
            {
                wheelRear.localPosition = _wheelRearRestPos;
                wheelRear.localRotation = _wheelRearRestRot;
            }
        }

        private void PlayIdle()
        {
            if (body == null)
                return;

            body.DOShakePosition(idleDuration, idleBodyStrength, 10, 90f, false, true)
                .SetLoops(-1, LoopType.Restart)
                .SetUpdate(true);
        }

        private void PlayDrive()
        {
            if (body != null)
            {
                body.DOShakePosition(driveDuration, driveBodyStrength, 12, 90f, false, true)
                    .SetLoops(-1, LoopType.Restart)
                    .SetUpdate(true);
            }

            ShakeWheel(wheelFront);
            ShakeWheel(wheelRear);
        }

        private void ShakeWheel(Transform wheel)
        {
            if (wheel == null)
                return;

            wheel.DOShakeRotation(driveDuration, new Vector3(0f, 0f, wheelShakeStrength * 100f), 12, 90f, true)
                .SetLoops(-1, LoopType.Restart)
                .SetUpdate(true);

            wheel.DOShakePosition(driveDuration, wheelShakeStrength * 0.5f, 10, 90f, false, true)
                .SetLoops(-1, LoopType.Restart)
                .SetUpdate(true);
        }
    }
}
