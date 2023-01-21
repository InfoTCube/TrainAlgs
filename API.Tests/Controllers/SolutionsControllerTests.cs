using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;
using FakeItEasy;
using Xunit;

namespace API.Tests.Controllers
{
    public class SolutionsControllerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SolutionsControllerTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _mapper = A.Fake<IMapper>();
        }
    }
}