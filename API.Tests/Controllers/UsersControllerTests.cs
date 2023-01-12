using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FakeItEasy;
using API.Interfaces;
using API.DTOs;
using API.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace API.Tests.Controllers;

public class UsersControllerTests
{
    private readonly IUnitOfWork _unitOfWork;
    public UsersControllerTests()
    {
        _unitOfWork = A.Fake<IUnitOfWork>();
    }

    [Theory]
    [InlineData("RandomUsername")]
    public async Task UsersController_GetUser_ReturnActionResult(string username)
    {
        //Arrange
        var user = A.Fake<MemberDto>();
        A.CallTo(() => _unitOfWork.UserRepository.GetMemberAsync(username)).Returns(user);
        var controller = new UsersController(_unitOfWork);

        //Act
        var results = await controller.GetUser(username);

        //Assert
        results.Should().NotBeNull();
        results.Should().BeOfType(typeof(ActionResult<MemberDto>));
    }
}
