namespace KimScor.GameplayTagSystem.Ability
{
    public class AbilityInputBuffer
    {
        private AbilitySpec _AbilitySpec;
        private bool _Activate = false;
        private float _Duration = 0.2f;

        public void SetBuffer(AbilitySpec abilitySpec, float duration = 0.2f)
        {
            _AbilitySpec = abilitySpec;
            _Activate = true;
            _Duration = duration;
        }

        public void CancelBuffer()
        {
            _AbilitySpec = null;
            _Activate = false;
        }

        public void Buffer(float deltaTime)
        {
            if (!_Activate || _AbilitySpec == null)
            {
                return;
            }

            _Duration -= deltaTime;

            if (_AbilitySpec.TryActivateAbility() || _Duration <= 0f)
            {
                CancelBuffer();
            }
        }
    }
}
