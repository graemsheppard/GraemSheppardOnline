using System;
using GraemSheppardOnline.Services.MongoDBContext;
using Microsoft.Extensions.Configuration;

namespace GraemSheppardOnline.Code
{
    public class ContextManager
    {

        private static ContextManager _instance;

        private static readonly object _lock = new object();

        public static ContextManager GetInstance ()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ContextManager();
                        _instance.MongoContext = new MongoDBContext(ConfigManager.GetInstance());
                    }
                }
            }

            return _instance;
        }


        public MongoDBContext MongoContext { get; set; }
    }
}
