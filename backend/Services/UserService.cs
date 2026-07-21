using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Settings;

namespace WebAPI.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);

        _users = database.GetCollection<User>("Users");
    }

public bool Register(WebAPI.DTOs.RegisterDto dto)
    {
        var existingUser = _users.Find(x => x.Username == dto.Username).FirstOrDefault();

        if (existingUser != null)
            return false;

        var user = new User
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Age = dto.Age,
            Gender = dto.Gender,
            Email = dto.Email,
            Username = dto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)       
         };

        _users.InsertOne(user);

        return true;
    }

public User? Login(WebAPI.DTOs.LoginRequest request)
    {
        var user = _users.Find(x => x.Username == request.Username)
                     .FirstOrDefault();
         if (user == null)
        return null;
        bool result = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        if (!result)
        return null;
        return user;
    }
}