using ListTakeRepositories.Models;
using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
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
            language = language ?? "takenet";

            var repositories = await ListRepositories(user, language);
            
            return Enumerable.Range(1, 1).Select(index => new Repository
            {
                OwnerOrg = new Owner
                {
                    AvatarURL = "https://avatars.githubusercontent.com/u/4369522?v=4"
                },
                Name = "takenet/textc-csharp",
                Description = "Textc is a natural language processing library that allows developers build text command based applications with extensible text parsing capabilities.",
                CreatedAtUTC = DateTime.Now
            })
            .ToArray();
        }

        public static async Task<List<Repository>> ListRepositories(string user, string language)
        {
            var repositories = await ProcessRepositories(user, language);

            //foreach (var repo in repositories)
            //    Console.WriteLine(repo.Name);

            return repositories;
        }

        private static async Task<List<Repository>> ProcessRepositories(string user, string language)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            var query = new Dictionary<string, string>()
            {
                ["q"] = "user:"+user+" language:"+language
            };
            var uri = QueryHelpers.AddQueryString(GITHUB_URI + "search/repositories", query);
            var streamTask = client.GetStreamAsync(uri);
            var githubResponse = await JsonSerializer.DeserializeAsync<GithubResponse>(await streamTask);

            return githubResponse.Repositories;
        }

    }
}