using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraemSheppardOnline.Services.MongoDBContext
{
    public class ArticleCollection<T> : ContextCollection<T>
    {
        public ArticleCollection(IMongoCollection<T> col) : base(col) { }


    }
}