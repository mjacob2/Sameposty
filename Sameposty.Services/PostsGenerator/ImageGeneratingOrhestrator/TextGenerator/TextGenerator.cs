using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;
public class TextGenerator(IOpenAIService openAiService) : ITextGenerator
{
    private readonly int maxTokensUsedToGenerateText = 1000;
    private readonly int maxTokensUsedToGenerateImagePrompt = 1000;
    private readonly int maxCharCount = 500;

    public async Task<string> ReGeneratePostDescription(ReGeneratePostRequest request)
    {
        var text = new ChatCompletionCreateRequest()
        {
            Model = Models.Gpt_4_0125_preview,
            MaxTokens = maxTokensUsedToGenerateImagePrompt,
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("$\"Jesteś pracownikiem agencji marketingowej i jesteś specjalistą od marketingu w social media. Twoim zadaniem jest tworzyć świetny tekst do posta na Facebook. Musisz pamiętać, jakie teksty były już tworzone dla tej firmy, żeby teksty się nie powtarzały. Tekst nie może być dłuższy niż {maxCharCount} znaków. Tworzysz sam tekst posta. Bez zanych komentarzy ani dopisków."),

                ChatMessage.FromSystem($"Pracujesz dla firmy, której nazwa marketingowa to {request.BrandName} a jej grupa docelowa to: {request.Audience}"),

                ChatMessage.FromUser($"Napisz post w tym temacie: {request.UserPrompt}"),
            }
        };

        var response = await openAiService.ChatCompletion.CreateCompletion(text);

        return response.Choices.First().Message.Content;
    }

    public async Task<string> GeneratePromptForImageForPost(string productsAndServices)
    {
        var text = new ChatCompletionCreateRequest()
        {
            Model = Models.Gpt_4_0125_preview,
            MaxTokens = maxTokensUsedToGenerateImagePrompt,
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("Jesteś pomocnym asystentem"),

                ChatMessage.FromUser($"Daj mi jedną, kreatywną propozycję, co może przedstawiać zdjęcie, które będzie opublikowane na social media firmy, która tak odpowiedziała na pytanie o to czym się zajmuje: {productsAndServices}"),
            }
        };

        var response = await openAiService.ChatCompletion.CreateCompletion(text);

        return response.Choices.First().Message.Content;
    }


    public async Task<string> GeneratePostDescription(GeneratePostRequest request)
    {
        var text = new ChatCompletionCreateRequest()
        {
            Model = Models.Gpt_4_0125_preview,
            MaxTokens = maxTokensUsedToGenerateText,
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem($"Jesteś pracownikiem agencji marketingowej i jesteś specjalistą od marketingu w social media. Twoim zadaniem jest tworzyć świetny tekst do posta na Facebook. Musisz pamiętać, jakie teksty były już tworzone dla tej firmy, żeby teksty się nie powtarzały. Tekst nie może być dłuższy niż {maxCharCount} znaków. Tworzysz sam tekst posta. Bez zanych komentarzy ani dopisków."),

                ChatMessage.FromAssistant("Ok. Żebym mógł przygotować świetny i dopasowany tekst, podaj mi więcej informacji o tej firmie i odpowiedz na następujące pytania: Jak się nazywa firma, dla której mam przygotować tekst na post na jej social media?"),

                ChatMessage.FromUser($"{request.BrandName}"),

                ChatMessage.FromAssistant("A czym zajmuje się ta firma?"),

                ChatMessage.FromUser($"{request.ProductsAndServices}"),

                ChatMessage.FromAssistant("A jaką misję ma firma i czym się kieruje?"),

                ChatMessage.FromUser($"{request.Mission}"),

                ChatMessage.FromAssistant("A co wiemy o grupie docelowej?"),

                ChatMessage.FromUser($"{request.Audience}"),

                ChatMessage.FromAssistant("A jaki cel marketingowy ta firma chce osiągnąć?"),

                ChatMessage.FromUser($"{request.Goals}"),

                ChatMessage.FromAssistant("A czym wyróżnia się ta firma na tle konkurencji?"),

                ChatMessage.FromUser($"{request.Assets}"),

                ChatMessage.FromAssistant("Ok, dziękuję za informacje. Teraz pomyslę, i stworzę najlepszy post na social media dla tej firmy.")
            }
        };

        var response = await openAiService.ChatCompletion.CreateCompletion(text);

        return response.Choices.First().Message.Content;
    }

}
