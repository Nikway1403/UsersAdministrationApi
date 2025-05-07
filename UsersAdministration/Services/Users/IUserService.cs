using UsersAdministration.Dtos.UserDtos;
using UsersAdministration.Models;

namespace UsersAdministration.Services.Users;

public interface IUserService
{
    bool CreateUser(UserCreationDto dto, string createdBy);

    bool UpdateName(string login, string newName, string modifiedBy);

    bool UpdateBirthday(string login, DateTime? newBirthday, string modifiedBy);

    bool UpdatePassword(string login, string newPassword, string modifiedBy);

    bool UpdateLogin(string oldLogin, string newLogin, string modifiedBy);

    IEnumerable<User> GetActiveUsers();

    User? GetUserByLogin(string login);

    User? GetUserByLoginAndPassword(string login, string password);

    IEnumerable<User> GetUsersOlderThan(int age);

    bool SoftDeleteUser(string login, string deletedBy);

    bool HardDeleteUser(string login);

    bool RestoreUser(string login, string adminLogin);
}