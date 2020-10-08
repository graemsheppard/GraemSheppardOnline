using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraemSheppardOnline.Code;
using GraemSheppardOnline.Models;
using GraemSheppardOnline.Services.MongoDBContext;
using GraemSheppardOnline.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GraemSheppardOnline.Controllers
{
    [Authorize(Roles = "Owner")]
    public class FileManagerController : Controller
    {

        private readonly FileManager _fileManager;
        private readonly MongoDBContext _contextMongo;
        static readonly int resultsPerPage = 8;

        public FileManagerController () {

            _fileManager = FileManager.GetInstance();
            _contextMongo = ContextManager.GetInstance().MongoContext;
        }

        public async Task <IActionResult> Index()
        {

            return View();
        }


        public async Task<IActionResult> Create ()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (FileManagerCreateVM vm)
        {
            if (_fileManager.GetMetadata(vm.Name) != null)
            {
                ModelState.AddModelError("", "A file already exists with that name");
                return View(vm);
            }

            var fileStream = vm.File.OpenReadStream();

            FileData data = new FileData
            {
                Name = vm.Name,
                DisplayName = vm.DisplayName,
                IsImage = vm.IsImage,
                IsPublic = vm.IsPublic
            };
            _fileManager.Insert(data, fileStream);
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("/FileManager/Details/{id}")]
        public async Task<IActionResult> Details (string id)
        {
            var data = _contextMongo.Files.Find(x => x.Id == id).FirstOrDefault();
            var metaData = _fileManager.GetMetadata(id);

            var vm = new FileManagerDetailsVM
            {
                DisplayName = data.DisplayName,
                Id = id,
                Name = data.Name,
                UploadDate = metaData.UploadDateTime
            };
            return View(vm);
        }

        [HttpGet]
        [Route("/FileManager/Delete/{id}")]
        public async Task<IActionResult> Delete (string id)
        {
            _fileManager.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/FileManager/Download/{id}")]
        public async Task<IActionResult> Download(string id) 
        {
            var metaData = _contextMongo.Files.Find(x => x.Id == id).FirstOrDefault();
            var fileStream = _fileManager.Get(id);
            var file = File(fileStream, "application/octet-stream", metaData.Name);
            return file;

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/Images/{id}")]
        public async Task<IActionResult> Images(string id)
        {
            var metaData = _contextMongo.Files.Find(x => x.Id == id).FirstOrDefault();
            if (!metaData.IsPublic)
            {
                return null;
            }
            var fileStream = _fileManager.Get(id);
            var file = File(fileStream, "application/octet-stream", metaData.Name);
            return file;

        }



        public async Task<IActionResult> GetResults(string search, int? page)
        {
            var query = _fileManager.Search(search);

            var results = query.Skip(resultsPerPage * (page ?? 0))
                               .Take(resultsPerPage)
                               .Select(x => new FileIndexCard
                               {

                                   Name = x.Filename,
                                   Id = x.Id.ToString(),
                                   CardHeader = "File",
                                   CardLink = "/FileManager/Details/" + x.Id
                               })
                               .ToList();

            return PartialView("_ResultsPartial", results);
        }
    }
}
