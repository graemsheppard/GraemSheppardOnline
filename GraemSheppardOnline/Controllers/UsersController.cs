using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;
using GraemSheppardOnline.Services.MongoDBContext;
using GraemSheppardOnline.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using GraemSheppardOnline.Code;
using System.Text.Json;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Identity;

namespace GraemSheppardOnline.Controllers
{
    #pragma warning disable CS1998

    [Authorize (Roles = "Admin, Owner")]
    public class UsersController : Controller
    {
        static readonly int resultsPerPage = 8;
        private readonly MongoDBContext _mongoDBContext;

        public UsersController ()
        {
            _mongoDBContext = ContextManager.GetInstance().MongoContext;
        }

        [HttpGet]
        [Route("/Users")]
        public async Task<IActionResult> Index(UsersIndexVM vm)
        {
            vm ??= new UsersIndexVM();
            
            var query = from t in _mongoDBContext.Users.AsQueryable()
                        where t.Email.Contains(vm.Search ?? "") ||
                              t.FirstName.Contains(vm.Search ?? "") ||
                              t.LastName.Contains(vm.Search ?? "")
                        select new UsersIndexVM.UsersIndexCard
                        {
                            CardHeader = "User",
                            CardLink = "/Users/Details/" + t.Id.ToString(),
                            CardWidth = "col-md-6",
                            Id = t.Id,
                            Email = t.Email,
                            FirstName = t.FirstName,
                            LastName = t.LastName,
                            Status = t.Status
                        };

            vm.Pages = (int)Math.Ceiling(query.Count() / (double)resultsPerPage);
            vm.Cards = query.Skip(vm.Page * resultsPerPage).Take(resultsPerPage).ToList();

            return View(vm);
        }

        [HttpGet]
        [Route("/Users/Details/{id}")]
        public async Task<IActionResult> Details (string id)
        {
            if (id == null) { return RedirectToAction("Index"); }

            UsersDetailsVM vm = _mongoDBContext.Users.AsQueryable()
                                .Where(x => x.Id == id)
                                .Select(x => new UsersDetailsVM
                                {
                                    Id = x.Id,
                                    Email = x.Email,
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    Status = x.Status,
                                    Role = x.Role
                                })
                                .FirstOrDefault();

            var connections = _mongoDBContext.Connections.AsQueryable()
                              .Where(x => x.UserId == vm.Id)
                              .OrderByDescending(x => x.TimeUTC);

            vm.LastConnection = connections
                               .Select(x => x.TimeUTC)
                               .FirstOrDefault();

            

            vm.Points = connections
                       .Where(x => x.Latitude != null && x.Longitude != null)
                       .Select(x => new Point { Latitude = x.Latitude, Longitude = x.Longitude })
                       .ToList();


                                
            return View(vm);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet]
        [Route("/Users/Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) { return RedirectToAction("Index"); }

            UsersEditVM vm = (from t in _mongoDBContext.Users.AsQueryable()
                                 where t.Id == id
                                 select new UsersEditVM
                                 {
                                     Id = t.Id.ToString(),
                                     Email = t.Email,
                                     FirstName = t.FirstName,
                                     LastName = t.LastName,
                                     Role = t.Role
                                 }).FirstOrDefault();

            return View(vm);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        [Route("/Users/Edit/{id}")]
        public async Task<IActionResult> Edit([Bind("Id, Email, FirstName, LastName, Role")] UsersEditVM vm)
        {
            if (vm == null) { return RedirectToAction("Index"); }

            var updateBuilder = Builders<User>.Update;
            var update = updateBuilder.Set(x => x.Email, vm.Email)
                                      .Set(x => x.FirstName, vm.FirstName)
                                      .Set(x => x.LastName, vm.LastName)
                                      .Set(x => x.Role, vm.Role);

            await _mongoDBContext.Users.UpdateOneAsync(x => x.Id == vm.Id, update, new UpdateOptions { IsUpsert = false });

            return Redirect("/Users/Details/" + vm.Id);
        }

        public async Task<IActionResult> GetResults (string search, int? page)
        {
            var query = from t in _mongoDBContext.Users.AsQueryable()
                        where t.Email.ToLower().Contains(search ?? "") ||
                              t.FirstName.ToLower().Contains(search ?? "") ||
                              t.LastName.ToLower().Contains(search ?? "")
                        select new UsersIndexVM.UsersIndexCard
                        {
                            CardHeader = "User",
                            CardLink = "/Users/Details/" + t.Id.ToString(),
                            CardWidth = "col-md-6",
                            Id = t.Id,
                            Email = t.Email,
                            FirstName = t.FirstName,
                            LastName = t.LastName,
                            Status = t.Status
                        };

            var results = query.Skip(resultsPerPage * (page ?? 0)).Take(resultsPerPage).ToList();
            
            return PartialView("_ResultsPartial", results);
        }

        



    }
}