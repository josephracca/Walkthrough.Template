using Application.Constant;
using Application.DTOs.Auth;
using Application.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class AuthHelper
    {
        private static readonly int _hashIterations = 10000;
        private static readonly int _saltSize = 64;
        private static readonly int _keySize = 256;

        public AuthHelper() { }

        public static HashSaltDto GenerateSaltedHash(string password)
        {
            var salt = CreateSalt();
            var hash = Hash(password, salt);
            return new HashSaltDto { Salt = salt, Hash = hash };
        }

        public static string CreateSalt()
        {
            var saltBytes = new byte[_saltSize];
            var provider = RandomNumberGenerator.Create();
            provider.GetNonZeroBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public static string Hash(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using var algorithm = new Rfc2898DeriveBytes(password, saltBytes, _hashIterations, HashAlgorithmName.SHA256);
            var key = Convert.ToBase64String(algorithm.GetBytes(_keySize));
            return $"{_hashIterations}:{salt}:{key}";
        }

        public static bool VerifyPassword(string originalHash, string password, string salt)
        {
            var requestHash = Hash(password, salt);
            return originalHash == requestHash;
        }

        public static string DecryptPassword(string encryptedPassword, string encryptionKey)
        {
            encryptedPassword = encryptedPassword.Replace(' ', '+');

            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);

            byte[] iv = new byte[16];
            byte[] encryptedPasswordBytes = new byte[encryptedBytes.Length - 16];

            Array.Copy(encryptedBytes, iv, 16);
            Array.Copy(encryptedBytes, 16, encryptedPasswordBytes, 0, encryptedBytes.Length - 16);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
                aesAlg.IV = iv;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC; // Set the mode explicitly

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(encryptedPasswordBytes))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        public static JwtSecurityToken GenerateJWT(int userId, JwtSettings jwtSettings)
        {

            var handler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>()
            {
               new Claim(UserClaim.Id, userId.ToString()),
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );
        }
    }
}
