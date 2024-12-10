using WhiskyBLL.Dto;

namespace WhiskyBLL.Interfaces
{
  public interface IUserRepository
  {
    void CreateUser(UserDto user);
    List<UserDto> GetUsers();
    UserDto GetUserById(int id);
    Task UpdateUser(UserDto user);
    Task DeleteUser(int id);
  }
}