namespace WhiskyDAL.Entities
{
  public class UserEntity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
  }
}