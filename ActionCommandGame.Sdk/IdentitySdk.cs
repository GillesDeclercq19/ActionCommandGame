﻿using System.Net.Http.Json;
using ActionCommandGame.Security.Model;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;


namespace ActionCommandGame.Sdk
{
    public class IdentitySdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentitySdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<JwtAuthenticationResult> SignIn(UserSignInRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = "/api/identity/sign-in";
            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JwtAuthenticationResult>();

            if (result is null)
            {
                return new JwtAuthenticationResult()
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage { Code = "ApiError", Message = "An API error occurred." }
                    }
                };
            }

            return result;
        }

        public async Task<JwtAuthenticationResult> Register(UserRegisterRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandApi");
            var route = "/api/identity/register";
            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JwtAuthenticationResult>();

            if (result is null)
            {
                return new JwtAuthenticationResult()
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage { Code = "ApiError", Message = "An API error occurred." }
                    }
                };
            }

            return result;
        }
    }
}
