using UsersAdministration.Models;

namespace UsersAdministration.Database.InMemoryDb;

public class InMemRepository : IInMemRepository
{
    private readonly List<User> _users = new List<User>();

    public void CreateUser(User newUser)
    {
        _users.Add(newUser);
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _users;
    }

    public User? GetUserByLogin(string login)
    {
        return _users.FirstOrDefault(u => u.Login == login);
    }

    public void Update(User user)
    {
        int i = _users.FindIndex(u => u.Login == user.Login);
        _users[i] = user;
    }

    public bool Remove(string login)
    {
        User? user = _users.FirstOrDefault(u => u.Login == login);

        if (user is null)
            return false;
        _users.Remove(user);
        return true;
    }
}