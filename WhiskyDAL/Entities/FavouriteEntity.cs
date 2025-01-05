namespace WhiskyDAL.Entities
{
  public class FavouriteEntity
  {
    public int UserId { get; set; }
    public int WhiskyId { get; set; }
    public UserEntity User { get; set; }
    public WhiskyEntity Whisky { get; set; }
  }
} 