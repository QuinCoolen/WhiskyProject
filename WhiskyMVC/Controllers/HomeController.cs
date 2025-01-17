using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WhiskyBLL;
using WhiskyBLL.Dto;
using WhiskyMVC.Models;
using System.Linq;
using WhiskyBLL.Interfaces;
using WhiskyBLL.Services;
using Microsoft.Extensions.Logging;

namespace WhiskyMVC.Controllers;

public class HomeController : Controller
{
    private readonly WhiskyService _whiskyService;
    private readonly PostService _postService;
    private readonly UserService _userService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(WhiskyService whiskyService, PostService postService, UserService userService, ILogger<HomeController> logger)
    {
        _whiskyService = whiskyService;
        _postService = postService;
        _userService = userService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<PostDto> postsDto = _postService.GetPosts();
        List<PostViewModel> posts = postsDto.Select(post => 
        {
            UserDto userDto = _userService.GetUserById(post.UserId);

            return new PostViewModel
            {
                Id = post.Id,
                UserId = post.UserId,
                WhiskyId = post.WhiskyId,
                Description = post.Description,
                Rating = post.Rating,
                CreatedAt = post.CreatedAt,
                User = new UserProfileViewModel // Create UserProfileViewModel to hold username
                {
                    UserName = userDto?.Name // Safely access the username
                }
            };
        }).ToList();

        return View(posts);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
