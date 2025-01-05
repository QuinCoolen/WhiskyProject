using NUnit.Framework;
using Moq;
using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;
using System.Collections.Generic;
using WhiskyBLL.Exceptions;
using WhiskyBLL.Services;

[TestFixture]
public class PostServiceTests
{
    private Mock<IPostRepository> _postRepositoryMock;
    private PostService _postService;

    [SetUp]
    public void Setup()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _postService = new PostService(_postRepositoryMock.Object);
    }

    [Test]
    public void CreatePost_ShouldAddPost_WhenDataIsValid()
    {
        // Arrange
        var postDto = new PostDto { UserId = 1, WhiskyId = 1, Description = "Great whisky!", Rating = 5, CreatedAt = DateTime.Now };

        // Act
        _postService.CreatePost(postDto);

        // Assert
        _postRepositoryMock.Verify(repo => repo.CreatePost(It.IsAny<PostDto>()), Times.Once);
    }

    [Test]
    public void CreatePost_ShouldThrowException_WhenWhiskyNotFound()
    {
        // Arrange
        var postDto = new PostDto { UserId = 1, WhiskyId = 999, Description = "Great whisky!", Rating = 5, CreatedAt = DateTime.Now };
        _postRepositoryMock.Setup(repo => repo.GetPostById(postDto.WhiskyId)).Returns((PostDto)null);

        // Act & Assert
        var ex = Assert.Throws<NotFoundException>(() => _postService.CreatePost(postDto));
        Assert.That(ex.Message, Is.EqualTo("Whisky not found."));
    }

    [Test]
    public void GetPostById_ShouldReturnPost_WhenPostExists()
    {
        // Arrange
        var postDto = new PostDto { Id = 1, UserId = 1, WhiskyId = 1, Description = "Great whisky!", Rating = 5, CreatedAt = DateTime.Now };
        _postRepositoryMock.Setup(repo => repo.GetPostById(postDto.Id)).Returns(postDto);

        // Act
        var result = _postService.GetPostById(postDto.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(postDto.Id));
    }

    [Test]
    public void GetPostById_ShouldThrowException_WhenPostNotFound()
    {
        // Arrange
        _postRepositoryMock.Setup(repo => repo.GetPostById(999)).Returns((PostDto)null);

        // Act & Assert
        var ex = Assert.Throws<NotFoundException>(() => _postService.GetPostById(999));
        Assert.That(ex.Message, Is.EqualTo("Post not found."));
    }

    [Test]
    public void UpdatePost_ShouldUpdatePost_WhenDataIsValid()
    {
        // Arrange
        var postDto = new PostDto { Id = 1, UserId = 1, WhiskyId = 1, Description = "Great whisky!", Rating = 5, CreatedAt = DateTime.Now };
        _postRepositoryMock.Setup(repo => repo.GetPostById(postDto.Id)).Returns(postDto);

        var updatedPostDto = new PostDto { Id = 1, UserId = 1, WhiskyId = 1, Description = "Updated description", Rating = 4, CreatedAt = DateTime.Now };

        // Act
        _postService.UpdatePost(updatedPostDto);

        // Assert
        _postRepositoryMock.Verify(repo => repo.UpdatePost(It.Is<PostDto>(p => p.Description == "Updated description" && p.Rating == 4)), Times.Once);
    }

    [Test]
    public void UpdatePost_ShouldThrowException_WhenPostNotFound()
    {
        // Arrange
        var postDto = new PostDto { Id = 1, UserId = 1, WhiskyId = 1, Description = "Great whisky!", Rating = 5, CreatedAt = DateTime.Now };
        _postRepositoryMock.Setup(repo => repo.GetPostById(postDto.Id)).Returns((PostDto)null);

        // Act & Assert
        var ex = Assert.Throws<NotFoundException>(() => _postService.UpdatePost(postDto));
        Assert.That(ex.Message, Is.EqualTo("Post not found."));
    }

    [Test]
    public void DeletePost_ShouldDeletePost_WhenPostExists()
    {
        // Arrange
        var postDto = new PostDto { Id = 1, UserId = 1, WhiskyId = 1, Description = "Great whisky!", Rating = 5, CreatedAt = DateTime.Now };
        _postRepositoryMock.Setup(repo => repo.GetPostById(postDto.Id)).Returns(postDto);

        // Act
        _postService.DeletePost(postDto.Id);

        // Assert
        _postRepositoryMock.Verify(repo => repo.DeletePost(postDto.Id), Times.Once);
    }

    [Test]
    public void DeletePost_ShouldThrowException_WhenPostNotFound()
    {
        // Arrange
        _postRepositoryMock.Setup(repo => repo.GetPostById(999)).Returns((PostDto)null);

        // Act & Assert
        var ex = Assert.Throws<NotFoundException>(() => _postService.DeletePost(999));
        Assert.That(ex.Message, Is.EqualTo("Post not found."));
    }
} 