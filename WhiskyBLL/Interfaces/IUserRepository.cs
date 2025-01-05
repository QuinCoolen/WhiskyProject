using WhiskyBLL.Dto;

namespace WhiskyBLL.Interfaces
{
  public interface IUserRepository
  {
    void CreateUser(UserDto user);
    UserDto GetUserByEmail(string email);
    UserDto GetUserById(int id);
  }
}