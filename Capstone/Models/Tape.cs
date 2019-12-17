using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Tape
    {
        [Key]
        public int TapeId { get; set; }
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Owner")]
        public ApplicationUser User { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set;}
        public string Genre { get; set; }
        public int Year { get; set; }
        [Display(Name = "Image")]
        public string ImagePath { get; set; }
       
        public string Link { get; set; }
        [Display(Name = "Available For Sale/Trade")]
        public bool isAvailable { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
