using System.Text.Json.Serialization;

namespace ListTakeRepositories.Models
{
    public class Repository
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarURL { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAtUTC { get; set; }

        public DateTime LastPush => CreatedAtUTC.ToLocalTime();

    }
}