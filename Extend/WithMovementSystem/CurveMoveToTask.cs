#if SCOR_ENABLE_MOVEMENTSYSTEM
using UnityEngine;
using System;
using StudioScor.Utilities;
using StudioScor.MovementSystem;

namespace StudioScor.AbilitySystem
{
    [CreateAssetMenu(menuName = "StudioScor/Ability/Task/new Curve MoveTo Task", fileName = "ATask_CurveMoveTo")]
    public class CurveMoveToTask : Task
    {
        [Header(" [ CurveMoveTo Task ] ")]
        [SerializeField] private EMoveDirection _MoveDirection = EMoveDirection.Local;
        [SerializeField, SEnumCondition(nameof(_MoveDirection), (int)EMoveDirection.MoveDirection)] private Vector3 _Direction = Vector3.forward;

        [Header(" [ Main Setting ] ")]
        [SerializeField, Tooltip("Duration Only Use Main Task. ")] private float _Duration = 0.5f;
        [SerializeField] private bool _UseScaleDurationToStrength = true;

        [Header(" [ Setting ] ")]
        [SerializeField] private bool _UseX = false;
        [SerializeField, SCondition(nameof(_UseX), true)] private bool _UseScaleXToStrength = true;
        [SerializeField, SCondition(nameof(_UseX), true)] private float _DistanceX = 0f;
        [SerializeField, SCondition(nameof(_UseX), true)] private AnimationCurve _CurveX = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField] private bool _UseY = false;
        [SerializeField, SCondition(nameof(_UseY), true)] private bool _UseScaleYToStrength = false;
        [SerializeField, SCondition(nameof(_UseY), true)] private float _DistanceY = 0f;
        [SerializeField, SCondition(nameof(_UseY), true)] private AnimationCurve _CurveY = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField] private bool _UseZ = true;
        [SerializeField, SCondition(nameof(_UseZ), true)] private bool _UseScaleZToStrength = true;
        [SerializeField, SCondition(nameof(_UseZ), true)] private float _DistanceZ = 5f;
        [SerializeField, SCondition(nameof(_UseZ), true)] private AnimationCurve _CurveZ = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public Vector3 Distance => new Vector3(DistanceX, DistanceY, DistanceZ);
        public float DistanceX => _UseX ? _DistanceX : 0f;
        public float DistanceY => _UseY ? _DistanceY : 0f;
        public float DistanceZ => _UseZ ? _DistanceZ : 0f;

        public override ITaskSpec CreateSpec(GameObject owner)
        {
            return new Spec(this, owner);
        }

        public class Spec : AbilityTaskSpec<CurveMoveToTask>
        {
            private readonly IMovementSystem _MovementSystem;
            private readonly ReachValueToTime _ReachValueToTimeX;
            private readonly ReachValueToTime _ReachValueToTimeY;
            private readonly ReachValueToTime _ReachValueToTimeZ;
            private float _NormalizedTime = 0f;
            private float _ElapsedTime = 0f;

            private float _Duration;
            private Quaternion Rotate;
            public override float Progress => _NormalizedTime;
            public Vector3 Distance => AbilityTask.Distance;
            public float DistanceX => AbilityTask.DistanceX;
            public float DistanceY => AbilityTask.DistanceY;
            public float DistanceZ => AbilityTask.DistanceZ;

            public Spec(CurveMoveToTask actionBlock, GameObject owner) : base(actionBlock, owner)
            {
                _MovementSystem = owner.GetComponent<IMovementSystem>();

                _ReachValueToTimeX = new();
                _ReachValueToTimeY = new();
                _ReachValueToTimeZ = new();
            }
            protected override void EnterTask()
            {
                _Duration = AbilityTask._UseScaleDurationToStrength ? AbilityTask._Duration * Strength : AbilityTask._Duration;

                _ElapsedTime = 0f;
                _NormalizedTime = 0f;

                switch (AbilityTask._MoveDirection)
                {
                    case EMoveDirection.MoveDirection:
                        if (_MovementSystem.MoveStrength > 0f)
                        {
                            Rotate = Quaternion.LookRotation(_MovementSystem.MoveDirection, Owner.transform.up);
                        }
                        else
                        {
                            Vector3 direction = Owner.transform.TransformDirection(AbilityTask._Direction);

                            Rotate = Quaternion.LookRotation(direction);
                        }
                        break;
                    case EMoveDirection.Local:
                        Rotate = Owner.transform.rotation;
                        break;
                    default:
                        Rotate = Quaternion.identity;
                        break;
                }


                if (AbilityTask._UseX)
                {
                    float distance = AbilityTask._UseScaleXToStrength ? AbilityTask._DistanceX * Strength : AbilityTask._DistanceX;

                    _ReachValueToTimeX.OnMovement(distance, AbilityTask._CurveX);
                }

                if (AbilityTask._UseY)
                {
                    float distance = AbilityTask._UseScaleYToStrength ? AbilityTask._DistanceY * Strength : AbilityTask._DistanceY;

                    _ReachValueToTimeY.OnMovement(distance, AbilityTask._CurveY);
                }

                if (AbilityTask._UseZ)
                {
                    float distance = AbilityTask._UseScaleZToStrength ? AbilityTask._DistanceZ * Strength : AbilityTask._DistanceZ;

                    _ReachValueToTimeZ.OnMovement(distance, AbilityTask._CurveZ);
                }
            }

            protected override void ExitTask()
            {
                _ReachValueToTimeX.EndMovement();
                _ReachValueToTimeY.EndMovement();
                _ReachValueToTimeZ.EndMovement();
            }

            protected override void UpdateMainTask(float deltaTime)
            {
                _ElapsedTime += deltaTime;
                _NormalizedTime = _ElapsedTime.SafeDivide(_Duration);

                if(_NormalizedTime < 0f)
                {
                    Log("Duration is ZERO", true);

                    _NormalizedTime = 1f;
                }
                else
                {
                    _NormalizedTime = MathF.Min(_NormalizedTime, 1f);
                }

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

                _MovementSystem.MovePosition(Rotate * addPosition);

                if (_NormalizedTime >= 1f)
                {
                    EndTask();
                }
            }

        }
    }
}
#endif