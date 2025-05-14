using UsersAdministration.Dtos.UserDtos;
using UsersAdministration.Dtos.UserDtos.DeleteDtos;
using UsersAdministration.Dtos.UserDtos.GetDtos;
using UsersAdministration.Dtos.UserDtos.ReturnDtos;
using UsersAdministration.Dtos.UserDtos.UpdateDtos;
using UsersAdministration.Models;

namespace UsersAdministration.Services.Users;

public interface IUserService
{
    Task CreateUser(UserCreationDto dto, string createdBy);

    Task UpdateName(UserUpdateNameDto dto, string modifiedBy);

    Task UpdateBirthday(UserUpdateBirthdayDto dto, string modifiedBy);

    Task UpdateGender(UserUpdateGenderDto dto, string modifiedBy);

    Task UpdatePassword(UserUpdatePasswordDto dto, string modifiedBy);

    Task UpdateLogin(UserUpdateLoginDto dto, string modifiedBy);

    Task<IEnumerable<User>> GetActiveUsers(string adminLogin);

    Task<UserReturnDto> GetUserByLogin(string userLogin, string adminLogin);

    Task<User> GetUserByLoginAndPassword(UserGetByPasswordDto dto);

    Task<IEnumerable<User>> GetUsersOlderThan(int age, string adminLogin);

    Task SoftDeleteUser(UserDeleteDto dto, string deletedBy);

    Task HardDeleteUser(UserDeleteDto dto, string adminLogin);

    Task RestoreUser(UserDeleteDto dto, string adminLogin);
}