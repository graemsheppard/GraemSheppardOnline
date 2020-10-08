using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraemSheppardOnline.Code;
using GraemSheppardOnline.Services.MongoDBContext;
using GraemSheppardOnline.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using MongoDB.Driver;

namespace GraemSheppardOnline.Views
{
    [Authorize(Roles = "User, Admin, Owner")]
    public class AccountController : Controller
    {
        private readonly MongoDBContext _contextMongo;

        public AccountController ()
        {
            _contextMongo = ContextManager.GetInstance().MongoContext;
        }

        public async Task<IActionResult> Index()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrWhiteSpace(id)) { return Redirect("/Home"); }

            var user = _contextMongo.Users
                                    .AsQueryable()
                                    .Where(x => x.Id == id)
                                    .FirstOrDefault();
            var vm = new AccountVM
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id
            };

            var connections = _contextMongo.Connections.AsQueryable()
                              .Where(x => x.UserId == vm.Id)
                              .OrderByDescending(x => x.TimeUTC);

            vm.LastConnection = connections
                               .Select(x => x.TimeUTC)
                               .Skip(1)
                               .FirstOrDefault();

            vm.Points = connections
                       .Where(x => x.Latitude != null && x.Longitude != null)
                       .Select(x => new Point { Latitude = x.Latitude, Longitude = x.Longitude })
                       .ToList();

            return View(vm);
        }

        public async Task<IActionResult> Edit ()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrWhiteSpace(id)) { return Redirect("/Home"); }

            var user = _contextMongo.Users
                                    .AsQueryable()
                                    .Where(x => x.Id == id)
                                    .FirstOrDefault();
            var vm = new AccountVM
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit ([Bind("Id, FirstName, LastName")] AccountVM vm)
        {
            var updateBuilder = Builders<User>.Update;
            var update = updateBuilder.Set(x => x.FirstName, vm.FirstName)
                                      .Set(x => x.LastName, vm.LastName);

            await _contextMongo.Users.UpdateOneAsync(x => x.Id == vm.Id, update);

            return Redirect("/Account");
        }
    }
}
