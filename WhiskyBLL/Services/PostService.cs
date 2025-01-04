using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;

namespace WhiskyBLL.Services
{
  public class PostService
  {
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
      _postRepository = postRepository;
    }

    public void CreatePost(PostDto post)
    {
      _postRepository.CreatePost(post);
    }

    public List<PostDto> GetPosts()
    {
      return _postRepository.GetPosts();
    }

    public PostDto GetPostById(int id)
    {
      return _postRepository.GetPostById(id);
    }

    public void UpdatePost(PostDto post)
    {
      _postRepository.UpdatePost(post);
    }

    public void DeletePost(int id)
    {
      _postRepository.DeletePost(id);
    }
  }
} 