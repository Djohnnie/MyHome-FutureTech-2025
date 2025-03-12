using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemanticKernelAzureOpenAi;


var deploymentName = Environment.GetEnvironmentVariable("DEPLOYMENT_NAME");
var endpoint = Environment.GetEnvironmentVariable("ENDPOINT");
var apiKey = Environment.GetEnvironmentVariable("API_KEY");

var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey);
builder.Plugins.AddFromType<GeneralPlugin>();

var kernel = builder.Build();

var chatCompletionAgent = new ChatCompletionAgent
{
    Name = "TimeAgent",
    Description = "Agent that knows about the current date and time.",
    Instructions = "You should only reply on questions related to the current date and time.",
    Kernel = kernel,
    Arguments = new KernelArguments(new OpenAIPromptExecutionSettings
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
    })
};

ChatHistory history = [];

while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");
    Console.ForegroundColor = ConsoleColor.White;
    var request = Console.ReadLine();
    history.AddUserMessage(request!);

    string fullMessage = "";
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("Assistant > ");

    await foreach (var response in chatCompletionAgent.InvokeAsync(history))
    {
        Console.Write(response.Content);
        fullMessage += response.Content;
    }

    Console.WriteLine();

    history.AddAssistantMessage(fullMessage);
    history.Debug();
}