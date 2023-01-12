using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace API.Tests.Controllers;

public class ModeratorControllerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ModeratorControllerTests()
    {
        _unitOfWork = A.Fake<IUnitOfWork>();
        _mapper = A.Fake<IMapper>();
    }

/*
    [Fact]
    public async Task ModeratorController_GetTasksToVerify_ReturnActionResult()
    {
        //Arrange
        var elementParams = A.Fake<ElementParams>();
        var tasks = A.Fake<PagedList<ListedTaskDto>>();
        var Response = A.Fake<HttpResponse>();
        A.CallTo(() => _unitOfWork.TaskRepository.GetTasksToVerifyAsync(elementParams)).Returns(tasks);
        var controller = new ModeratorController(_unitOfWork, _mapper);

        //Act
        var results = await controller.GetTasksToVerify(elementParams);

        //Assert
        results.Should().NotBeNull();
        results.Should().BeOfType(typeof(ActionResult<PagedList<ListedTaskDto>>));
    }
*/

    [Theory]
    [InlineData("nameTag")]
    public async Task ModeratorController_GetTaskToVerify_ReturnActionResult(string nameTag)
    {
        //Arrange
        var task = A.Fake<AlgTask>();
        A.CallTo(() => _unitOfWork.TaskRepository.GetTaskToVerifyByNameTagAsync(nameTag)).Returns(task);
        var taskDto = A.Fake<TaskDto>();
        A.CallTo(() => _mapper.Map<TaskDto>(task)).Returns(taskDto);
        var controller = new ModeratorController(_unitOfWork, _mapper);

        //Act
        var results = await controller.GetTaskToVerify(nameTag);

        //Assert
        results.Should().NotBeNull();
        results.Should().BeOfType(typeof(ActionResult<TaskDto>));
    }

    [Theory]
    [InlineData("nameTag")]
    public async Task ModeratorController_VerifyTask_ReturnOk(string nameTag)
    {
        //Arrange
        A.CallTo(() => _unitOfWork.TaskRepository.GetTaskToVerifyByNameTagAsync(nameTag)).Returns(new AlgTask { Verified = false });
        A.CallTo(() => _unitOfWork.Complete());
        A.CallTo(() => _unitOfWork.Complete()).Returns(true);
        var controller = new ModeratorController(_unitOfWork, _mapper);

        //Act
        var results = await controller.VerifyTask(nameTag);

        //Assert
        results.Should().NotBeNull();
        results.Should().BeOfType(typeof(NoContentResult));
        A.CallTo(() => _unitOfWork.TaskRepository.Update(A<AlgTask>.That.Matches(t => t.Verified))).MustHaveHappened();
    }

}
