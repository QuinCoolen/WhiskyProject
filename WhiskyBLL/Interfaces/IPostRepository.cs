using WhiskyBLL.Dto;

namespace WhiskyBLL.Interfaces
{
    public interface IPostRepository
    {
        void CreatePost(PostDto post);
        List<PostDto> GetPosts();
        PostDto GetPostById(int id);
        void UpdatePost(PostDto post);
        void DeletePost(int id);
    }
} 