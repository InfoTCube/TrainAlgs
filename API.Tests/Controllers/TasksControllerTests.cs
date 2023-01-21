using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace API.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TasksControllerTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _mapper = A.Fake<IMapper>();
        }

        [Theory]
        [InlineData("tag")]
        public async Task TasksController_GetTask_ReturnActionResult(string nameTag)
        {
            //Arrange
            var task = A.Fake<AlgTask>();
            A.CallTo(() => _unitOfWork.TaskRepository.GetTaskByNameTagAsync(nameTag)).Returns(task);
            var controller = new TasksController(_unitOfWork, _mapper);

            //Act
            var results = await controller.GetTask(nameTag);

            //Assert
            results.Should().NotBeNull();
            results.Should().BeOfType(typeof(ActionResult<TaskDto>));
        }
    }
}