using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraemSheppardOnline.ViewModels
{
    public class UsersVM
    {
    }

    public class UsersIndexVM
    {

        public UsersIndexVM ()
        {
            Cards = new List<UsersIndexCard>();
        }
        public List<UsersIndexCard> Cards { get; set; }

        public UsersDetailsVM Details { get; set; }

        public int Pages { get; set; }

        public int Page { get; set; }

        public string Search { get; set; }

        public class UsersIndexCard : CardBase {
            public string Id { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Status;
            
        }
    }

    public class UsersDetailsVM
    {
        public string Id { get; set; }
        public string Email { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public List<Point> Points { get; set; }
        public int Status { get; set; }
        public DateTime? LastConnection { get; set; }
    }

    public class UsersEditVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }

    public class Point { 
        public double? Latitude { get; set; }    
        public double? Longitude { get; set; }
    }


    public class UsersCreateVM
    {

    }

    public class UsersDeleteVM
    {

    }


}
