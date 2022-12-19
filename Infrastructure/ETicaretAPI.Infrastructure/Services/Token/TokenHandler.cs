using ETicaretAPI.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly private IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int second)
        {
            Application.DTOs.Token token = new();

            //Burada securityKey'in simetriği alınıyor.
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //Burada şifrelenmiş kimlik oluşturuluyor.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            //Burada oluşturulacak token ayarları veriliyor.
            token.Expiration = DateTime.UtcNow.AddMinutes(second);   //Gelen dakika değeri ile token'ın ömrü belirlenir.
            JwtSecurityToken securityToken = new(
                    audience: _configuration["Token:Audience"],      //Token'ı kimin kullanacağını belirten parametre.
                    issuer: _configuration["Token:Issuer"],          //Token'ı kimin dağıttığını belirten parametre.
                    expires: token.Expiration,                       //Token'ın ömrünü belirleyen parametre.
                    notBefore: DateTime.UtcNow,                      //Token'ın üretildikten kaç dakika sonra aktif olacağini belirleyen parametre.
                    signingCredentials: signingCredentials           //Token'ın şifrelenirken kullanacaği security key bilgisini taşıyan parametre.
                );

            //Token oluşturucu sınıfından bir örnek alınıyor.
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
