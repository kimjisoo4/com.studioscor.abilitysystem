namespace StudioScor.AbilitySystem
{
    public class AbilityInputBuffer
    {
        private IAbilitySystem _abilitySystem;
        private IAbilitySpec _abilitySpec;

        public Ability Ability => _abilitySpec is null ? null : _abilitySpec.Ability;

        private bool _wasActivated = false;
        private bool _wasReleased = false;
        private float _remainTime = 0.2f;

        public AbilityInputBuffer()
        {

        }
        public AbilityInputBuffer(IAbilitySystem newAbilitySystem)
        {
            SetAbilitySystem(newAbilitySystem);
        }

        public void SetAbilitySystem(IAbilitySystem newAbilitySystem)
        {
            _abilitySystem = newAbilitySystem;
            _wasActivated = false;
            _wasReleased = false;
        }
        
        public void SetBuffer(Ability ability, float remainTime = 0.2f)
        {
            if (_abilitySystem is null)
                return;

            if (_abilitySystem.TryGetAbilitySpec(ability, out _abilitySpec))
            {
                _wasActivated = true;
                _wasReleased = false;
                _remainTime = remainTime;
            }
        }
        public void SetBuffer(IAbilitySpec abilitySpec, float remainTime = 0.2f)
        {
            _wasActivated = true;
            _wasReleased = false;
            _abilitySpec = abilitySpec;
            _remainTime = remainTime;
        }

        public void ReleaseBuffer(Ability ability)
        {
            if (_abilitySpec is null || _abilitySpec.Ability != ability)
                return;

            _wasReleased = true;
        }
        public void ReleaseBuffer(IAbilitySpec abilitySpec)
        {
            if (_abilitySpec is null || _abilitySpec != abilitySpec)
                return;

            _wasReleased = true;
        }

        public void ResetAbilityInputBuffer()
        {
            _abilitySpec = null;
            _wasActivated = false;
            _remainTime = 0f;
        }

        public void CancelBuffer()
        {
            _abilitySpec = null;
            _wasActivated = false;
            _wasReleased = false;
        }

        public void UpdateBuffer(float deltaTime)
        {
            if (!_wasActivated)
                return;

            if (_abilitySystem is null)
                return;

            if (_abilitySpec is null)
                return;

            _remainTime -= deltaTime;

            if(_abilitySpec.TryActiveAbility())
            {
                if (_wasReleased)
                    _abilitySpec.ReleaseAbility();

                CancelBuffer();
            }
            else if(_remainTime <= 0f)
            {
                CancelBuffer();
            }
        }
    }
}
