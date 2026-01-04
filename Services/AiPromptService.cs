namespace PersonalPages.Services
{
    public class AiPromptService
    {
        public string GeneratePrompt(string mood)
        {
            return $"Write about a moment when you felt {mood}.";
        }
    }
}
