using System.Text.Json.Serialization;

namespace ListTakeRepositories.Models
{
    public class Repository
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("owner")]
        public Owner OwnerOrg { get; set; }

        public string AvatarURL => OwnerOrg.AvatarURL;

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAtUTC { get; set; }

        public DateTime Created => CreatedAtUTC.ToLocalTime();

    }
}