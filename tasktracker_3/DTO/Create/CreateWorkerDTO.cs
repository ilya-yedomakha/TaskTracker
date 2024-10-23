using System.ComponentModel.DataAnnotations;
using tasktracker_3.Models.Enums;

namespace tasktracker_3.DTO
{
    public class CreateWorkerDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 20 characters.")]
        public string Name { get; set; } = "";

        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Surname must be between 1 and 20 characters.")]
        public string Surname { get; set; } = "";
        [Required]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "Job must be between 1 and 25 characters.")]
        public string Job { get; set; } = "";
        [Range(0, 1, ErrorMessage = "Sex must be an integer between 0 and 5.")]
        public Sex Sex { get; set; }
        [Required]
        [Range(0, 99, ErrorMessage = "Age must be between 0 and 99.")]
        public int Age { get; set; }

        // tasks
        public ICollection<long> TasksIds { get; set; } = [];
        // projects
        public ICollection<long> ProjectsIds { get; set; } = [];


    }
}
