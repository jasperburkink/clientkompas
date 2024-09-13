using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity
{
    public class BearerTokenService : IBearerTokenService
    {
        public const string SECRET_KEY = "thisisasecretkey@123"; // TODO: Secret to KeyVault
        private const string CLAIM_NAME_CVSUSERID = "CVSUserId";

        public Task<string> GenerateBearerTokenAsync(AuthenticationUser user, IList<string> roles)
        {
            try
            {
                return Task.Run(() =>
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var expires = DateTime.Now.Add(BearerTokenConstants.TOKEN_TIMEOUT);

                    var claims = new List<Claim>
                    {
                        new (JwtRegisteredClaimNames.Sub, user.Id),
                        new (JwtRegisteredClaimNames.Name, user.UserName ?? ""),
                        new (JwtRegisteredClaimNames.Email, user.Email ?? ""),
                        new (JwtRegisteredClaimNames.Exp, expires.Ticks.ToString()),
                        new (CLAIM_NAME_CVSUSERID, (user.CVSUserId ?? 0).ToString()),
                    };

                    foreach (var role in roles)
                    {
                        claims.Add(new(ClaimTypes.Role, role));
                    }

                    var token = new JwtSecurityToken(
                        issuer: BearerTokenConstants.ISSUER,
                        audience: BearerTokenConstants.AUDIENCE,
                        claims: claims,
                        expires: expires,
                        signingCredentials: signinCredentials
                    );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occured while generating the JWT Bearer token.", ex);
            }
        }

        public static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = BearerTokenConstants.ISSUER,
                ValidAudience = BearerTokenConstants.AUDIENCE,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY))
            };
        }
    }
}
