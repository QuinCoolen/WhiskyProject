using WhiskyBLL.Dto;
using WhiskyBLL.Exceptions;
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

        var user = new UserDto
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = hashedPassword,
        };

        _userRepository.CreateUser(user);
    }
    public bool UserExists(string email)
    {
        var user = _userRepository.GetUserByEmail(email);
        return user != null;
    }

    public UserDto GetUserByEmail(string email)
    {
        try 
        {
            Console.WriteLine("GetUserByEmail called with email: " + email);
            var user = _userRepository.GetUserByEmail(email);
            
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            Console.WriteLine("User found: " + user.Email);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
            };
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve user: " + ex.Message);
        }
    }
}
