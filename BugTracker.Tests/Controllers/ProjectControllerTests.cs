using AutoMapper;
using BugTracker.Controllers;
using BugTracker.DTOs.Project;
using BugTracker.Entity;
using BugTracker.Repositories;
using BugTracker.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Xunit;
namespace BugTracker.Tests.Controllers
{
    public class ProjectControllerTests
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;

        public ProjectControllerTests()
        {
            _projectService = A.Fake<IProjectService>();
            _projectRepository = A.Fake<IProjectRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async void ProjectController_GetProjects_ReturnOk()
        {
            // Arrange
            var projects = A.Fake<List<Project>>();
            var projectDTOs = A.Fake<List<ProjectDTO>>();

            A.CallTo(() => _mapper.Map<List<ProjectDTO>>(projects)).Returns(projectDTOs);

            var controller = new ProjectsController(_projectService, _projectRepository, _mapper);

            // Act
            var result = await controller.GetProjects();

            // Assert
            //result.Value.Should().BeEquivalentTo(projectDTOs);
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ActionResult<List<ProjectDTO>>));
        }

        [Fact]
        public void ProjectController_GetProject_ReturnOk()
        {
            //Arrange
            int id = 1;
            var project = A.Fake<Project>();
            var projectDTOWDetails = A.Fake<ProjectDTOWithPersonnel>();
            A.CallTo(() => _projectRepository.GetProjectAsync(id)).Returns(project);
            A.CallTo(() => _mapper.Map<ProjectDTOWithPersonnel>(project)).Returns(projectDTOWDetails);
            var controller = new ProjectsController(_projectService, _projectRepository, _mapper);

            //Act
            var result = controller.GetProject(id);

            //Assert 
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<ActionResult<ProjectDTOWithPersonnel>>>();
        }
        [Fact]
        public async void ProjectController_GetProject_ReturnNotFound()
        {
            // Arrange
            int invalidProjectId = -1;
            A.CallTo(() => _projectRepository.GetProjectAsync(invalidProjectId)).Returns(Task.FromResult<Project>(null));
            var controller = new ProjectsController(_projectService, _projectRepository, _mapper);

            // Act
            var result = await controller.GetProject(invalidProjectId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void ProjectController_CreateProject_ReturnOk()
        {
            //Arrange
            var projectCreation = A.Fake<ProjectCreationDTO>();
            var project = A.Fake<Project>();
            var projectDTO = A.Fake<ProjectDTO>();
            A.CallTo(() => _mapper.Map<Project>(projectCreation)).Returns(project);
            A.CallTo(() => _projectRepository.PostProject(project));
            A.CallTo(() => _mapper.Map<ProjectDTO>(project)).Returns(projectDTO);

            var controller = new ProjectsController(_projectService, _projectRepository, _mapper);
            controller.Url = A.Fake<IUrlHelper>();
            A.CallTo(() => controller.Url.RouteUrl(A<UrlRouteContext>._)).Returns("fake_url");

            //Act
            var result = controller.CreateProject(projectCreation);

            //Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
        }
        [Fact]
        public void ProjectController_UpdateProject_ReturnNoContent()
        {
            //Arrange
            int id = 1;
            var projectUpdate = A.Fake<ProjectUpdateDTO>();
            var projectDB = A.Fake<Project>();

            A.CallTo(() => _projectService.UpdateProject(id)).Returns(projectDB);
            A.CallTo(() => _mapper.Map(projectUpdate, projectDB)).Returns(projectDB);

            var controller = new ProjectsController(_projectService, _projectRepository, _mapper);

            //Act
            var result = controller.UpdateProject(id, projectUpdate);

            //Assert
            result.Result.Should().BeOfType<NoContentResult>();
        }
    }
}