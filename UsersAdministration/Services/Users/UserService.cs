using System.Text.RegularExpressions;
using UsersAdministration.Database.InMemoryDb;
using UsersAdministration.Dtos.UserDtos;
using UsersAdministration.Models;

namespace UsersAdministration.Services.Users;

public class UserService : IUserService
{
    private readonly IInMemRepository _inMemRepository;
    
    public UserService(IInMemRepository inMemRepository)
    {
        _inMemRepository = inMemRepository;
    }

    public bool CreateUser(UserCreationDto dto, string createdBy)
    {
        if (!IsValidLogin(dto.Login) 
            || !IsValidPassword(dto.Password) 
            || !IsValidName(dto.Name)
            || !IsUserUnique(dto.Login))
            return false;
        
        User user = new User
        {
            Guid = Guid.NewGuid(),
            Login = dto.Login,
            Password = dto.Password,
            Name = dto.Name,
            Gender = dto.Gender,
            Birthday = dto.Birthday,
            IsAdmin = dto.IsAdmin,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
        
        //TODO
        _inMemRepository.CreateUser(user);
        return true;
    }
    
    public bool UpdateName(string login, string newName, string modifiedBy)
    {
        if (!IsValidName(newName)) return false;

        User? user = GetModifiableUser(login, modifiedBy);
        if (user == null) return false;

        user.Name = newName;
        ApplyModification(user, modifiedBy);

        _inMemRepository.Update(user);
        return true;
    }
    
    public bool UpdateBirthday(string login, DateTime? newBirthday, string modifiedBy)
    {
        User? user = GetModifiableUser(login, modifiedBy);
        if (user == null) return false;

        user.Birthday = newBirthday;
        ApplyModification(user, modifiedBy);

        _inMemRepository.Update(user);
        return true;
    }
    
    public bool UpdatePassword(string login, string newPassword, string modifiedBy)
    {
        if (!IsValidPassword(newPassword)) return false;

        var user = GetModifiableUser(login, modifiedBy);
        if (user == null) return false;

        user.Password = newPassword;
        ApplyModification(user, modifiedBy);

        _inMemRepository.Update(user);
        return true;
    }
    
    public bool UpdateLogin(string oldLogin, string newLogin, string modifiedBy)
    {
        //TODO
        if (!IsValidLogin(newLogin) || !IsUserUnique(newLogin)) return false;

        var user = GetModifiableUser(oldLogin, modifiedBy);
        if (user == null) return false;

        user.Login = newLogin;
        ApplyModification(user, modifiedBy);

        _inMemRepository.Update(user);
        return true;
    }
    
    public IEnumerable<User> GetActiveUsers()
    {
        return _inMemRepository
            .GetAllUsers()
            .Where(u => u.RevokedOn == null);
    }
    
    public User? GetUserByLogin(string login)
    {
        User? user = _inMemRepository.GetUserByLogin(login);
        return user?.RevokedOn == null ? user : null;
    }
    
    public User? GetUserByLoginAndPassword(string login, string password)
    {
        //TODO
        User? user = _inMemRepository.GetUserByLogin(login);
        return (user != null && user.RevokedOn == null && user.Password == password) ? user : null;
    }
    
    public IEnumerable<User> GetUsersOlderThan(int age)
    {
        DateTime today = DateTime.UtcNow;
        return GetActiveUsers()
            .Where(u => u.Birthday.HasValue && (today.Year - u.Birthday.Value.Year) > age);
    }
    
    public bool SoftDeleteUser(string login, string deletedBy)
    {
        User? user = _inMemRepository.GetUserByLogin(login);
        User? by = _inMemRepository.GetUserByLogin(deletedBy);

        if (user == null || user.RevokedOn != null || by == null || by.RevokedOn != null)
            return false;

        user.RevokedOn = DateTime.UtcNow;
        user.RevokedBy = deletedBy;
        ApplyModification(user, deletedBy);

        _inMemRepository.Update(user);
        return true;
    }
    
    public bool HardDeleteUser(string login)
    {
        //TODO
        return _inMemRepository.Remove(login);
    }
    
    public bool RestoreUser(string login, string adminLogin)
    {
        User? admin = _inMemRepository.GetUserByLogin(adminLogin);
        User? user = _inMemRepository.GetUserByLogin(login);

        if (admin == null || !admin.IsAdmin || admin.RevokedOn != null || user == null || user.RevokedOn == null)
            return false;

        user.RevokedOn = null;
        user.RevokedBy = null;
        ApplyModification(user, adminLogin);

        _inMemRepository.Update(user);
        return true;
    }
    
    
    
    
    
    
    
    private bool IsUserUnique(string login)
    {
        if (_inMemRepository
                .GetAllUsers()
                .FirstOrDefault(u => u.Login == login) != null)
            return false;
        
        return true;
    }
    
    private bool IsValidLogin(string login)
    {
        return Regex.IsMatch(login, "^[a-zA-Z0-9]+$");
    }

    private bool IsValidPassword(string password)
    {
        return Regex.IsMatch(password, "^[a-zA-Z0-9]+$");
    }

    private bool IsValidName(string name)
    {
        return Regex.IsMatch(name, "^[a-zA-Zа-яА-ЯёЁ]+$");
    }
    
    private User? GetModifiableUser(string login, string modifiedBy)
    {
        
        User? user = _inMemRepository.GetUserByLogin(login);
        User? actor = _inMemRepository.GetUserByLogin(modifiedBy);

        if (login != modifiedBy || !actor.IsAdmin)
            return null;

        if (user == null || actor == null || user.RevokedOn != null || actor.RevokedOn != null)
            return null;

        return user;
    }
    
    private void ApplyModification(User user, string modifiedBy)
    {
        user.ModifiedBy = modifiedBy;
        user.ModifiedOn = DateTime.UtcNow;
    }
}