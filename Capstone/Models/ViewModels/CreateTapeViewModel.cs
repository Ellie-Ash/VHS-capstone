using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.ViewModels
{
    public class CreateTapeViewModel
    {
        public Tape Tape { get; set; }
        public IFormFile MyImage { set; get; }
    }
}
