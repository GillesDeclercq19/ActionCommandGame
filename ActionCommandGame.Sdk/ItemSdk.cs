using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class ItemSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ItemSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<ItemResult>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = "/api/items";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var item = await response.Content.ReadFromJsonAsync<IList<ItemResult>>();

            if (item is null)
            {
                return new List<ItemResult>();
            }

            return item;
        }

        public async Task<ItemResult?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = $"/api/items/{id}";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var item = await response.Content.ReadFromJsonAsync<ItemResult>();

            return item;
        }

        public async Task<ItemResult?> Create(ItemRequest item)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = "/api/items";
            var response = await httpClient.PostAsJsonAsync(route, item);

            response.EnsureSuccessStatusCode();

            var createdItem = await response.Content.ReadFromJsonAsync<ItemResult>();

            return createdItem;
        }


        public async Task<ItemResult?> Update(int id, ItemRequest item)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = $"/api/items/{id}";
            var response = await httpClient.PutAsJsonAsync(route, item);

            response.EnsureSuccessStatusCode();

            var updatedItem = await response.Content.ReadFromJsonAsync<ItemResult>();

            return updatedItem;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = $"/api/items/{id}";
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
