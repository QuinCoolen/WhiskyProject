namespace WhiskyBLL.Dto
{
  public class UserDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
  }
}