using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GraemSheppardOnline.Services.MongoDBContext;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using GraemSheppardOnline.Code;

namespace GraemSheppardOnline.Controllers
{
    public class TestController : Controller
    {
        private readonly FileManager _fileManager;
        private readonly MongoDBContext _contextMongo;
        private readonly EmailManager _emailManager;

        public TestController ()
        {
            _contextMongo = ContextManager.GetInstance().MongoContext;
            _fileManager = FileManager.GetInstance();
            _emailManager = EmailManager.GetInstance();
        }



    }
}
