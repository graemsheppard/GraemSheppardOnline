using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraemSheppardOnline.Models
{
    public class FileData
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsPublic { get; set; }
        public bool IsImage { get; set; }
    }
}
