using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraemSheppardOnline.ViewModels
{
    public class FileManagerVM
    {
        
    }

    public class FileManagerCreateVM
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsPublic { get; set; }
        public bool IsImage { get; set; }
        public IFormFile File { get; set; }
    }

    public class FileMangerIndexVM
    {
        public List<FileIndexCard> Cards { get; set; }

       
    }

    public class FileIndexCard : CardBase
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Extension { get; set; }
    }

    public class FileManagerDetailsVM {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Extension { get; set; }
        public DateTime? UploadDate { get; set; }
       
    }
}
