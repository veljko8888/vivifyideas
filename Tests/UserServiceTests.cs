using AutoMapper;
using NUnit.Framework;
using System.Threading.Tasks;
using testVeljkara.AutoMapper;
using testVeljkara.DbLayer;
using testVeljkara.Dtos;
using testVeljkara.Helpers;
using testVeljkara.ServiceLayer.Interfaces;
using testVeljkara.ServiceLayer.Services;

namespace Tests
{
    public class UserServiceTests
    {
        private IUserService _userService;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            ApplicationDbContext dbContext = new DbContextCreator().GetDatabaseContext();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperDefinition());
            });

            _mapper = mappingConfig.CreateMapper();

            _userService = new UserService(dbContext, _mapper);
        }

        [Test]
        public async Task InsertUserSuccess()
        {
            var user = new UserDto();
            user.Username = "veljko";
            user.Password = "veljko123";
            Result<UserDto> result = await _userService.Insert(user);
            Assert.AreEqual(ResultStatus.Created, result.Status);
        }

        [Test]
        public async Task InsertUserUsernameExists()
        {
            var user = new UserDto();
            user.Username = "veljko";
            user.Password = "veljko123";
            Result<UserDto> result = await _userService.Insert(user);
            Assert.AreEqual(ResultStatus.Created, result.Status);
            user.Password = "123Veljko";
            result = await _userService.Insert(user);
            Assert.AreEqual(ResultStatus.InvalidParameters, result.Status);
        }

        [Test]
        public async Task UsernameNotFound()
        {
            var user = new UserDto();
            user.Username = "veljko";
            user.Password = "veljko123";
            Result<UserDto> result = await _userService.Insert(user);
            Assert.AreEqual(ResultStatus.Created, result.Status);
            Result<UserDto> findUser = await _userService.FindByUsernameAsync("marko", "asd123");
            Assert.AreEqual(ResultStatus.NotFound, findUser.Status);
        }
    }
}
