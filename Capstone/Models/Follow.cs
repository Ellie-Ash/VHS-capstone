using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Follow
    {
        [Key]
        public int FollowId { get; set; }
        [Display(Name = "User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Display(Name = "Following")]
        public string FollowerId { get; set; }
        public ApplicationUser Follower { get; set; }
    }
}
