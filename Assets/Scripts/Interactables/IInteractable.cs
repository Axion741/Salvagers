namespace Assets.Scripts
{
    public interface IInteractable
    {
        void UseObject();

        void HighlightObject(bool enabled);

        string GetInteractionPrompt();

        void TogglePowered(bool toggle);
    }
}