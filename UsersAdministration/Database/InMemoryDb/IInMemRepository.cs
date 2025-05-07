using UsersAdministration.Models;

namespace UsersAdministration.Database.InMemoryDb;

public interface IInMemRepository
{
    public void CreateUser(User newUser);

    public IEnumerable<User> GetAllUsers();

    public User? GetUserByLogin(string login);

    public void Update(User user);

    public bool Remove(string login);
}