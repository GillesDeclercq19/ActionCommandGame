namespace ActionCommandGame.Sdk.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient AddAuthorization(this HttpClient httpClient, string? bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return httpClient;
            }

            if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                httpClient.DefaultRequestHeaders.Remove("Authorization");
            }
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");

            return httpClient;
        }
    }
}
