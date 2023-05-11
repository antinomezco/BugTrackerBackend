using AutoMapper;
using BugTracker.DTOs.Person;
using BugTracker.DTOs.Project;
using BugTracker.DTOs.Ticket;
using BugTracker.Entity;

namespace BugTracker.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PersonCreationDTO, Person>();
            CreateMap<PersonUpdateDTO, Person>();
            CreateMap<Person, PersonDTO>();
            CreateMap<Person, PersonPatchDTO>().ReverseMap();
            CreateMap<Person, PersonDTOWithProjects>()
                .ForMember(personDTO => personDTO.Projects, options => options.MapFrom(MapPersonDTOProjects));
            CreateMap<Person, PersonDTOWithTickets>();
            //.ForMember(personDTO => personDTO.Tickets, options => options.MapFrom(MapPersonDTOTickets));


            CreateMap<ProjectCreationDTO, Project>();
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectUpdateDTO, Project>()
                .ForMember(project => project.PersonnelProjects, options => options.MapFrom(MapPersonnelUpdateProjects));
            CreateMap<Project, ProjectDTOWithPersonnel>()
                .ForMember(projectDTO => projectDTO.Personnel, options => options.MapFrom(MapProjectDTOPersonnel))
                .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.Tickets))
                .ForMember(dest => dest.Personnel, opt => opt.MapFrom(src => src.PersonnelProjects.Select(pp => pp.Person)));


            CreateMap<TicketCreationDTO, Ticket>();
            CreateMap<Ticket, TicketDTOWithDetails>();
            CreateMap<Ticket, TicketDTO>()
                .ForMember(dest => dest.AssignedPerson, opt => opt.MapFrom(src => src.AssignedPerson.Name));
            CreateMap<TicketUpdateDTO, Ticket>();
        }

        //private List<TicketDTO> MapPersonDTOTickets(Person person, PersonDTOWithTickets personDTOWithTickets)
        //{
        //    var result = new List<TicketDTO>();

        //    if(personDTOWithTickets.Tickets == null)
        //        return result;

        //    foreach (var persTickets in personDTOWithTickets.Tickets)
        //    {
        //        result.Add(new TicketDTO
        //        { 
        //            Id = persTickets.Id,
        //            Title = persTickets.Title,
        //            Description = persTickets.Description,
        //            ProjectId = persTickets.ProjectId,
        //            ProjectName = persTickets.ProjectName,
        //        });
        //    }

        //    return result;
        //}

        private List<PersonProject> MapPersonnelUpdateProjects(ProjectUpdateDTO projectUpdateDTO, Project project)
        {
            var result = new List<PersonProject>();

            if (projectUpdateDTO == null)
                return result;

            foreach (var personId in projectUpdateDTO.PersonnelId)
            {
                result.Add(new PersonProject { PersonId = personId });
            }

            return result;
        }

        private List<PersonDTO> MapProjectDTOPersonnel(Project project, ProjectDTO projectDTO)
        {
            var result = new List<PersonDTO>();

            if (project.PersonnelProjects == null)
                return result;

            foreach (var projectPerson in project.PersonnelProjects)
            {
                result.Add(new PersonDTO()
                {
                    Id = projectPerson.PersonId,
                    Name = projectPerson.Person.Name,
                    Email = projectPerson.Person.Email,
                    Role = projectPerson.Person.Role
                });
            }

            return result;
        }

        private List<ProjectDTO> MapPersonDTOProjects(Person person, PersonDTO personDTO)
        {
            var result = new List<ProjectDTO>();

            if (person.PersonnelProjects == null)
                return result;

            foreach (var personProject in person.PersonnelProjects)
            {
                result.Add(new ProjectDTO()
                {
                    Id = personProject.ProjectId,
                    Name = personProject.Project.Name,
                    Description = personProject.Project.Description,
                    CreatedAt = personProject.Project.CreatedAt
                });
            }
            return result;
        }
    }
}
