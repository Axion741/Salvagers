using System.Diagnostics;

namespace Assets.Scripts
{
    interface IInteractable
    {
        void UseObject();

        void HighlightObject(bool enabled);
    }
}
