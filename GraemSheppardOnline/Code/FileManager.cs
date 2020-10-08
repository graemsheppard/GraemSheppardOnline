using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraemSheppardOnline.Models;
using GraemSheppardOnline.Services.MongoDBContext;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;


namespace GraemSheppardOnline.Code
{
    public class FileManager
    {

        private static FileManager _instance;

        private static readonly object _lock = new object();

        private GridFSBucket _bucket { get; set; }

        private MongoDBContext _contextMongo;

        public static FileManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        var config = ConfigManager.GetInstance();
                        string connectionString = config.GetValue<string>("DatabaseSettings:ConnectionString");
                        string databaseName = config.GetValue<string>("DatabaseSettings:DatabaseName");
                        var client = new MongoClient(connectionString);
                        var database = client.GetDatabase(databaseName);

                        _instance = new FileManager
                        {
                            _contextMongo = ContextManager.GetInstance().MongoContext,
                            _bucket = new GridFSBucket(database, new GridFSBucketOptions
                            {
                                BucketName = "Files",
                                ChunkSizeBytes = 1048576
                            })
                        };

                    }
                }
            }

            return _instance;
        }

        public string Insert(FileData data, Stream stream)
        {
            var id = _bucket.UploadFromStream(data.Name, stream);
            var fileData = new Services.MongoDBContext.File
            {
                Id = id.ToString(),
                DisplayName = data.DisplayName,
                Name = data.Name,
                IsImage = data.IsImage,
                IsPublic = data.IsPublic
            };
            _contextMongo.Files.InsertOne(fileData);
            return id.ToString();
        }

        public GridFSFileInfo GetMetadata (string search) {
            var filterBuilder = Builders<GridFSFileInfo>.Filter;

            ObjectId id = default;
            try
            {
                id = new ObjectId(search);
            } catch (Exception) { }

            var filter = filterBuilder.Eq("_id",  id);

            var result = _bucket.Find(filter).FirstOrDefault();
            return result;
        }

        public Stream Get(string id)
        {

            var stream = _bucket.OpenDownloadStream(new ObjectId(id));
            return stream;

        }

        public List<GridFSFileInfo> Search (string search = null)
        {
            FilterDefinition<GridFSFileInfo> filter;
            if (string.IsNullOrWhiteSpace(search))
            {
                filter = Builders<GridFSFileInfo>.Filter.Empty;
            } else
            {
                filter = Builders<GridFSFileInfo>.Filter.Regex(x => x.Filename, search);
            }
            
            var result = _bucket.Find(filter).ToList();

            return result;
        }

        public async void Delete(string id)
        {

            await _bucket.DeleteAsync(new ObjectId(id));
            await _contextMongo.Files.DeleteOneAsync(x => x.Id == id);
        }
    }
}
