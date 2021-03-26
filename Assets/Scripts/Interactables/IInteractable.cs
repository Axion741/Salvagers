using Assets.Scripts.Models;

namespace Assets.Scripts
{
    public interface IInteractable
    {
        void UseObject();

        void HighlightObject(bool enabled);

        InteractionPrompt GetInteractionPrompt();

        void TogglePowered(bool toggle);
    }
}