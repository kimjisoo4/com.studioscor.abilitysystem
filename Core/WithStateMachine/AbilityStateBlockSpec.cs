using StudioScor.Utilities;


namespace StudioScor.AbilitySystem
{
    public class AbilityStateBlockSpec : BaseStateClass
    {
        public override bool UseDebug => false;

        protected override void EnterState()
        {
        }

        public void OnUpdateState(float delatTime)
        {
            UpdateState(delatTime);
        }
        public void OnFixedUpdateState(float deltaTime)
        {
            FixedUpdateState(deltaTime);
        }
        protected virtual void UpdateState(float delatTime) { }
        protected virtual void FixedUpdateState(float delatTime) { }
    }
}
