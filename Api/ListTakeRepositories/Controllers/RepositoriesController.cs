using ListTakeRepositories.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace ListTakeRepositories.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoriesController : ControllerBase
    {
        private static readonly string GITHUB_URI = "https://api.github.com/";
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger<RepositoriesController> _logger;

        public RepositoriesController(ILogger<RepositoriesController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetOldCsharpRepositories")]
        public async Task<IEnumerable<Repository>> Get([FromQuery]string? user, [FromQuery]string? language)
        {
            user = user ?? "takenet";
            language = language ?? "csharp";

            return await ListRepositories(user, language);
        }

        public static async Task<IEnumerable<Repository>> ListRepositories(string user, string language)
        {
            var listRepositories = await ProcessRepositories(user, language);
            IEnumerable<Repository> repositories = (IEnumerable<Repository>)listRepositories;

            IEnumerable<Repository> query = repositories.OrderBy(repository => repository.Created).Take(5);

            return query;
        }

        private static async Task<List<Repository>> ProcessRepositories(string user, string language)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            var query = new Dictionary<string, string>()
            {
                ["q"] = "user:"+user+" language:"+language,
                ["per_page"] = "100"
            };
            var uri = QueryHelpers.AddQueryString(GITHUB_URI + "search/repositories", query);
            var streamTask = client.GetStreamAsync(uri);
            var githubResponse = await JsonSerializer.DeserializeAsync<GithubResponse>(await streamTask);

            return githubResponse.Repositories;
        }

    }
}