using System;
using System.Net.Http;
using GraemSheppardOnline.Models;
using GraemSheppardOnline.Services.MongoDBContext;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GraemSheppardOnline.Code
{
    public class IPStackManager
    {

        private static IPStackManager _instance;

        private static readonly object _lock = new object();

        public static IPStackManager GetInstance ()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new IPStackManager();
                        _instance.Client = new IPStackClient();
                        
                    }
                }
            }

            return _instance;
        }

        public IPStackClient Client { get; set; }
        

    }

    public class IPStackClient : HttpClient
    {

        private string _accessKey { get; set; }

        public IPStackClient ()
        {
            _accessKey = ConfigManager.GetInstance().GetValue<string>("AccessKeys:IPStack");
            BaseAddress = new Uri(ConfigManager.GetInstance().GetValue<string>("BaseAddresses:IPStack"));
            DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<IPStackResponse> RequestInfoAsync (string ip)
        {
            IPStackResponse temp = null;
            var request = $"/{ip}?access_key={_accessKey}";
            var response = await GetAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();
            temp = JsonSerializer.Deserialize<IPStackResponse>(responseJson);
                
            return temp;
        }
    }

}
