using System.Text.RegularExpressions;
using UsersAdministration.Database.InMemoryDb;
using UsersAdministration.Dtos.UserDtos;
using UsersAdministration.Dtos.UserDtos.DeleteDtos;
using UsersAdministration.Dtos.UserDtos.GetDtos;
using UsersAdministration.Dtos.UserDtos.ReturnDtos;
using UsersAdministration.Dtos.UserDtos.UpdateDtos;
using UsersAdministration.Exceptions;
using UsersAdministration.Models;

namespace UsersAdministration.Services.Users;

public class UserService : IUserService
{
    private readonly IInMemRepository _inMemRepository;
    
    public UserService(IInMemRepository inMemRepository)
    {
        _inMemRepository = inMemRepository;
    }

    public async Task CreateUser(UserCreationDto dto, string createdBy)
    { 
        IsValidLogin(dto.Login);
        IsValidPassword(dto.Password);
        IsValidName(dto.Name);
        await IsUserUnique(dto.Login);

        if (!(await IsUserAdmin(createdBy)) && dto.IsAdmin)
            throw new ForbiddenException("Not admin to make admins");
        
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
        
        await _inMemRepository.CreateUser(user);
    }
    
    public async Task UpdateName(UserUpdateNameDto dto, string modifiedBy)
    {
        IsValidName(dto.NewName);

        User? user = await GetModifiableUser(dto.UserLogin, modifiedBy);
        if (user == null) throw new NotFoundException("user not found");

        user.Name = dto.NewName;
        ApplyModification(user, modifiedBy);

        await _inMemRepository.Update(user);
    }
    
    public async Task UpdateBirthday(UserUpdateBirthdayDto dto, string modifiedBy)
    {
        User? user = await GetModifiableUser(dto.UserLogin, modifiedBy);
        if (user == null) throw new NotFoundException("user not found");

        user.Birthday = dto.NewDate;
        ApplyModification(user, modifiedBy);

        await _inMemRepository.Update(user);
    }

    public async Task UpdateGender(UserUpdateGenderDto dto, string modifiedBy)
    {
        IsGenderValid(dto.NewGender);
        User? user = await GetModifiableUser(dto.UserLogin, modifiedBy);

        if (user == null) throw new NotFoundException("user not found");

        user.Gender = dto.NewGender;
        ApplyModification(user, modifiedBy);
        
        await _inMemRepository.Update(user);
    }
    
    public async Task UpdatePassword(UserUpdatePasswordDto dto, string modifiedBy)
    {
        IsValidPassword(dto.NewPassword);

        User? user = await GetModifiableUser(dto.UserLogin, modifiedBy);
        if (user == null) throw new NotFoundException("user not found");

        user.Password = dto.NewPassword;
        ApplyModification(user, modifiedBy);

        await _inMemRepository.Update(user);
    }
    
    public async Task UpdateLogin(UserUpdateLoginDto dto, string modifiedBy)
    {
        IsValidLogin(dto.NewLogin);
        await IsUserUnique(dto.NewLogin);

        User? user = await GetModifiableUser(dto.UserOldLogin, modifiedBy);
        if (user == null) throw new NotFoundException("user not found");

        user.Login = dto.NewLogin;
        ApplyModification(user, modifiedBy);

        await _inMemRepository.Update(user);
    }
    
    public async Task<IEnumerable<User>> GetActiveUsers(string adminLogin)
    {
        await EnsureUserAdmin(adminLogin);
        IEnumerable<User> result = await _inMemRepository.GetAllUsers();
            result = result.Where(u => u.RevokedOn == null)
            .OrderBy(u => u.CreatedOn);

        return result;
    }
    
    public async Task<UserReturnDto> GetUserByLogin(string userLogin, string adminLogin)
    {
        await EnsureUserAdmin(adminLogin);
        User? user = await _inMemRepository.GetUserByLogin(userLogin);
        if (user == null || user.RevokedOn != null)
            throw new NotFoundException("Bad user");
        UserReturnDto result = new UserReturnDto
        {
            Name = user.Name,
            Gender = user.Gender,
            Birthday = user.Birthday,
            Active = user.RevokedOn == null,
        };
        
        return result;
    }
    
    public async Task<User> GetUserByLoginAndPassword(UserGetByPasswordDto dto)
    {
        User? user = await _inMemRepository.GetUserByLogin(dto.Login);
        if (user == null || user.RevokedOn != null)
            throw new NotFoundException("Bad user");

        if (user.Password != dto.Password)
            throw new ForbiddenException("wrong password");

        return user;
    }
    
    public async Task<IEnumerable<User>> GetUsersOlderThan(int age, string adminLogin)
    {
        DateTime today = DateTime.UtcNow;

        IEnumerable<User> activeUsers = await _inMemRepository.GetAllUsers();
        activeUsers = activeUsers.Where(u => u.RevokedOn == null);
        IEnumerable<User> result = activeUsers
            .Where(u => u.Birthday.HasValue && (today.Year - u.Birthday.Value.Year) > age);

        return result;
    }
    
    public async Task SoftDeleteUser(UserDeleteDto dto, string deletedBy)
    { 
        await EnsureUserAdmin(deletedBy);
        
        User? user = await _inMemRepository.GetUserByLogin(dto.UserLogin);

        if (user == null || user.RevokedOn != null)
            throw new NotFoundException("User not found or already deleted");

        user.RevokedOn = DateTime.UtcNow;
        user.RevokedBy = deletedBy;
        ApplyModification(user, deletedBy);

        await _inMemRepository.Update(user);
    }
    
    public async Task HardDeleteUser(UserDeleteDto dto, string adminLogin)
    {
        await EnsureUserAdmin(adminLogin);
        
        await _inMemRepository.Remove(dto.UserLogin);
    }
    
    public async Task RestoreUser(UserDeleteDto dto, string adminLogin)
    {
        User? user = await _inMemRepository.GetUserByLogin(dto.UserLogin);

        await EnsureUserAdmin(adminLogin);
        if (user == null
            || user.RevokedOn == null)
            throw new NotFoundException("User not found or did not delete");

        user.RevokedOn = null;
        user.RevokedBy = null;
        ApplyModification(user, adminLogin);

        await _inMemRepository.Update(user);
    }
    
    private async Task IsUserUnique(string login)
    {
        IEnumerable<User> users = await _inMemRepository
            .GetAllUsers();
        if (users
                .FirstOrDefault(u => u.Login == login) != null)
            throw new ValidationException("Login not unique");
        
    }
    
    private void IsValidLogin(string login)
    {
        if (!Regex.IsMatch(login, "^[a-zA-Z0-9]+$"))
            throw new ValidationException("login not valid");
    }

    private void IsValidPassword(string password)
    {
        if (!Regex.IsMatch(password, "^[a-zA-Z0-9]+$"))
            throw new ValidationException("password not valid");
    }

    private void IsValidName(string name)
    {
        if (!Regex.IsMatch(name, "^[a-zA-Zа-яА-ЯёЁ]+$"))
            throw new ValidationException("Name not valid");
    }

    private void IsGenderValid(int gender)
    {
        if (gender > 2 || gender < 0) 
            throw new ValidationException("wrong gender");
    }
    
    private async Task<User?> GetModifiableUser(string login, string modifiedBy)
    {
        
        User? user = await _inMemRepository.GetUserByLogin(login);
        User? actor = await _inMemRepository.GetUserByLogin(modifiedBy);

        if (login != modifiedBy && !actor.IsAdmin)
            throw new ForbiddenException("Not admin");

        if (user == null || actor == null || user.RevokedOn != null || actor.RevokedOn != null)
            throw new NotFoundException("User not found or deleted");

        return user;
    }
    
    private void ApplyModification(User user, string modifiedBy)
    {
        user.ModifiedBy = modifiedBy;
        user.ModifiedOn = DateTime.UtcNow;
    }

    private async Task EnsureUserAdmin(string login)
    {
        User? user = await _inMemRepository.GetUserByLogin(login);
        if (user is null || !user.IsAdmin || user.RevokedOn != null) 
            throw new ForbiddenException("Not admin");
    }

    private async Task<bool> IsUserAdmin(string login)
    {
        User? user = await _inMemRepository.GetUserByLogin(login);
        if (user is null) throw new NotFoundException("no user");

        return user.IsAdmin;
    }
}