using UnityEngine;
using StudioScor.Utilities;
using System;
using System.Collections.Generic;

namespace StudioScor.AbilitySystem
{
    [CreateAssetMenu(menuName ="StudioScor/Ability/Task/new Sphere Trace Task", fileName = "ATask_SphereTrace")]
    public class SphereTraceTask : Task
    {
        
        [Header(" [ Sphere Trace Task ] ")]
        [Header(" [ Main Task Setting ] ")]
        [SerializeField] private bool _UseScaleDurationToStrength = true;
        [SerializeField] private float _Duration = 1f;

        [Header(" [ Setting ] ")]
        [SerializeField, SRange(0f, 1f)] private float _Start = 0.2f;
        [SerializeField, SRange(0f, 1f)] private float _End = 0.8f;
        [SerializeField] private SphereTrace _SphereTrace;
        [SerializeField] private Vector3 _Offset;
        [SerializeField] private bool _UseScaleRadiusToStrength = false;
        [SerializeField] private float _Radius = 1f;
        [SerializeField] private Variable_LayerMask _TraceMask;

        public override ITaskSpec CreateSpec(GameObject owner)
        {
            return new Spec(this, owner);
        }

        public class Spec : AbilityTaskSpec<SphereTraceTask>
        {
            #region
            public delegate void TraceHitHandler(Spec traceTask, List<RaycastHit> Hits);
            #endregion

            private readonly List<Transform> _IgnoreHitTransforms;
            private List<RaycastHit> _HitResults;
            private Vector3 _PrevPosition;

            private float _Duration;
            private float _Radius;
            public override float Progress => _NormalizedTime;

            private float _ElapsedTime;
            private float _NormalizedTime;

            public event TraceHitHandler OnTraceHits;

            public Spec(SphereTraceTask actionBlock, GameObject owner) : base(actionBlock, owner)
            {
                _IgnoreHitTransforms = new();
                _HitResults = new();
            }

            protected override void EnterTask()
            {
                _Duration = AbilityTask._UseScaleDurationToStrength ? AbilityTask._Duration * Strength : AbilityTask._Duration;
                _Radius = AbilityTask._UseScaleRadiusToStrength ? AbilityTask._Radius * Strength : AbilityTask._Radius;

                _IgnoreHitTransforms.Clear();

                _PrevPosition = Owner.transform.TransformPoint(AbilityTask._Offset);

                _ElapsedTime = _Duration;
                _NormalizedTime = 0f;
            }

            protected override void UpdateMainTask(float deltaTime)
            {
                _ElapsedTime -= deltaTime;
                _NormalizedTime = _ElapsedTime / _Duration;
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

                Vector3 currentPosition = Owner.transform.TransformPoint(AbilityTask._Offset);
                FSphereTrace sphereTrace = new(_PrevPosition, currentPosition, _Radius, AbilityTask._TraceMask.Value);

                if (AbilityTask._SphereTrace.Trace(Owner.transform, sphereTrace, ref _HitResults, _IgnoreHitTransforms, UseDebug))
                {
                    Log($"Hit! - {_HitResults.Count}");

                    _IgnoreHitTransforms.AddRange(_HitResults.ConvertAll(e => e.transform));

                    Callback_OnTraceHits();
                }

                _PrevPosition = currentPosition;
            }

            protected void Callback_OnTraceHits()
            {
                Log("On Trace Hits");

                OnTraceHits?.Invoke(this, _HitResults);
            }
        }
    }
}

