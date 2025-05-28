using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;

namespace Executes
{
    public class ChatMessagePrefix
    {
        [JsonPropertyName("ChatPrefix")] public string Prefix { get; set; } = "";
    }
    public class ExecutesConfig : BasePluginConfig
    {
        [JsonPropertyName("ChatMessagePrefix")]
        public ChatMessagePrefix ChatMessagePrefix { get; set; } = new ChatMessagePrefix();
    }
}
