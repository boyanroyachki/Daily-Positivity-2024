using OllamaSharp;
using System.Text;

namespace Daily_Positivity.Services
{
    public class OllamaService
    {
        private readonly OllamaApiClient _ollama;

        public OllamaService()
        {
            var uri = new Uri("http://localhost:11434");
            _ollama = new OllamaApiClient(uri);
            _ollama.SelectedModel = "tinyllama";
        }

        public async Task<string> GeneratePositiveSentenceAsync()
        {
            try
            {
                var prompt = "Generate a random positive sentence.";
                ConversationContext context = null;
                var responseStringBuilder = new StringBuilder();

                context = await _ollama.StreamCompletion(prompt, context, stream => responseStringBuilder.Append(stream.Response));

                return responseStringBuilder.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating positive sentence: {ex.Message}");
                return "Stay positive!";
            }
        }
    }
}
