using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActionCommandGame.RestApi.Services.Helpers;
using ActionCommandGame.RestApi.Settings;
using ActionCommandGame.Security.Model;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ActionCommandGame.RestApi.Services
{
    public class IdentityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPlayerService _playerService;

        public IdentityService(JwtSettings jwtSettings, UserManager<IdentityUser> userManager, IPlayerService playerService)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _playerService = playerService;
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

            return await GenerateJwtToken(user, _jwtSettings.Secret, _jwtSettings.Expiry.Value);
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

            await _userManager.AddToRoleAsync(user, "User");

            var playerRequest = new PlayerRequest
            {
                Zeni = 100,
                Experience = 0,
                Name = request.Player, 
                UserId = user.Id
            };

            await _playerService.Create(playerRequest);

            if (!result.Succeeded)
            {
                return JwtAuthenticationHelpers.RegisterError(result.Errors);
            }

            return await GenerateJwtToken(user, _jwtSettings.Secret, _jwtSettings.Expiry.Value);
        }

        public async Task<JwtAuthenticationResult> GenerateJwtToken(IdentityUser user, string secret, TimeSpan expiry)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var userRoles = await _userManager.GetRolesAsync(user);
            var key = Encoding.ASCII.GetBytes(secret);
            var playerId = await _playerService.GetPlayerIdOfUser(user.Id);

            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("PlayerId", playerId.ToString())
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return new JwtAuthenticationResult { Token = jwtToken };
        }
    }
}