using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.ViewModels
{
    public class CreateTapeViewModel
    {
        public Tape Tape { get; set; }
        [Required]
        [Display (Name ="Image")]
        public IFormFile MyImage { set; get; }
    }
}
