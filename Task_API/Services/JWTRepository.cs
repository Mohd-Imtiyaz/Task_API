using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.Model;

namespace Task_API.Services
{
    public class JWTRepository : IJWTRepository
    {
        private readonly IConfiguration _configuration;
        private readonly TaskDataBaseContext _taskDataBaseContext;

        private readonly List<string> Validtoken = new List<string>();

        public JWTRepository(IConfiguration configuration, TaskDataBaseContext taskDataBaseContext)
        {
            _configuration = configuration;
            _taskDataBaseContext = taskDataBaseContext;
        }

        public async Task<string> TokenGenerate(TUser user)
        {
            if (user != null)
            {
                var resultLoginCheck = _taskDataBaseContext.TUsers
                   .Where(e => e.UName == user.UName && e.UPassword == user.UPassword)
                   .FirstOrDefault();

                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Authentication:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),

                        new Claim("UserName", user.UName),
                        new Claim("Email", user.UEmail),
                        new Claim("Name", user.UName),
                        new Claim("Roles", user.Roles),
                        new Claim("ActiveStatus", user.ActiveStatus)
                    };




                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretForKey"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Authentication:Issuer"],
                    _configuration["Authentication:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: signIn);

                var tokenreturn = new JwtSecurityTokenHandler().WriteToken(token);
                Validtoken.Add(tokenreturn);
                return tokenreturn;

            }
            else
            {
                throw new ArgumentException("Nodata");
            }
        }

        public bool TokenValid(string token)
        {
            return Validtoken.Contains(token);
        }

        public void RemoveToken(string token)
        {
            Validtoken.Remove(token);
        }
        private ActionResult<TUser> ValidateUserCredential(string? Username, string? password)
        {
            var userCred = _taskDataBaseContext.TUsers.Where(u => u.UName == Username && u.UPassword == password).FirstOrDefault();
            if (userCred == null)
            {
                throw new ArgumentException("Invlaid password");
            }
            return userCred;
        }

        public async Task<TUser> ValidateHashingPasswordAsync(string password)
        {
            using var sha256 = SHA256.Create();
            var hashbytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashpassword = Convert.ToBase64String(hashbytes);
            return await Task.FromResult(new TUser { UPassword = hashpassword });
        }

    }
}
