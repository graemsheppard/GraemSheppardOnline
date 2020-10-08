using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraemSheppardOnline.Services.MongoDBContext
{
    public class ConnectionCollection<T> : ContextCollection<T>
    {
        public ConnectionCollection(IMongoCollection<T> col) : base(col) { }


    }

}
