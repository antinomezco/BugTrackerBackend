using AutoMapper;
using BugTracker.DTOs.Person;
using BugTracker.DTOs.Project;
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




            CreateMap<ProjectCreationDTO, Project>()
                .ForMember(project => project.PersonnelProjects, options => options.MapFrom(MapPersonnelProjects));
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectUpdateDTO, Project>()
                .ForMember(project => project.PersonnelProjects, options => options.MapFrom(MapPersonnelUpdateProjects));
            CreateMap<Project, ProjectDTOWithPersonnel>()
                .ForMember(projectDTO => projectDTO.Personnel, options => options.MapFrom(MapProjectDTOPersonnel));
        }

        private List<PersonProject> MapPersonnelProjects(ProjectCreationDTO projectCreationDTO, Project project)
        {
            var result = new List<PersonProject>();

            if (projectCreationDTO == null)
                return result;

            foreach(var personId in projectCreationDTO.PersonnelId)
            {
                result.Add(new PersonProject { PersonId = personId });
            }

            return result;
        }

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

            foreach(var projectPerson in project.PersonnelProjects)
            {
                result.Add(new PersonDTO()
                {
                    Id = projectPerson.PersonId,
                    Name = projectPerson.Person.Name
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
                    Name = personProject.Project.Name
                });
            }
            return result;
        }
    }
}
