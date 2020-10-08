using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;
using GraemSheppardOnline.ViewModels;
using GraemSheppardOnline.Services.MongoDBContext;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using GraemSheppardOnline.Code;
using System.Diagnostics;

namespace GraemSheppardOnline.Controllers
{
#pragma warning disable CS1998
    public class AuthenticateController : Controller
    {
        private readonly MongoDBContext _contextMongo;
        private readonly IPStackClient _ipStackClient;

        public AuthenticateController ()
        {
            _contextMongo = ContextManager.GetInstance().MongoContext;
            _ipStackClient = IPStackManager.GetInstance().Client;
        }

        public async Task<IActionResult> Index()
        {
            var test = HttpContext.Session.Id;
            return View();
        }
        

        [Route("/Authenticate")]
        public async Task<IActionResult> Authenticate ()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Email, "graem@test.com"),
            };

            var identity = new ClaimsIdentity(claims, "UserLogin");

            var principal = new ClaimsPrincipal(new[] { identity });

            await HttpContext.SignInAsync(principal);

            return Redirect("Home");

        }

        [HttpGet]
        [Route("/Login")]
        public async Task <IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [Route("/Login")]
        public async Task <IActionResult> Login(string ReturnUrl, AuthenticateVM vm)
        {
            bool success = false;
            
            try
            {
                vm.Email = vm.Email.Trim().ToLower();
                success = AuthenticateManager.TryLoginUser(vm.Email, vm.Password);
            } catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            if (!success)
            {
                return View(vm);
            }

            var user = await _contextMongo.Users.Find(x => x.Email == vm.Email).FirstOrDefaultAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, "User");

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var logUserLogin = AuthenticateManager.LogUserLogin(user.Id, ip);

            logUserLogin.Start();


            return Redirect(ReturnUrl ?? "/Home");
            

        }


        [HttpGet]
        [Route("/Register")]
        public async Task <IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> Register(AuthenticateVM vm)
        {
            if (vm.Email != null)
            {
                vm.Email = vm.Email.ToLower().Trim();
            }
            if (!ValidateEntry(vm)) { return View(vm); }

            var authToken = HttpContext.Session.Id;
            var hashed = AuthenticateManager.HashPassword(vm.Password, authToken);
            var user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                AuthToken = authToken,
                Email = vm.Email,
                Status = 0,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Role = "User"
            };

            var authenticate = new Authenticate
            {
                Id = user.Id,
                Password = hashed
            };

            try
            {
                _contextMongo.Users.InsertOne(user);
                _contextMongo.Authenticate.InsertOne(authenticate);
            }
            catch (Exception) { }
            
            return Redirect("/Login");
        }

        public async Task<IActionResult> Logout ()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Home");

        }


        private bool ValidateEntry (AuthenticateVM vm)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(vm.Email))
            {
                ModelState.AddModelError("", "Please enter an email");
            }
            
            if (string.IsNullOrWhiteSpace(vm.Password))
            {
                ModelState.AddModelError("", "Please enter a password");

            }
            if (_contextMongo.Users.AsQueryable().Where(x => x.Email == vm.Email).Any())
            {
                ModelState.AddModelError("", "An account with that email already exists");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }


        
    }
}