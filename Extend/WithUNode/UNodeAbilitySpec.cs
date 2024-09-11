#if SCOR_ENABLE_UNODE
using MaxyGames.UNode;
using UnityEngine;

namespace StudioScor.AbilitySystem.UNode
{
    public static class AbilitySystemWithUNodeUtility
    {
        public const string FUNCTION_CAN_ACTIVE_ABILITY = "CanActiveAbility";
        public const string FUNCTION_CAN_RETRIGGER_ABILITY = "CanReTriggerAbility";
        public const string FUNCTION_ON_GRANT_ABILITY = "OnGrantAbility";
        public const string FUNCTION_ON_REMOVE_ABILITY = "OnRemoveAbility";
        public const string FUNCTION_ENTER_ABILITY = "EnterAbility";
        public const string FUNCTION_EXIT_ABILITY = "ExitAbility";
        public const string FUNCTION_ON_FINISH_ABILITY = "OnFinishAbility";
        public const string FUNCTION_CANCEL_ABILITY_WITH_SOURCE = "CancelAbilityWithSource";
        public const string FUNCTION_ON_CANCEL_ABILITY = "OnCancelAbility";
        public const string FUNCTION_ON_RELEASE_ABILITY = "OnReleaseAbility";
        public const string FUNCTION_ON_RETRIGGER_ABILITY = "OnReTriggerAbility";
        public const string FUNCTION_ON_CHANGE_LEVEL = "OnChangeLevel";
    }

    public class UNodeAbilitySpec : GameObjectAbilitySpec
    {
        [Header(" [ UNode Ability Spec ] ")]
        [SerializeField] private ClassComponent _classComponent;

        private bool _isCommitActivate = false;
        private bool _isCommitReTrigger = false;

        public void CommitActivate()
        {
            _isCommitActivate = true;
        }
        public void CommitReTrigger()
        {
            _isCommitReTrigger = true;
        }

        public override bool CanActiveAbility()
        {
            if (!base.CanActiveAbility())
                return false;

            _isCommitActivate = false;

            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_CAN_ACTIVE_ABILITY, null);

            if (!_isCommitActivate)
                return false;

            return true;
        }

        public override bool CanReTriggerAbility()
        {
            if (!base.CanReTriggerAbility())
                return false;

            _isCommitReTrigger = false;

            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_CAN_RETRIGGER_ABILITY, null);

            if (!_isCommitReTrigger)
                return false;

            return true;
        }

        protected override void OnGrantAbility()
        {
            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_ON_GRANT_ABILITY, null);
        }
        protected override void OnRemoveAbility()
        {
            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_ON_REMOVE_ABILITY, null);
        }
        protected override void EnterAbility()
        {
            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_ENTER_ABILITY, null);
        }
        protected override void ExitAbility()
        {
            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_EXIT_ABILITY, null);
        }
        protected override void OnFinishAbility()
        {
            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_ON_FINISH_ABILITY, null);
        }

        public override void CancelAbilityFromSource(object source)
        {
            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_CANCEL_ABILITY_WITH_SOURCE, new object[]{source} );
        }
        protected override void OnCancelAbility()
        {
            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_ON_CANCEL_ABILITY, null);
        }
        protected override void OnReleaseAbility()
        {
            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_ON_RELEASE_ABILITY, null);
        }
        protected override void OnReTriggerAbility()
        {
            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_ON_RETRIGGER_ABILITY, null);
        }
        protected override void OnChangeLevel(int prevLevel)
        {
            _classComponent.InvokeFunction(AbilitySystemWithUNodeUtility.FUNCTION_ON_CHANGE_LEVEL, new object[1]{prevLevel});
        }
    }
}

#endif