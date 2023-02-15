using UnityEngine;
using StudioScor.Utilities;
using StudioScor.AbilitySystem;
using System;
using System.Collections.Generic;


namespace Portfolio.Abilities
{
    [CreateAssetMenu(menuName ="StudioScor/Ability/Task/new Sphere Trace Task", fileName = "ATask_SphereTrace")]
    public class SphereTraceTask : AbilityTask
    {
        [Header(" [ Sphere Trace Task ] ")]
        [Header(" [ Main Task Setting ] ")]
        [SerializeField] private float _Duration = 1f;

        [Header(" [ Setting ] ")]
        [SerializeField, SRange(0f, 1f)] private float _Start = 0.2f;
        [SerializeField, SRange(0f, 1f)] private float _End = 0.8f;
        [SerializeField] private Vector3 _Offset;
        [SerializeField] private float _Radius = 1f;
        [SerializeField] private LayerMask _TraceMask;
        
        [SerializeField] private IgnoreHitCondition[] _IgnoreHitConditions;
        [SerializeField] private HitResultAction[] _HitResultActions;
        public override IAbilityTaskSpec CreateSpec(IAbilitySpec abilitySpec)
        {
            return new Spec(this, abilitySpec);
        }

        public class Spec : AbilityTaskSpec<SphereTraceTask>
        {
            private readonly Transform _Transform;
            private readonly List<Transform> _IgnoreHitTransforms;
            private List<RaycastHit> _HitResults;
            private Vector3 _PrevPosition;

            public override float Progress => _NormalizedTime;

            private float _ElapsedTime;
            private float _NormalizedTime;

            public Spec(SphereTraceTask actionBlock, IAbilitySpec abilitySpec) : base(actionBlock, abilitySpec)
            {
                _Transform = abilitySpec.AbilitySystem.transform;

                _IgnoreHitTransforms = new();
                _HitResults = new();
            }

            protected override void EnterTask()
            {
                _IgnoreHitTransforms.Clear();

                _IgnoreHitTransforms.Add(_Transform);

                _PrevPosition = _Transform.TransformPoint(AbilityTask._Offset);

                _ElapsedTime = AbilityTask._Duration;
                _NormalizedTime = 0f;
            }
            protected override void UpdateMainTask(float deltaTime)
            {
                _ElapsedTime -= deltaTime;
                _NormalizedTime = _ElapsedTime / AbilityTask._Duration;
                _NormalizedTime = MathF.Max(_NormalizedTime, 1f);

                UpdateTrace();

                if (_NormalizedTime >= 1f)
                    EndTask();
            }
            protected override void UpdateSubTask(float normalizedTime)
            {
                _NormalizedTime = normalizedTime;

                UpdateTrace();

                if (_NormalizedTime >= 1f)
                    EndTask();
            }

            protected void UpdateTrace()
            {
                if (!Progress.InRange(AbilityTask._Start, AbilityTask._End))
                    return;

                _HitResults.Clear();

                Vector3 currentPosition = _Transform.TransformPoint(AbilityTask._Offset);

                if(SUtility.Physics.DrawSphereCastAll(_PrevPosition, currentPosition, AbilityTask._Radius, AbilityTask._TraceMask, ref _HitResults, _IgnoreHitTransforms, UseDebug))
                {
                    foreach (var ignoreCondition in AbilityTask._IgnoreHitConditions)
                    {
                        ignoreCondition.IngnoreHit(_Transform, ref _HitResults);
                    }

                    foreach (var hitActions in AbilityTask._HitResultActions)
                    {
                        hitActions.HitActions(_Transform, _HitResults);
                    }

                    _IgnoreHitTransforms.AddRange(_HitResults.ConvertAll(e => e.transform));
                }
            }
        }
    }
}
