using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int TapeId { get; set; }
        public Tape Tape { get; set; }

    }
}
