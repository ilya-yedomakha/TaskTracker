using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;
using tasktracker_3.Models;

namespace tasktracker_3.DTO
{
    public class ProjectDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
