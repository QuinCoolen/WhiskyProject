namespace WhiskyBLL.Dto
{
  public class FavouriteDto
  {
    public int UserId { get; set; }
    public int WhiskyId { get; set; }
    public UserDto User { get; set; }
    public WhiskyDto Whisky { get; set; }
  }
} 