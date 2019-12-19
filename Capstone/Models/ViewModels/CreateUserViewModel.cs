using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.ViewModels
{
    public class CreateUserViewModel
    {
        public ApplicationUser User { get; set; }
        
        public IFormFile MyImage { set; get; }
    }
}
