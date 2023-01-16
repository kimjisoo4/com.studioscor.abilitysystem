using StudioScor.Utilities;


namespace StudioScor.AbilitySystem
{
    public interface IAbilityState
    {
        public IAbilitySpec AbilitySpec { get; }
        public FiniteStateMachineSystem<AbilityStateBlockSpec> StateMachine { get; }
        public void OnFinishState();
    }
}
