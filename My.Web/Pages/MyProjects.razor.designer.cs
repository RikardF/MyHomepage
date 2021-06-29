using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using My.Web.Services;
using My.Core.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.IO;

namespace My.Web.Pages
{
    public partial class MyProjectsComponent : ComponentBase
    {
        protected IEnumerable<GithubRepo> repos = new List<GithubRepo>();
        [Inject]
        private IHttpClientFactory _clientFactory { get; set; }

        partial void OnHttpClientHandlerCreate(ref HttpClientHandler handler);

        protected override async Task OnInitializedAsync()
        {
            using var client = _clientFactory.CreateClient("GithubAPI");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            try
            {
                this.repos = await client.GetFromJsonAsync<IEnumerable<GithubRepo>>("users/RikardF/repos");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            foreach (var repo in repos)
            {
                repo.Languages = await client.GetFromJsonAsync<Dictionary<string, int>>($"repos/RikardF/{repo.Name}/languages");
                repo.Contributors = await client.GetFromJsonAsync<IEnumerable<GithubOwner>>($"repos/RikardF/{repo.Name}/contributors");
            }
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        public string GetImagePath(string repoName)
        {
            var workingDirectory = Environment.CurrentDirectory;
            string filePath = $"{workingDirectory}\\wwwroot\\images\\{repoName}.png";
            if (File.Exists(filePath)) return $"../images/{repoName}.png";
            else return $"../images/NoImage.png";
        }
    }
}
