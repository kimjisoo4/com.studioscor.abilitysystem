#if SCOR_ENABLE_MOVEMENTSYSTEM
using UnityEngine;
using System;
using StudioScor.Utilities;
using StudioScor.MovementSystem;


namespace StudioScor.AbilitySystem.Extend
{

    [CreateAssetMenu(menuName = "StudioScor/Ability/Task/new Curve MoveTo Task", fileName = "ATask_CurveMoveTo")]
    public class CurveMoveToTask : AbilityTask
    {
        [Header(" [ CurveMoveTo Task ] ")]
        [SerializeField] private EMoveDirection _MoveDirection = EMoveDirection.Local;
        [SerializeField, SEnumCondition(nameof(_MoveDirection), (int)EMoveDirection.MoveDirection)] private Vector3 _Direction = Vector3.forward;

        [Header(" [ Main Setting ] ")]
        [SerializeField, Tooltip("Duration Only Use Main Task. ")] private float _Duration = 0.5f;

        [Header(" [ Setting ] ")]
        [SerializeField] private bool _UseX = false;
        [SerializeField, SCondition(nameof(_UseX), true)] private float _DistanceX = 0f;
        [SerializeField, SCondition(nameof(_UseX), true)] private AnimationCurve _CurveX = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField] private bool _UseY = false;
        [SerializeField, SCondition(nameof(_UseY), true)] private float _DistanceY = 0f;
        [SerializeField, SCondition(nameof(_UseY), true)] private AnimationCurve _CurveY = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField] private bool _UseZ = true;
        [SerializeField, SCondition(nameof(_UseZ), true)] private float _DistanceZ = 5f;
        [SerializeField, SCondition(nameof(_UseZ), true)] private AnimationCurve _CurveZ = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public override IAbilityTaskSpec CreateSpec(IAbilitySpec abilitySpec)
        {
            return new Spec(this, abilitySpec);
        }

        public class Spec : AbilityTaskSpec<CurveMoveToTask>
        {
            private readonly MovementSystemComponent _MovementSystem;
            private readonly ReachValueToTime _ReachValueToTimeX;
            private readonly ReachValueToTime _ReachValueToTimeY;
            private readonly ReachValueToTime _ReachValueToTimeZ;
            public override float Progress => _NormalizedTime;
            private float _NormalizedTime = 0f;
            private float _ElapsedTime = 0f;

            Quaternion Rotate;

            public Spec(CurveMoveToTask actionBlock, IAbilitySpec abilitySpec) : base(actionBlock, abilitySpec)
            {
                _MovementSystem = abilitySpec.AbilitySystem.GetComponent<MovementSystemComponent>();

                _ReachValueToTimeX = new();
                _ReachValueToTimeY = new();
                _ReachValueToTimeZ = new();
            }
            protected override void EnterTask()
            {
                _ElapsedTime = AbilityTask._Duration;

                _NormalizedTime = 0f;

                switch (AbilityTask._MoveDirection)
                {
                    case EMoveDirection.MoveDirection:
                        if (_MovementSystem.MoveStrength > 0f)
                        {
                            Rotate = Quaternion.LookRotation(_MovementSystem.MoveDirection, _MovementSystem.transform.up);
                        }
                        else
                        {
                            Vector3 direction = _MovementSystem.transform.TransformDirection(AbilityTask._Direction);

                            Rotate = Quaternion.LookRotation(direction);
                        }
                        break;
                    case EMoveDirection.Local:
                        Rotate = _MovementSystem.transform.rotation;
                        break;
                    default:
                        Rotate = Quaternion.identity;
                        break;
                }


                if (AbilityTask._UseX)
                    _ReachValueToTimeX.OnMovement(AbilityTask._DistanceX, AbilityTask._CurveX);

                if (AbilityTask._UseY)
                    _ReachValueToTimeY.OnMovement(AbilityTask._DistanceY, AbilityTask._CurveY);

                if (AbilityTask._UseZ)
                    _ReachValueToTimeZ.OnMovement(AbilityTask._DistanceZ, AbilityTask._CurveZ);
            }
            protected override void ExitTask()
            {
                _NormalizedTime = 1f;

                _ReachValueToTimeX.EndMovement();
                _ReachValueToTimeY.EndMovement();
                _ReachValueToTimeZ.EndMovement();
            }
            protected override void UpdateMainTask(float deltaTime)
            {
                _ElapsedTime += deltaTime;
                _NormalizedTime = _ElapsedTime / AbilityTask._Duration;

                _NormalizedTime = MathF.Max(_NormalizedTime, 1f);

                UpdateMovement();
            }
            protected override void UpdateSubTask(float normalizedTime)
            {
                _NormalizedTime = normalizedTime;

                UpdateMovement();
            }

            private void UpdateMovement()
            {
                Log(" Normalized Time - " + _NormalizedTime);

                float x = _ReachValueToTimeX.UpdateMovement(_NormalizedTime);
                float y = _ReachValueToTimeY.UpdateMovement(_NormalizedTime);
                float z = _ReachValueToTimeZ.UpdateMovement(_NormalizedTime);

                Vector3 addPosition = new Vector3(x, y, z);

                _MovementSystem.AddPosition(Rotate * addPosition);

                if (_NormalizedTime >= 1f)
                {
                    EndTask();
                }
            }

        }
    }
}
#endif