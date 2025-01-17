namespace WhiskyMVC.Models
{
  public class UserProfileViewModel
  {
    public string UserName { get; set; }
    public List<PostViewModel> Posts { get; set; }
    public List<FavouriteWhiskyViewModel> Favourites { get; set; }
    public bool IsCurrentUser { get; set; }
  }
}