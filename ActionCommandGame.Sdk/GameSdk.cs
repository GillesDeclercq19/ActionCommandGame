using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;
using System.Net.Http.Json;


namespace ActionCommandGame.Sdk
{
    public class GameSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public GameSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ServiceResult<GameResult>> PerformAction(int playerId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = $"/api/games/performAction?playerId={playerId}";
            var response = await httpClient.PostAsync(route, null);

            response.EnsureSuccessStatusCode();

            var action = await response.Content.ReadFromJsonAsync<ServiceResult<GameResult>>();
            return action ?? new ServiceResult<GameResult>();
        }

        public async Task<ServiceResult<BuyResult>> Buy(int playerId, int itemId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = $"/api/games/buy?playerId={playerId}&itemId={itemId}";
            var response = await httpClient.PostAsync(route, null);

            response.EnsureSuccessStatusCode();

            var buy = await response.Content.ReadFromJsonAsync<ServiceResult<BuyResult>>();
            return buy ?? new ServiceResult<BuyResult>();
        }
    }
}
