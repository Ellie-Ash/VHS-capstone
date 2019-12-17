using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.ViewModels
{
    public class AllUsersViewModel
    {
        public ApplicationUser User { get; set; }
        public List<ApplicationUser> AllUsers { get; set; }
    }
}
