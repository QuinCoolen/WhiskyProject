using WhiskyBLL.Dto;

namespace WhiskyMVC.Models
{
  public class UserProfileViewModel
  {
    public string UserName { get; set; }
    public List<PostViewModel> Posts { get; set; }
  }
}