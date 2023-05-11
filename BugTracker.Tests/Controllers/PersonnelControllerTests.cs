using AutoMapper;
using BugTracker.Repositories;
using BugTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Xunit;
using BugTracker.Controllers;
using BugTracker.DTOs.Project;
using BugTracker.Entity;
using Microsoft.AspNetCore.Mvc;
using BugTracker.DTOs.Person;
using FluentAssertions;

namespace BugTracker.Tests.Controllers
{
    public class PersonnelControllerTests
    {
        private readonly IPersonRepository _personRepository;
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;

        public PersonnelControllerTests()
        {
            _personRepository = A.Fake<IPersonRepository>();
            _ticketService = A.Fake<ITicketService>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async void PersonnelController_GetPerson_ReturnOk()
        {
            // Arrange
            int id = 1;
            var person = A.Fake<Person>();
            var personWProjects = A.Fake<PersonDTOWithProjects>();

            A.CallTo(() => _mapper.Map<PersonDTOWithProjects>(person)).Returns(personWProjects);

            var controller = new PersonnelController(_personRepository, _ticketService, _mapper);

            // Act
            var result = await _personRepository.GetPerson(id);


            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<ActionResult<PersonDTOWithProjects>>>();
        }
    }
}
