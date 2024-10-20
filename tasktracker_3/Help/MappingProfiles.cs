using AutoMapper;
using tasktracker_3.DTO;
using tasktracker_3.Models;

namespace tasktracker_3.Help
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TaskUnit, TaskUnitDTO>();
            CreateMap<Project, ProjectDTO>();
            CreateMap<Worker, WorkerDTO>();
            CreateMap<CreateTaskUnitDTO, TaskUnit>()
                 .ForMember(dest => dest.Project, opt => opt.MapFrom(src =>
                    src.ProjectId.HasValue ? new Project { Id = src.ProjectId.Value } : null))
                 .ForMember(dest => dest.Workers, opt => opt.MapFrom(src =>
                    src.WorkersIds.Select(id => new Worker { Id = id }).ToList()))
                 .ForMember(dest => dest.ParentOf, opt => opt.MapFrom(src =>
                    src.ChildTasksIds.Select(id => new TaskUnit { Id = id }).ToList()))
                 .ForMember(dest => dest.ChildOf, opt => opt.MapFrom(src =>
                    src.ParentTasksIds.Select(id => new TaskUnit { Id = id }).ToList()));
            CreateMap<CreateWorkerDTO, Worker>()
                 .ForMember(dest => dest.Projects, opt => opt.MapFrom(src =>
                    src.ProjectsIds.Select(id => new Project { Id = id }).ToList()))
                 .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src =>
                    src.TasksIds.Select(id => new TaskUnit { Id = id }).ToList()));
            CreateMap<CreateProjectDTO, Project>()
                 .ForMember(dest => dest.Workers, opt => opt.MapFrom(src =>
                    src.WorkersIds.Select(id => new Worker { Id = id }).ToList()))
                 .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src =>
                    src.TasksIds.Select(id => new TaskUnit { Id = id }).ToList()));
        }
    }
}
