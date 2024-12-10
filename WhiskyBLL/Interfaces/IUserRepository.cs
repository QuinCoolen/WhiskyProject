using WhiskyBLL.Domain;

namespace WhiskyBLL.Interfaces
{
  public interface IUserRepository
  {
    void CreateUser(UserDomain user);
    // List<UserDto> GetUsers();
    // UserDto GetUserById(int id);
    // Task UpdateUser(UserDto user);
    // Task DeleteUser(int id);
  }
}