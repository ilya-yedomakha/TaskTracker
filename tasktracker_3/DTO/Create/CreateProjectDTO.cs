using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tasktracker_3.Models;

namespace tasktracker_3.DTO
{
    public class CreateProjectDTO
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Name { get; set; } = "";
        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; } = "";

        //workers
        public ICollection<long> WorkersIds { get; set; } = [];

        //tasks
        public ICollection<long> TasksIds { get; set; } = [];

    }
}
