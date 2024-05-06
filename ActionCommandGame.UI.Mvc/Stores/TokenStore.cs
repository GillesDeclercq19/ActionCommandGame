using ActionCommandGame.Security.Model.Abstractions;

namespace ActionCommandGame.UI.Mvc.Stores
{
    public class TokenStore: ITokenStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetToken()
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return null;
            }

            if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("Token", out string? token))
            {
                return token;
            }

            return null;
        }

        public void SaveToken(string? bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return;
            }

            if (_httpContextAccessor.HttpContext is null)
            {
                return;
            }

            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("Token"))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("Token");
            }

            _httpContextAccessor.HttpContext.Response.Cookies.Append("Token", bearerToken, new CookieOptions { HttpOnly = true });
        }
    }
}
