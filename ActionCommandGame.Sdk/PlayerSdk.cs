using ActionCommandGame.Services.Model.Results;
using System.Net.Http.Json;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Security.Model.Abstractions;

namespace ActionCommandGame.Sdk
{
    public class PlayerSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;
        public PlayerSdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {   
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<IList<PlayerResult>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = "/api/players";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var player = await response.Content.ReadFromJsonAsync<IList<PlayerResult>>();

            if (player is null)
            {
                return new List<PlayerResult>();
            }

            return player;
        }
        public async Task<PlayerResult?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = $"/api/players/{id}";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var player = await response.Content.ReadFromJsonAsync<PlayerResult>();

            return player;
        }

        
        public async Task<PlayerResult?> Create(PlayerRequest player)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = "/api/players";
            var response = await httpClient.PostAsJsonAsync(route, player);

            response.EnsureSuccessStatusCode();

            var createdPlayer = await response.Content.ReadFromJsonAsync<PlayerResult>();

            return createdPlayer;
        }

        
        public async Task<PlayerResult?> Update(int id, PlayerRequest player)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = $"/api/players/{id}";
            var response = await httpClient.PutAsJsonAsync(route, player);

            response.EnsureSuccessStatusCode();

            var updatedPlayer = await response.Content.ReadFromJsonAsync<PlayerResult>();

            return updatedPlayer;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = $"/api/players/{id}";
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
