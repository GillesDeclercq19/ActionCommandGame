using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Security.Model.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class PositiveGameEventSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;
        public PositiveGameEventSdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<IList<PositiveGameEventResult>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = "/api/positivegameevents";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var gameEvent = await response.Content.ReadFromJsonAsync<IList<PositiveGameEventResult>>();

            if (gameEvent is null)
            {
                return new List<PositiveGameEventResult>();
            }

            return gameEvent;
        }

        public async Task<PositiveGameEventResult?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = $"/api/positivegameevents/{id}";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var gameEvent = await response.Content.ReadFromJsonAsync<PositiveGameEventResult>();

            return gameEvent;
        }

        public async Task<PositiveGameEventResult?> GetRandomPositiveGameEvent()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = "/api/positivegameevents/random";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var gameEvent = await response.Content.ReadFromJsonAsync<PositiveGameEventResult>();

            return gameEvent;
        }

        public async Task<PositiveGameEventResult?> Create(PositiveGameEventRequest gameEvent)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = "/api/positivegameevents";
            var response = await httpClient.PostAsJsonAsync(route, gameEvent);

            response.EnsureSuccessStatusCode();

            var createdGameEvent = await response.Content.ReadFromJsonAsync<PositiveGameEventResult>();

            return createdGameEvent;
        }

        public async Task<PositiveGameEventResult?> Update(int id, PositiveGameEventRequest gameEvent)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = $"/api/positivegameevents/{id}";
            var response = await httpClient.PutAsJsonAsync(route, gameEvent);

            response.EnsureSuccessStatusCode();

            var updatedGameEvent = await response.Content.ReadFromJsonAsync<PositiveGameEventResult>();

            return updatedGameEvent;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var bearerToken = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearerToken);

            var route = $"/api/positivegameevents/{id}";
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}

