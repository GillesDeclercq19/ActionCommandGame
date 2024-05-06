using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Security.Model.Abstractions;
using ActionCommandGame.Services.Model.Results;
using System.Net.Http.Json;


namespace ActionCommandGame.Sdk
{
    public class PlayerItemSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;
        public PlayerItemSdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<IList<PlayerItemResult>> Find(int? playerId = null)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            string route;

            if (playerId.HasValue)
            {
                route = $"/api/playeritems?playerId={playerId}";
            }
            else
            {
                route = "/api/playeritems";
            }

            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();

            var playerItem = await response.Content.ReadFromJsonAsync<IList<PlayerItemResult>>();

            return playerItem ?? new List<PlayerItemResult>();
        }

        public async Task<PlayerItemResult?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = $"/api/playeritems/{id}";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var playerItem = await response.Content.ReadFromJsonAsync<PlayerItemResult>();

            return playerItem;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = $"/api/playeritems/{id}";
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
