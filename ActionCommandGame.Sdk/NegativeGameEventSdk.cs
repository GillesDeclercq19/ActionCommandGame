using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class NegativeGameEventSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public NegativeGameEventSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IList<NegativeGameEventResult>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = "/api/negativegameevents";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var gameEvent = await response.Content.ReadFromJsonAsync<IList<NegativeGameEventResult>>();

            if (gameEvent is null)
            {
                return new List<NegativeGameEventResult>();
            }

            return gameEvent;
        }

        public async Task<NegativeGameEventResult?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = $"/api/negativegameevents/{id}";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var gameEvent = await response.Content.ReadFromJsonAsync<NegativeGameEventResult>();

            return gameEvent;
        }

        public async Task<NegativeGameEventResult?> GetRandomNegativeGameEvent()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = "/api/negativegameevents/random";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var gameEvent = await response.Content.ReadFromJsonAsync<NegativeGameEventResult>();

            return gameEvent;
        }


        public async Task<NegativeGameEventResult?> Create(NegativeGameEventRequest gameEvent)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = "/api/negativegameevents";
            var response = await httpClient.PostAsJsonAsync(route, gameEvent);

            response.EnsureSuccessStatusCode();

            var createdGameEvent = await response.Content.ReadFromJsonAsync<NegativeGameEventResult>();

            return createdGameEvent;
        }


        public async Task<NegativeGameEventResult?> Update(int id, NegativeGameEventRequest gameEvent)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = $"/api/negativegameevents/{id}";
            var response = await httpClient.PutAsJsonAsync(route, gameEvent);

            response.EnsureSuccessStatusCode();

            var updatedGameEvent = await response.Content.ReadFromJsonAsync<NegativeGameEventResult>();

            return updatedGameEvent;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = $"/api/negativegameevents/{id}";
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
