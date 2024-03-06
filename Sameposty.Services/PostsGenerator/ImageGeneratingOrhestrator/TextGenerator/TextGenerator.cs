using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;
public class TextGenerator(IOpenAIService openAiService) : ITextGenerator
{
    private readonly int maxTokensUsedToGenerateText = 1000;
    private readonly int maxCharCount = 500;

    public async Task<string> GeneratePostDescription(GeneratePostRequest request)
    {
        var text = new ChatCompletionCreateRequest()
        {
            Model = Models.Gpt_4_0125_preview,
            MaxTokens = maxTokensUsedToGenerateText,
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem($"Pracujesz w małej, lokalnej firmie i jesteś odpowiedzialny za prowadzenie social media dla tej firmy. Twój szef podał Ci tę listę najważniejszych branż: {request.Branch}, usług i/lub produktów: {request.ProductsAndServices}, cele jakimi kieruje się firma {request.Goals} oraz jakie czynniki wyróżniają tę firmę na rynku: {request.Assets} jakie cehują tę firmę. Twoim zadaniem jest tworzenie bardzo ciekawych tekstów do postów na social media. Teksty powinny być ciekawe i angażujące użytkowników. Każdy tekst powinien być unikalny. Posty powinny też zachęcać do skorzystania z usług tej firmy. Teksty, jeśli to pasuje do branży, mogą zawierać emotikony i/lub hashtagi. Post nie może być dłuższy niż {maxCharCount} znaków."),
                ChatMessage.FromUser($"Przygotuj teraz jeden z takich tekstów."),
            }
        };

        var response = await openAiService.ChatCompletion.CreateCompletion(text);

        return response.Choices.First().Message.Content;
    }
}
