namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public interface IActivator
    {
        public bool IsActivated { get; }
        public DoorInter DoorRef { get; }

        public void OnActivatorAction();
    }
}
//Git