using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GraemSheppardOnline.Services.MongoDBContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Threading;

namespace GraemSheppardOnline.Code
{
    public static class AuthenticateManager
    {


        public static Func<CookieSigningOutContext, Task> SignUserOut = signOut =>
        {
           
            var userId = signOut.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var update = Builders<User>.Update.Set(x => x.Status, 0);
            return ContextManager.GetInstance().MongoContext.Users.UpdateOneAsync(x => x.Id == userId, update);
        };

        public static Func<CookieSigningInContext, Task> SignUserIn = signIn =>
        {

            var userId = signIn.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var update = Builders<User>.Update.Set(x => x.Status, 1);
            return ContextManager.GetInstance().MongoContext.Users.UpdateOneAsync(x => x.Id == userId, update);
        };

        public static string HashPassword(string password, string authToken)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < password.Length || i < authToken.Length; i++)
            {
                if (i < password.Length) { builder.Append(password[i]); }
                if (i < authToken.Length) { builder.Append(authToken[i]); }
            }
            var passwordSalt = builder.ToString();
            var passwordBytes = Encoding.UTF8.GetBytes(passwordSalt);
            var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(passwordBytes);
            builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }
            var result = builder.ToString();
            return result;
        }

        public static bool TryLoginUser(string email, string password)
        {
            var user = (from t in ContextManager.GetInstance().MongoContext.Users.AsQueryable()
                        where t.Email.ToLower() == email
                        select t).FirstOrDefault();

            if (user == null) throw new Exception("User not found");

            var auth = (from t in ContextManager.GetInstance().MongoContext.Authenticate.AsQueryable()
                        where t.Id == user.Id
                        select t).FirstOrDefault();

            if (auth == null) throw new Exception("User authentication missing");

            string hashed = HashPassword(password, user.AuthToken);

            if (hashed != auth.Password) throw new Exception("Incorrect password");

            return true;
        }

        public static Func<string, string, Task> LogUserLogin = (id, ip) =>
        {
            return new Task(async () =>
            {
                var ipStackClient = IPStackManager.GetInstance().Client;
                var contextMongo = ContextManager.GetInstance().MongoContext;
                var stackResponse = await ipStackClient.RequestInfoAsync(ip);
                var connection = new Connection
                {
                    TimeUTC = DateTime.UtcNow,
                    UserId = id,
                    Ip = ip,
                    HostName = stackResponse.HostName,
                    Latitude = stackResponse.Latitude,
                    Longitude = stackResponse.Longitude,
                    ContinentCode = stackResponse.ContinentCode,
                    ContinentName = stackResponse.ContinentName,
                    CountryCode = stackResponse.CountryCode,
                    CountryName = stackResponse.CountryName,
                    RegionCode = stackResponse.RegionCode,
                    RegionName = stackResponse.RegionName,
                    Zip = stackResponse.Zip,
                    City = stackResponse.City,

                };
                await contextMongo.Connections.InsertOneAsync(connection);
            });
        };


    }
}
