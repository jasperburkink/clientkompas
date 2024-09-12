using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity
{
    public class BearerTokenService : IBearerTokenService
    {
        public const string SECRET_KEY = "thisisasecretkey@123"; // TODO: Secret to KeyVault

        public Task<string> GenerateBearerTokenAsync()
        {
            try
            {
                return Task.Run(() =>
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "ABCXYZ",
                        audience: "http://localhost:51398",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: signinCredentials
                    );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occured while generating the JWT bearer token.", ex);
            }
        }
    }
}
