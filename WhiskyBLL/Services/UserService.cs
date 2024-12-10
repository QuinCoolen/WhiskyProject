using WhiskyBLL.Domain;
using WhiskyBLL.Interfaces;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void RegisterUser(RegisterUserDto dto)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new UserDomain
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = hashedPassword,
        };

        _userRepository.CreateUser(user);
    }
}
