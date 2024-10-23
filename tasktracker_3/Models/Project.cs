using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tasktracker_3.Models
{
    [Table("projects")]
    public class Project
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Name { get; set; } = "";
        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; } = "";

        //workers
        public ICollection<Worker> Workers { get; set; } = new HashSet<Worker>();

        //tasks
        public ICollection<TaskUnit> Tasks { get; set; } = new HashSet<TaskUnit>();
    }
}
