using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModelFilter.Application.Interface;
using ModelFilter.Application.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ModelFilter.Application.UseCases.Token
{
    public class GenerateTokenHandle : IJwtService
    {
        private readonly TokenSettings _settings;
        public GenerateTokenHandle(IOptions<TokenSettings> options) 
        {
            _settings = options.Value;
        }
        private string CreateAccessToken(string name, string userName, string userId)
        {
            var tokenDescription = CreateTokenDescription(1);

            var roles = new string[]
            {
                "storage.sgb.read",
                "storage.sgb.write",
                "storage.sgb.delete"
            };

            var listClaims = new List<Claim>
                {
                    //new Claim("name", name),
                    new Claim("userName",userName),
                    new Claim("userId", userId),
                    new Claim("active", "true"),
                    new Claim("rolesToUser", string.Join(",",roles))
                };

            tokenDescription.Subject = new ClaimsIdentity(listClaims);

            return CreateToken(tokenDescription);
        }
        private string CreateRefreshToken()
        {
            var tokenDescription = CreateTokenDescription(2);

            return CreateToken(tokenDescription);

        }
        private string CreateToken(SecurityTokenDescriptor tokenDescription)
        {
            var tokenHeader = new JwtSecurityTokenHandler();

            var token = tokenHeader.CreateToken(tokenDescription);

            return tokenHeader.WriteToken(token);
        }
        private SecurityTokenDescriptor CreateTokenDescription(int hoursExpiration)
        {
            var tokenDescription = new SecurityTokenDescriptor();

            //var key = Encoding.ASCII.GetBytes("85b72295-e585-44cf-b9b0-ff3ead4380f9");

            var key = Encoding.ASCII.GetBytes(_settings.Key);

            var expiration = DateTime.UtcNow.AddHours(_settings.HoursExpiresRefreshToken);
            tokenDescription.Expires = expiration;
            tokenDescription.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            return tokenDescription;

        }

        public (string acessToken, string refreshToken) Token(string name, string userName, string userId)
        {
            var accessToken = CreateAccessToken(name, userName, userId);
            var refreshToken = CreateRefreshToken();


            return (accessToken, refreshToken);
        }
    }
}
