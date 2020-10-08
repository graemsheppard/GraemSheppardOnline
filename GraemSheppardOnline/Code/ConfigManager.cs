using System;
using GraemSheppardOnline.Services.MongoDBContext;
using Microsoft.Extensions.Configuration;

namespace GraemSheppardOnline.Code
{
    public class ConfigManager
    {

        private static IConfiguration _instance;

        private static readonly object _lock = new object();

        public static IConfiguration GetInstance()
        {
            
            return _instance;
        }

        public static void SetInstance (IConfiguration configuration)
        {
            if (_instance == null)
            {
                _instance = configuration;
            }
        }
    }
}
