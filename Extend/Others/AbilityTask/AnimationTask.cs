using UnityEngine;
using StudioScor.Utilities;
using StudioScor.AbilitySystem;
using System;

namespace Portfolio.Abilities
{
    public interface IAnimationTask
    {
        public float NormalizedTime { get; }
        public bool TryStopAnimation(int animationHash);
        public void Play(int animationHash, float fadeIn = 0.2f, float fadeOut = 0.8f, float offset = 0f);
        public Action OnFinished { get; set; }
        public Action OnStartedBlendOut { get; set; }
    }

    [CreateAssetMenu(menuName = "StudioScor/Ability/Task/new AnimationTask",fileName = "ATask_Animation")]
    public class AnimationTask : Task
    {
        [Header(" Animation Task ")]
        [SerializeField] private string _Animation = "";
        [SerializeField, SRange(0f, 1f)] private float _FadeIn = 0.2f;
        [SerializeField, SRange(0f, 1f)] private float _FadeOut = 0.8f;
        [SerializeField] private float _Offset = 0f;
        [SerializeField] private bool _EndWhenBlendOut = false;

        public override ITaskSpec CreateSpec(GameObject owner)
        {
            return new Spec(this, owner);
        }

        public class Spec : AbilityTaskSpec<AnimationTask>
        {
            private readonly IAnimationTask _AnimationPlayer;

            private readonly Action OnAnimationFinished;
            public override float Progress => _AnimationPlayer.NormalizedTime;

            private int _AnimationHash;

            public Spec(AnimationTask actionBlock, GameObject owner) : base(actionBlock, owner)
            {
                _AnimationPlayer = owner.GetComponent<IAnimationTask>();

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
