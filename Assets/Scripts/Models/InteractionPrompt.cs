namespace Assets.Scripts.Models
{
    public class InteractionPrompt
    {
        public string Prompt { get; set; }

        public string Key { get; set; }

        public InteractionPrompt()
        {
            Prompt = "";
            Key = "E";
        }
    }
}
