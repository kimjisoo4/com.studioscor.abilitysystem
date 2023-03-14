#if SCOR_ENABLE_MOVEMENTSYSTEM
using UnityEngine;
using System;
using StudioScor.Utilities;
using StudioScor.MovementSystem;

namespace StudioScor.AbilitySystem
{
    [CreateAssetMenu(menuName = "StudioScor/TaskSystem/new Reach MoveTo Task", fileName = "Task_ReachMoveTo")]
    public class ReachMoveToTask : Task
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

        public class Spec : TaskSpec
        {
            private new readonly ReachMoveToTask _Task;

            private readonly IMovementSystem _MovementSystem;
            private readonly ReachValueToTime _ReachValueToTimeX;
            private readonly ReachValueToTime _ReachValueToTimeY;
            private readonly ReachValueToTime _ReachValueToTimeZ;
            private float _NormalizedTime = 0f;
            private float _ElapsedTime = 0f;

            private float _Duration;
            private Quaternion Rotate;
            public override float Progress => _NormalizedTime;
            public Vector3 Distance => _Task.Distance;
            public float DistanceX => _Task.DistanceX;
            public float DistanceY => _Task.DistanceY;
            public float DistanceZ => _Task.DistanceZ;

            public Spec(Task task, GameObject owner) : base(task, owner)
            {
                _Task = task as ReachMoveToTask;
                _MovementSystem = owner.GetComponent<IMovementSystem>();

                _ReachValueToTimeX = new();
                _ReachValueToTimeY = new();
                _ReachValueToTimeZ = new();
            }
            protected override void EnterTask()
            {
                _Duration = _Task._UseScaleDurationToStrength ? _Task._Duration * Strength : _Task._Duration;

                _ElapsedTime = 0f;
                _NormalizedTime = 0f;

                switch (_Task._MoveDirection)
                {
                    case EMoveDirection.MoveDirection:
                        if (_MovementSystem.MoveStrength > 0f)
                        {
                            Rotate = Quaternion.LookRotation(_MovementSystem.MoveDirection, Owner.transform.up);
                        }
                        else
                        {
                            Vector3 direction = Owner.transform.TransformDirection(_Task._Direction);

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


                if (_Task._UseX)
                {
                    float distance = _Task._UseScaleXToStrength ? _Task._DistanceX * Strength : _Task._DistanceX;

                    _ReachValueToTimeX.OnMovement(distance, _Task._CurveX);
                }

                if (_Task._UseY)
                {
                    float distance = _Task._UseScaleYToStrength ? _Task._DistanceY * Strength : _Task._DistanceY;

                    _ReachValueToTimeY.OnMovement(distance, _Task._CurveY);
                }

                if (_Task._UseZ)
                {
                    float distance = _Task._UseScaleZToStrength ? _Task._DistanceZ * Strength : _Task._DistanceZ;

                    _ReachValueToTimeZ.OnMovement(distance, _Task._CurveZ);
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