using System.Diagnostics;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Microsoft.AspNetCore.Mvc;
using GraemSheppardOnline.Models;

using GraemSheppardOnline.Services.MongoDBContext;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using GraemSheppardOnline.Code;
using System.Collections.Generic;
using GraemSheppardOnline.ViewModels;
using System;

namespace GraemSheppardOnline.Controllers
{
    #pragma warning disable CS1998

    public class HomeController : Controller
    {

        private readonly MongoDBContext _contextMongo;

        public HomeController ()
        {
            _contextMongo = ContextManager.GetInstance().MongoContext;
        }
        public async Task<IActionResult> Index()
        {

            var articles = _contextMongo.Articles
                            .AsQueryable()
                            .Where(x => x.Page == "Home")
                            .OrderBy(x => x.Order)
                            .Select(x => new ArticleVM
                            {
                                Content = x.Content,
                                Title = x.Title,
                                Id = x.Id,
                                LastEdited = x.LastEdited
                            })
                            .ToList();

            return View(articles);

        }



        public async Task<IActionResult> Contact()
        {
            var vm = new ContactVM
            {
                Message = new EmailMessage()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Contact (ContactVM vm)
        {
            try
            {
                EmailManager.GetInstance().SendEmail(vm.Message);
            } catch (Exception e)
            {
                return Error();
            }
            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
