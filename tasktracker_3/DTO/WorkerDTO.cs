using System.Text.Json.Serialization;
using tasktracker_3.Models.Enums;

namespace tasktracker_3.DTO
{
    public class WorkerDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Job { get; set; } = "";
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Sex Sex { get; set; }
        public int Age { get; set; } = 0;

    }
}
