using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace backend;

public class LlmController
{
    public static readonly string SystemPrompt =
        "You are the Shakespeare translator. You translate chat messages to Shakespearean language. You use old, fancy, shakespearean language and high class words.";

    private static OpenAIService openAiService;
    
    public static async void Init()
    {
        openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = File.ReadAllText("resources/OpenAI_APIKey.apikey").ReplaceLineEndings("")
        });
    }

    public static async Task<string?> TranslateToShakespeare(string text)
    {
        var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem(SystemPrompt),
                ChatMessage.FromUser("Hey, what's up?"),
                ChatMessage.FromAssistant("Hark! What tidings dost thou bring upon this fine day?"),
                ChatMessage.FromUser(text)
            },
            Model = Models.Gpt_4o_mini ,
            MaxTokens = 100,
        });
        
        if (completionResult.Successful)
        {
            var generatedMessage = completionResult.Choices.First().Message.Content;
            Console.WriteLine("Message: " + generatedMessage);
            return generatedMessage;
        }
        
        Console.WriteLine("Shakespeare's tongue is broken.");
        Console.WriteLine(completionResult.Error.Message);

        return null;
    }
}
