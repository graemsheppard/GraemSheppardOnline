using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraemSheppardOnline.ViewModels
{
    public class AccountVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? LastConnection { get; set; }
        public List<Point> Points { get; set; }
    }
}
