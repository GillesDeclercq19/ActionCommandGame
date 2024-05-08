using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActionCommandGame.RestApi.Services.Helpers;
using ActionCommandGame.RestApi.Settings;
using ActionCommandGame.Security.Model;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ActionCommandGame.RestApi.Services
{
    public class IdentityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityService(JwtSettings jwtSettings, UserManager<IdentityUser> userManager)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
        }

        public async Task<JwtAuthenticationResult> SignIn(UserSignInRequest request)
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret) || !_jwtSettings.Expiry.HasValue)
            {
                return JwtAuthenticationHelpers.JwtConfigurationError();
            }

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return JwtAuthenticationHelpers.LoginFailed();
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return JwtAuthenticationHelpers.LoginFailed();
            }

            var token = GenerateJwtToken(user, _jwtSettings.Secret, _jwtSettings.Expiry.Value);

            return new JwtAuthenticationResult()
            {
                Token = token
            };
        }

        public async Task<JwtAuthenticationResult> Register(UserRegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret) || !_jwtSettings.Expiry.HasValue)
            {
                return JwtAuthenticationHelpers.JwtConfigurationError();
            }

            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
            {
                return JwtAuthenticationHelpers.UserExists();
            }

            var user = new IdentityUser(request.Username);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return JwtAuthenticationHelpers.RegisterError(result.Errors);
            }

            var token = GenerateJwtToken(user, _jwtSettings.Secret, _jwtSettings.Expiry.Value);

            return new JwtAuthenticationResult()
            {
                Token = token
            };
        }

        private string GenerateJwtToken(IdentityUser user, string secret, TimeSpan expiry)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrWhiteSpace(user.UserName))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(expiry),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}