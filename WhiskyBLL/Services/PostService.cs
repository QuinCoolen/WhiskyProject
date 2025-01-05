using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;
using WhiskyBLL.Exceptions;

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
      if (post == null)
      {
        throw new InvalidPostDataException("Post data is invalid.");
      }

      if (string.IsNullOrWhiteSpace(post.Description))
      {
        throw new InvalidPostDataException("Post description is required.");
      }

      if (post.Rating < 1 || post.Rating > 5)
      {
        throw new InvalidPostDataException("Post rating must be between 1 and 5.");
      }

      if (post.UserId <= 0)
      {
        throw new InvalidPostDataException("User ID is required.");
      }

      if (post.WhiskyId <= 0)
      {
        throw new InvalidPostDataException("Whisky ID is required.");
      }

      if (post.Whisky == null)
      {
        throw new NotFoundException("Whisky not found.");
      }

      _postRepository.CreatePost(post);
    }

    public List<PostDto> GetPosts()
    {
      return _postRepository.GetPosts();
    }

    public PostDto GetPostById(int id)
    {
      var post = _postRepository.GetPostById(id);
      if (post == null)
      {
        throw new NotFoundException("Post not found.");
      }
      return post;
    }

    public void UpdatePost(PostDto post)
    {
      if (post == null)
      {
        throw new InvalidPostDataException("Post data is invalid.");
      }

      var existingPost = _postRepository.GetPostById(post.Id);
      if (existingPost == null)
      {
        throw new NotFoundException("Post not found.");
      }

      _postRepository.UpdatePost(post);
    }

    public void DeletePost(int id)
    {
      var existingPost = _postRepository.GetPostById(id);
      if (existingPost == null)
      {
        throw new NotFoundException("Post not found.");
      }

      _postRepository.DeletePost(id);
    }
  }
} 