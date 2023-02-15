using UnityEngine;
using StudioScor.Utilities;
using StudioScor.AbilitySystem;
using System;

namespace Portfolio.Abilities
{
    [CreateAssetMenu(menuName = "StudioScor/Ability/Task/new AnimationTask",fileName = "ATask_Animation")]
    public class AnimationTask : AbilityTask
    {
        [Header(" Animation Task ")]
        [SerializeField] private string _Animation = "";
        [SerializeField, SRange(0f, 1f)] private float _FadeIn = 0.2f;
        [SerializeField, SRange(0f, 1f)] private float _FadeOut = 0.8f;
        [SerializeField] private float _Offset = 0f;
        [SerializeField] private bool _EndWhenBlendOut = false;

        public override IAbilityTaskSpec CreateSpec(IAbilitySpec abilitySpec)
        {
            return new Spec(this, abilitySpec);
        }

        public class Spec : AbilityTaskSpec<AnimationTask>
        {
            private readonly AnimationPlayer _AnimationPlayer;

            private readonly Action OnAnimationFinished;
            public override float Progress => _AnimationPlayer.NormalizedTime;

            private int _AnimationHash;

            public Spec(AnimationTask actionBlock, IAbilitySpec abilitySpec) : base(actionBlock, abilitySpec)
            {
                _AnimationPlayer = abilitySpec.AbilitySystem.GetComponent<AnimationPlayer>();

                OnAnimationFinished = () => { EndTask(); };
            }
 
            protected override void EnterTask()
            {
                _AnimationHash = Animator.StringToHash(AbilityTask._Animation);

                AnimationPlay();
            }
            protected override void ExitTask()
            {
                _AnimationPlayer.TryStopAnimation(_AnimationHash);
            }

            private void AnimationPlay()
            {
                _AnimationPlayer.Play(_AnimationHash, AbilityTask._FadeIn, AbilityTask._FadeOut, AbilityTask._Offset);

                if (!AbilityTask._EndWhenBlendOut)
                    _AnimationPlayer.OnFinished = OnAnimationFinished;
                else
                    _AnimationPlayer.OnStartedBlendOut = OnAnimationFinished;
            }
        }
    }
}
