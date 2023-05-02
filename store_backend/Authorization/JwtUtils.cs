using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using store_backend.Dto;
using store_backend.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace store_backend.Authorization
{
    public class JwtUtils:IJwtUtils
    {
        private readonly AppSettings _appSettings;
        
        public JwtUtils(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GenerateJwtToken(PersonaDTO persona)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor { 
                Subject= new ClaimsIdentity(new[] {new Claim("id", persona.Id.ToString())}),
                Expires= DateTime.UtcNow.AddDays(1),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateJwtToken(string token) { 
            if (token == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var personaId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return personaId;
            }
            catch
            {
                return null;
            }
        }
    } 

    public interface IJwtUtils
    {
        public string GenerateJwtToken(PersonaDTO persona);
        public int? ValidateJwtToken(string token);
    }
}
