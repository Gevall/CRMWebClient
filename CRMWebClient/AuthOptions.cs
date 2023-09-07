using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CRMWebClient
{
    public class AuthOptions
    {
        public const string ISSUER = "CRMAuthSystem"; // Издатель токена
        public const string AUDIENCE = "CRMAuthClient"; //Клиент
        const string SecretKey = "СRM_sercret_key_for_Auth_22sf94aa31441sd322"; //Ключ для шифрования
        public const int LifeTIme = 1; // Вермя жизни токена

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        }

    }
}
