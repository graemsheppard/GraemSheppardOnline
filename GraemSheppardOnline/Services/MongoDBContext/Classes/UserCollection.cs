using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraemSheppardOnline.Services.MongoDBContext
{
    public class UserCollection<T> : ContextCollection<T>
    {
        public UserCollection(IMongoCollection<T> col) : base(col) { }


    }

}
