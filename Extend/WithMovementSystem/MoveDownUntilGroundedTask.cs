#if SCOR_ENABLE_MOVEMENTSYSTEM
using UnityEngine;
using StudioScor.Utilities;
using StudioScor.MovementSystem;

namespace StudioScor.AbilitySystem
{
    [CreateAssetMenu(menuName = "StudioScor/AbilitySystem/Task/new MoveDown Until Grounded Task", fileName = "ATask_MoveDown")]
    public class MoveDownUntilGroundedTask : Task
    {
        [Header(" [ Jump Down Task ] ")]
        [Header(" [ Main Task Setting ] ")]
        [SerializeField] private float _Duration = 1f;
        [Header(" [ Setting ] ")]
        [SerializeField] private float _StartSpeed = 20f;
        [SerializeField] private float _TargetSpeed = 9.81f;

        public override ITaskSpec CreateSpec(GameObject owner)
        {
            return new Spec(this, owner);
        }

        public class Spec : AbilityTaskSpec<MoveDownUntilGroundedTask>
        {
            private readonly IMovementSystem _MovementSystem;
            private float _StartSpeed;
            private float _TargetSpeed;

            private float _NormalizedTime = 0f;
            private float _ElapsedTime;
            public override float Progress => _NormalizedTime;

            public Spec(MoveDownUntilGroundedTask actionBlock, GameObject owner) : base(actionBlock, owner)
            {
                _MovementSystem = owner.GetComponent<IMovementSystem>();
            }
            protected override void EnterTask()
            {
                _StartSpeed = Strength * AbilityTask._StartSpeed;
                _TargetSpeed = Strength * AbilityTask._TargetSpeed;

                _ElapsedTime = 0f;
                _NormalizedTime = 0f;
            }
            protected override void ExitTask()
            {
                _NormalizedTime = 1f;
            }
            protected override void UpdateMainTask(float deltaTime)
            {
                _ElapsedTime += deltaTime;
                _NormalizedTime = _ElapsedTime.SafeDivide(AbilityTask._Duration);

                UpdateMovement();
            }
            protected override void UpdateSubTask(float normalizedTime)
            {
                _NormalizedTime = normalizedTime;

                UpdateMovement();
            }

            private void UpdateMovement()
            {
                float speed = Mathf.Lerp(_StartSpeed, _TargetSpeed, _NormalizedTime);

                _MovementSystem.AddVelocity(Vector3.down * speed);

                if (_MovementSystem.IsGrounded)
                {
                    EndTask();
                }
            }
        }
    }
}
#endif