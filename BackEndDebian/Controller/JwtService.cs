using BackEndDebian.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BackEndDebian.Controller
{
    public class JwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(string secretKey, string issuer, string audience)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
        }

        public string GenerateToken(string username, string role, List<JwToken> jwToken1)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            int time = 1;
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(time),
                signingCredentials: credentials
            );
            JwToken jw = new JwToken() 
            { userName = username, 
              access_token = new JwtSecurityTokenHandler().WriteToken(token), 
              time_start = DateTime.Now, 
              time_end = DateTime.Now.AddMinutes(time) 
            };
            jwToken1.Add(jw);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static bool ValidateToken(string token, List<JwToken> storedToken)
        {
            var findToken = storedToken.FirstOrDefault(t => t.access_token == token);
            if (findToken == null || findToken.time_end<DateTime.Now)
            {
                return false;
            }
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes("Cifra39-Cifra39-Cifra39-Cifra39-Cifra39");
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "BackEndDebian",
                    ValidAudience = "FrontClient",
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
                SecurityToken validatedToken;
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void CleanExpiredTokens(List<JwToken> tokens)
        {
            tokens.RemoveAll(t => t.time_end < DateTime.Now);
        }
    }
}
