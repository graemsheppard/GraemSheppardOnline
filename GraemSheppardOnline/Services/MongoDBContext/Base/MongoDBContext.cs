using Microsoft.Extensions.Configuration;
using MongoDB.Driver;


namespace GraemSheppardOnline.Services.MongoDBContext
{

    public partial class MongoDBContext 
    {

        private IMongoDatabase _database;

        public MongoDBContext (IConfiguration configuration) {

            #region Configure database
            string connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            string databaseName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
            IMongoClient client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
            #endregion

            GetCollections();

        }

        private IMongoCollection<T> GetCollection<T>(string s)
        {
            return _database.GetCollection<T>(s);
        }
      


    }

}
