using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Security.Model.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;
using System.Net.Http.Json;


namespace ActionCommandGame.Sdk
{
    public class GameSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;
        public GameSdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<ServiceResult<GameResult>> PerformAction(int playerId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = $"/api/games/performAction?playerId={playerId}";
            var response = await httpClient.PostAsync(route, null);

            response.EnsureSuccessStatusCode();

            var action = await response.Content.ReadFromJsonAsync<ServiceResult<GameResult>>();
            return action ?? new ServiceResult<GameResult>();
        }

        public async Task<ServiceResult<BuyResult>> Buy(int playerId, int itemId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = $"/api/games/buy?playerId={playerId}&itemId={itemId}";
            var response = await httpClient.PostAsync(route, null);

            response.EnsureSuccessStatusCode();

            var buy = await response.Content.ReadFromJsonAsync<ServiceResult<BuyResult>>();
            return buy ?? new ServiceResult<BuyResult>();
        }
    }
}
