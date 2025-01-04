namespace WhiskyBLL.Dto
{
  public class PostDto
  {
    public int Id { get; set; }
    public int WhiskyId { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public UserDto User { get; set; }
    public WhiskyDto Whisky { get; set; }
  }
}
