using UsersAdministration.Models;

namespace UsersAdministration.Database.InMemoryDb;

public interface IInMemRepository
{
    Task CreateUser(User newUser);

    Task<IEnumerable<User>> GetAllUsers();

    Task<User?> GetUserByLogin(string login);

    Task Update(User user);

    Task<bool> Remove(string login);
}