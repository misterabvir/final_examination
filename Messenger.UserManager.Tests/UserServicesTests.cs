using FluentAssertions;
using MassTransit;
using Messenger.Shared.Contracts.Users.Requests;
using Messenger.Shared.Contracts.Users.Responses;
using Messenger.Shared.Enums;
using Messenger.Shared.Security.Base;
using Messenger.UserManager.BusinessLogicalLayer.Errors;
using Messenger.UserManager.BusinessLogicalLayer.Services;
using Messenger.UserManager.BusinessLogicalLayer.Services.Base;
using Messenger.UserManager.DataAccessLayer.Repositories.Base;
using Messenger.UserManager.Models;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Threading;

namespace Messenger.UserManager.Tests;

public class UserServicesTests
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IEncryptService _encryptService;
    private readonly ITokenService _tokenService;
    private readonly IMapperService _mapper;
    private readonly IUserService _userService;
    private readonly IBus _bus;

    public UserServicesTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _roleRepository = Substitute.For<IRoleRepository>();
        _encryptService = Substitute.For<IEncryptService>();
        _tokenService = Substitute.For<ITokenService>();
        _mapper = Substitute.For<IMapperService>();
       _bus = Substitute.For<IBus>();
        _userService = new UserService(_userRepository, _roleRepository, _encryptService, _tokenService, _mapper, _bus);
    }


    [Fact]
    public async Task GetAllUsersSuccessShouldReturnAllUsers()
    {
        //arrange
        var expectedRole = "User";
        User expectedUser = new User() { Email = "email@email.com", Password = [], Salt = [], RoleId = Guid.Empty };

        _userRepository.GetAllAsync(default).Returns([expectedUser]);
        _mapper.Map<User, UserResponse>(Arg.Any<User>()).Returns(new UserResponse() { Id = expectedUser.Id, Email = expectedUser.Email, Role = expectedRole });

        //act
        var result = await _userService.GetAll(default);

        //assert
        await _userRepository.Received(1).GetAllAsync(default);
        _mapper.Received(1).Map<User, UserResponse>(Arg.Any<User>());

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNullOrEmpty();
        result.Value!.Should().BeOfType<List<UserResponse>>();
        result.Value!.FirstOrDefault().Should().NotBeNull();
        result.Value!.First().Id.Should().Be(expectedUser.Id);
        result.Value!.First().Email.Should().Be(expectedUser.Email);
        result.Value!.First().Role.Should().Be(expectedRole);
    }

    [Fact]
    public async Task GetIsExistShouldReturnFalesResult()
    {
        //arrange
        _userRepository.GetByIdAsync(Arg.Any<Guid>(), default).ReturnsNull();

        //act
        var result = await _userService.GetIsExist(new UserIsExistRequest("email@email.com"), default);

        //assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<UserExistingResponse>();
        result.Value!.IsExisting.Should().BeFalse();
        result.Value!.UserId.Should().BeEmpty();
    }

    [Fact]
    public async Task GetIsExistShouldReturnTrueResult()
    {
        //arrange
        _userRepository.GetByEmailAsync(Arg.Any<string>(), default)
            .Returns(new User() { Email = "email@email.com", Password = [], Salt = [], RoleId = Guid.Empty });

        //act
        var result = await _userService.GetIsExist(new UserIsExistRequest("email@email.com"), default);

        //assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<UserExistingResponse>();
        result.Value!.IsExisting.Should().BeTrue();
        result.Value!.UserId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task DeleteUserFailIfUserNotFound()
    {
        //arrange
        var user = new User() { Email = "email@email.com", Password = [], Salt = [], RoleId = Guid.Empty };
        _userRepository.GetByIdAsync(Arg.Any<Guid>(), default).ReturnsNull();    

        //act
        var result = await _userService.Delete(new UserDeleteRequest() { Id = Guid.NewGuid() }, default);

        //assert
        await _userRepository.Received(1).GetByIdAsync(Arg.Any<Guid>(), default);
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Should().BeOfType<UserNotFound>();
    }


    [Fact]
    public async Task DeleteUserSuccessShouldBeReturnTrueResult()
    {
        //arrange
        var user = new User() { Email = "email@email.com", Password = [], Salt = [], RoleId = Guid.Empty };
        _userRepository.GetByIdAsync(Arg.Any<Guid>(), default).Returns(user);
        
        //act
        var result = await _userService.Delete(new UserDeleteRequest() { Id = Guid.NewGuid() }, default);

        //assert
        await _userRepository.Received(1).GetByIdAsync(Arg.Any<Guid>(), default);
        await _userRepository.Received(1).DeleteAsync(user, default);
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<UserDeleteResponse>();
        result.Value!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task LoginFailWhenUserWithEmailNotFount()
    {
        //arrange
        var user = new User() { Email = "email@email.com", Password = [], Salt = [], RoleId = Guid.Empty };
        _userRepository.GetByEmailAsync(Arg.Any<string>(), default).ReturnsNull();

        //act
        var result = await _userService.Login(new UserAuthRequest() { Email = user.Email, Password = "password" }, default);

        //assert
        await _userRepository.Received(1).GetByEmailAsync(Arg.Any<string>(), default);
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Should().BeOfType<UserNotFound>();
    }

    [Fact]
    public async Task LoginFailWhenPasswordNotMatch()
    {
        //arrange
        var user = new User() { Email = "email@email.com", Password = [], Salt = [], RoleId = Guid.Empty };
        _userRepository.GetByEmailAsync(Arg.Any<string>(), default).Returns(user);
        _encryptService.HashPassword(Arg.Any<string>(), Arg.Any<byte[]>()).Returns(new byte[1]);

        //act
        var result = await _userService.Login(new UserAuthRequest() { Email = user.Email, Password = "password" }, default);

        //assert
        await _userRepository.Received(1).GetByEmailAsync(Arg.Any<string>(), default);
        _encryptService.Received(1).HashPassword(Arg.Any<string>(), Arg.Any<byte[]>());
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Should().BeOfType<InvalidPassword>();
    }

    [Fact]
    public async Task LoginSuccessShouldBeReturnTrueResult()
    {
        //arrange
        var user = new User() { Email = "email@email.com", Password = [], Salt = [], RoleId = Guid.Empty, Role = new Role() { RoleType = RoleType.User } };
        _userRepository.GetByEmailAsync(Arg.Any<string>(), default).Returns(user);

        _encryptService.GenerateSalt().Returns(user.Salt);
        _encryptService.HashPassword(Arg.Any<string>(), Arg.Any<byte[]>()).Returns(user.Password);

        _tokenService.GenerateToken(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>()).Returns("token");
        //act
        var result = await _userService.Login(new UserAuthRequest() { Email = user.Email, Password = "password" }, default);

        //assert
        await _userRepository.Received(1).GetByEmailAsync(Arg.Any<string>(), default);
        await _userRepository.Received(1).UpdateAsync(user, default);

        _encryptService.Received(1).GenerateSalt();
        _encryptService.Received(2).HashPassword(Arg.Any<string>(), Arg.Any<byte[]>());

        _tokenService.Received(1).GenerateToken(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<UserTokenResponse>();
        result.Value!.Token.Should().Be("token");
    }


    [Fact]
    public async Task RegisterFailedWhenEmailNotUnique()
    {
        //arrange
        var user = new User() { Email = "email@email.com", Password = [], Salt = [], RoleId = Guid.Empty, Role = new Role() { RoleType = RoleType.User } };
        _userRepository.GetByEmailAsync(Arg.Any<string>(), default).Returns(user);
        //act
        var result = await _userService.Register(new UserAuthRequest() { Email = user.Email, Password = "password" }, default);

        //assert
        await _userRepository.Received(1).GetByEmailAsync(Arg.Any<string>(), default);
        
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();

        result.Error.Should().NotBeNull();
        result.Error.Should().BeOfType<UserAlreadyExists>();
    }


    [Fact]
    public async Task RegisterSuccessWhenRegisteredFirstUserLikeAdminShouldBeReturnTrueResult()
    {
        //arrange        
        _userRepository.GetByEmailAsync(Arg.Any<string>(), default).ReturnsNull();
        _userRepository.GetAllAsync(default).Returns([]);_roleRepository.GetRoleByRoleTypeAsync(RoleType.Administrator).Returns(new Role() { RoleType = RoleType.Administrator });
        _encryptService.GenerateSalt().Returns([]);
        _encryptService.HashPassword(Arg.Any<string>(), Arg.Any<byte[]>()).Returns([]);
        _tokenService.GenerateToken(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>()).Returns("token");

        //act
        var result = await _userService.Register(new UserAuthRequest() { Email = "email", Password = "password" }, default);

        //assert
        await _userRepository.Received(1).GetByEmailAsync(Arg.Any<string>(), default);
        await _userRepository.Received(1).GetAllAsync(default);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), default);
        _encryptService.Received(1).GenerateSalt();
        _encryptService.Received(1).HashPassword(Arg.Any<string>(), Arg.Any<byte[]>());
        _tokenService.Received(1).GenerateToken(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<UserTokenResponse>();
        result.Value!.Token.Should().Be("token");
    }


    [Fact]
    public async Task RegisterSuccessWhenRegisteredNotFirstUserLikeRegularUserShouldBeReturnTrueResult()
    {
        //arrange        
        User user = new User() { Email = "email", Password = [], Salt = [], RoleId = Guid.Empty, Role = new Role() { RoleType = RoleType.User } };
        _userRepository.GetByEmailAsync(Arg.Any<string>(), default).ReturnsNull();
        _userRepository.GetAllAsync(default).Returns([user]); 
        _roleRepository.GetRoleByRoleTypeAsync(RoleType.User).Returns(new Role() { RoleType = RoleType.User });
        _encryptService.GenerateSalt().Returns([]);
        _encryptService.HashPassword(Arg.Any<string>(), Arg.Any<byte[]>()).Returns([]);
        _tokenService.GenerateToken(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>()).Returns("token");

        //act
        var result = await _userService.Register(new UserAuthRequest() { Email = "email", Password = "password" }, default);

        //assert
        await _userRepository.Received(1).GetByEmailAsync(Arg.Any<string>(), default);
        await _userRepository.Received(1).GetAllAsync(default);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), default);
        _encryptService.Received(1).GenerateSalt();
        _encryptService.Received(1).HashPassword(Arg.Any<string>(), Arg.Any<byte[]>());
        _tokenService.Received(1).GenerateToken(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<UserTokenResponse>();
        result.Value!.Token.Should().Be("token");
    }
}
