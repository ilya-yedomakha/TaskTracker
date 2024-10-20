using System.ComponentModel.DataAnnotations;
using tasktracker_3.Models.Enums;

namespace tasktracker_3.DTO
{
    public class CreateTaskUnitDTO
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Title { get; set; } = "";

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        [Range(0, 5, ErrorMessage = "Status must be an integer between 0 and 5.")]
        public Status? Status { get; set; }

        [Range(0, 4, ErrorMessage = "Priority must be an integer between 0 and 4.")]
        public Priority? Priority { get; set; }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public long? ProjectId { get; set; } = null;
        public IList<long> WorkersIds { get; set; } = [];
        public IList<long> ChildTasksIds { get; set; } = [];
        public IList<long> ParentTasksIds { get; set; } = [];
    }
}
