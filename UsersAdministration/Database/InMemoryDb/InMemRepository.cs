using UsersAdministration.Models;

namespace UsersAdministration.Database.InMemoryDb;

public class InMemRepository : IInMemRepository
{
    private readonly List<User> _users = new List<User>();

    public Task CreateUser(User newUser)
    {
        _users.Add(newUser);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<User>> GetAllUsers()
    {
        return Task.FromResult(_users.AsEnumerable());
    }

    public Task<User?> GetUserByLogin(string login)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Login == login));
    }

    public Task Update(User user)
    {
        int i = _users.FindIndex(u => u.Login == user.Login);
        _users[i] = user;
        return Task.CompletedTask;
    }

    public Task<bool> Remove(string login)
    {
        User? user = _users.FirstOrDefault(u => u.Login == login);

        if (user is null)
            return Task.FromResult(false);
        _users.Remove(user);
        return Task.FromResult(true);
    }
}