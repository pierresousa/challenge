using System.Text.Json.Serialization;

namespace ListTakeRepositories.Models
{
    public class GithubResponse
    {
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        [JsonPropertyName("incomplete_results")]
        public bool IncompleteResults { get; set; }

        [JsonPropertyName("items")]
        public List<Repository> Repositories { get; set; }
    }
}