using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace SemanticKernelOllama;

public static class Extensions
{
    public static void Debug(this ChatHistory chatHistory)
    {
        foreach (var chat in chatHistory)
        {
            if (chat.Items != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var item in chat.Items)
                {
                    if (item is FunctionCallContent functionCall)
                    {
                        if (item.InnerContent is OllamaSharp.Models.Chat.Message.ToolCall toolCall)
                        {
                            Console.Write("Debug > ");
                            Console.WriteLine($"{chat.Role} > FunctionCallContent: {toolCall.Function.Name}");
                        }
                    }
                    else if (item is FunctionResultContent functionResult)
                    {
                        Console.Write("Debug > ");
                        Console.WriteLine($"{chat.Role} > FunctionResultContent: {functionResult.Result}");
                    }
                    else if (item is TextContent textContent)
                    {
                        Console.Write("Debug > ");
                        Console.WriteLine($"{chat.Role} > TextContent: {textContent.Text}");
                    }
                }
            }
        }
    }
}