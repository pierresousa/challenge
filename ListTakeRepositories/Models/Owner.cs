using System.Text.Json.Serialization;

namespace ListTakeRepositories.Models
{
    public class Owner
    {
        [JsonPropertyName("avatar_url")]
        public string AvatarURL { get; set; }

    }
}