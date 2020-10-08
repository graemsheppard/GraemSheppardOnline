using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraemSheppardOnline.ViewModels
{
    public class ArticleVM
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? LastEdited { get; set; }
    }
}
