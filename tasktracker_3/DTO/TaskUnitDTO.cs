using System.Text.Json.Serialization;
using tasktracker_3.Models.Enums;

namespace tasktracker_3.DTO
{
    public class TaskUnitDTO
    {
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status? Status { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Priority? Priority { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public long? ProjectId { get; set; }
    }
}
