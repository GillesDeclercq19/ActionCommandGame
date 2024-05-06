using ActionCommandGame.Security.Model;
using ActionCommandGame.Services.Model.Core;
using Microsoft.AspNetCore.Identity;


namespace ActionCommandGame.RestApi.Services.Helpers
{
    public static class JwtAuthenticationHelpers
    {
        public static JwtAuthenticationResult LoginFailed()
        {
            return new JwtAuthenticationResult()
            {
                Messages = new List<ServiceMessage>()
                {
                    new ServiceMessage()
                    {
                        Code = "LoginFailed",
                        Message = "User/Password combination is incorrect.",
                        MessagePriority = MessagePriority.Error
                    }
                }
            };
        }

        public static JwtAuthenticationResult JwtConfigurationError()
        {
            return new JwtAuthenticationResult()
            {
                Messages = new List<ServiceMessage>()
                {
                    new ServiceMessage()
                    {
                        Code = "JwtConfigurationError",
                        Message = "JWT Settings are not configured correctly",
                        MessagePriority = MessagePriority.Error
                    }
                }
            };
        }

        public static JwtAuthenticationResult UserExists()
        {
            return new JwtAuthenticationResult()
            {
                Messages = new List<ServiceMessage>()
                {
                    new ServiceMessage()
                    {
                        Code = "UserExists",
                        Message = "This user already exists in the database",
                        MessagePriority = MessagePriority.Error
                    }
                }
            };
        }

        public static JwtAuthenticationResult RegisterError(IEnumerable<IdentityError> errors)
        {
            var jwtResult = new JwtAuthenticationResult();
            foreach (var error in errors)
            {
                jwtResult.Messages.Add(new ServiceMessage
                {
                    Code = error.Code,
                    Message = error.Description,
                    MessagePriority = MessagePriority.Error
                });
            }

            return jwtResult;
        }
    }
}
