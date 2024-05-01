using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Certify.Client
{
    public partial class CustomService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;

        public CustomService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/CertifyApp/");
        }

        public async Task<Certify.Server.Models.CertifyApp.User> Authenticate(string username, string password)
        {
            var uri = new Uri(baseUri, $"Users(Username={username}, Password={password})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.User>(response);
        }

    }
}
