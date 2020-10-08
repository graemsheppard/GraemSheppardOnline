using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraemSheppardOnline.Services.MongoDBContext
{
    public class FileCollection<T> : ContextCollection<T>
    {
        public FileCollection(IMongoCollection<T> col) : base(col) { }


    }

}
