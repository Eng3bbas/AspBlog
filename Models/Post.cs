using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(255)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public  User User { get; set; }
        public  ICollection<PostImage> PostImages { get; set; } = new HashSet<PostImage>();
    }
}
