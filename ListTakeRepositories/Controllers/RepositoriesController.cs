using ListTakeRepositories.Models;
using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;

namespace ListTakeRepositories.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoriesController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly ILogger<RepositoriesController> _logger;

        public RepositoriesController(ILogger<RepositoriesController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetOldCsharpRepositories")]
        public IEnumerable<Repository> Get([FromQuery]string user="takenet", [FromQuery] string language="csharp")
        {

            return Enumerable.Range(1, 1).Select(index => new Repository
            {
                AvatarURL = "https://avatars.githubusercontent.com/u/4369522?v=4",
                Name = "takenet/textc-csharp",
                Description = "Textc is a natural language processing library that allows developers build text command based applications with extensible text parsing capabilities.",
                CreatedAtUTC = DateTime.Now
            })
            .ToArray();
        }

        public static async Task<List<Repository>> ListRepositories()
        {
            var repositories = await ProcessRepositories();

            //foreach (var repo in repositories)
            //    Console.WriteLine(repo.Name);

            return repositories;
        }

        private static async Task<List<Repository>> ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);

            return repositories;
        }

    }
}