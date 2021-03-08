namespace Assets.Scripts.WorldObjects
{
    interface IDoor
    {
        void ToggleDoor();

        void SetPanelSprites();

        void ToggleDoorLights(bool toggle);
    }
}
