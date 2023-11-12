namespace StudioScor.AbilitySystem
{
    public class AbilityInputBuffer
    {
        private IAbilitySystem abilitySystem;
        private IAbilitySpec abilitySpec;

        private bool activate = false;
        private float remainTime = 0.2f;

        public AbilityInputBuffer()
        {

        }
        public AbilityInputBuffer(IAbilitySystem newAbilitySystem)
        {
            SetAbilitySystem(newAbilitySystem);
        }

        public void SetAbilitySystem(IAbilitySystem newAbilitySystem)
        {
            abilitySystem = newAbilitySystem;
            activate = false;
        }
        
        public void SetBuffer(Ability ability, float remainTime = 0.2f)
        {
            if (abilitySystem is null)
                return;

            if (abilitySystem.TryGetAbilitySpec(ability, out abilitySpec))
            {
                activate = true;
                this.remainTime = remainTime;
            }
        }
        public void SetBuffer(IAbilitySpec abilitySpec, float remainTime = 0.2f)
        {
            this.abilitySpec = abilitySpec;
            activate = true;
            this.remainTime = remainTime;
        }

        public void ResetAbilityInputBuffer()
        {
            abilitySpec = null;
            activate = false;
            remainTime = 0f;
        }

        public void CancelBuffer()
        {
            abilitySpec = null;
            activate = false;
        }

        public void UpdateBuffer(float deltaTime)
        {
            if (!activate || abilitySystem is null || abilitySpec is null)
            {
                return;
            }

            remainTime -= deltaTime;

            if (abilitySpec.TryActiveAbility() || remainTime <= 0f)
            {
                CancelBuffer();
            }
        }
    }
}
