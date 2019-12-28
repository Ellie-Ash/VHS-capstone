using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Bio")]
        public string Bio { get; set; }

        
        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [NotMapped]
        public ICollection<Like> Likes { get; set; }
        [NotMapped]
        public ICollection<Follow> Following { get; set; }
        [NotMapped]
        public ICollection<Follow> Followers { get; set; }
        
        public virtual ICollection<Tape> Tapes { get; set; }

    }
}
