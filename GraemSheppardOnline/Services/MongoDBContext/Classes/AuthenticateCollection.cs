using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraemSheppardOnline.Services.MongoDBContext
{
    public class AuthenticateCollection<T> : ContextCollection<T>
    {
        public AuthenticateCollection(IMongoCollection<T> col) : base(col) { }


    }



}