using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using WhiskyBLL;
using WhiskyBLL.Dto;
using WhiskyBLL.Services;
using WhiskyMVC.Models;

namespace WhiskyMVC.Controllers
{
    public class PostController : Controller
    {
        private readonly PostService _postService;
        private readonly WhiskyService _whiskyService;

        public PostController(PostService postService, WhiskyService whiskyService)
        {
            _postService = postService;
            _whiskyService = whiskyService;
        }

        public IActionResult Index()
        {
            List<PostDto> postsDto = _postService.GetPosts();
            List<PostViewModel> posts = postsDto.Select(post => new PostViewModel
            {
                Id = post.Id,
                UserId = post.UserId,
                WhiskyId = post.WhiskyId,
                Description = post.Description,
                Rating = post.Rating,
                CreatedAt = post.CreatedAt
            }).ToList();

            return View(posts);
        }

        public IActionResult Details(int id)
        {
            PostDto postDto = _postService.GetPostById(id);

            WhiskyDto whiskyDto = _whiskyService.GetWhiskyById(postDto.WhiskyId);
            PostViewModel post = new()
            {
                Id = postDto.Id,
                UserId = postDto.UserId,
                WhiskyId = postDto.WhiskyId,
                Description = postDto.Description,
                Rating = postDto.Rating,
                CreatedAt = postDto.CreatedAt,
                Whisky = new WhiskyViewModel
                {
                    Id = whiskyDto.Id,
                    Name = whiskyDto.Name,
                    Age = whiskyDto.Age,
                    Year = whiskyDto.Year,
                    Country = whiskyDto.Country,
                    Region = whiskyDto.Region
                }
            };

            return View(post);
        }

        public IActionResult Create()
        {
            var whiskies = _whiskyService.GetWhiskys();
            ViewBag.Whiskies = whiskies;
            return View();
        }

        [HttpPost]
        public IActionResult Create(PostViewModel post)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);

            WhiskyDto whisky;

            if (post.CreateNewWhisky)
            {
                WhiskyDto whiskyDto = new WhiskyDto
                {
                    Name = post.Whisky.Name,
                    Age = post.Whisky.Age,
                    Year = post.Whisky.Year,
                    Country = post.Whisky.Country,
                    Region = post.Whisky.Region
                };

                _whiskyService.CreateWhisky(whiskyDto);
                whisky = _whiskyService.GetWhiskyByName(whiskyDto.Name);
            }
            else
            {
                whisky = _whiskyService.GetWhiskyById(post.WhiskyId);

                if (whisky == null)
                {
                    return BadRequest("Selected whisky does not exist.");
                }
            }

            PostDto postDto = new PostDto
            {
                UserId = userId,
                WhiskyId = whisky.Id,
                Description = post.Description,
                Rating = post.Rating,
                CreatedAt = DateTime.Now
            };

            _postService.CreatePost(postDto);

            return RedirectToAction("Profile", "User");
        }

        public IActionResult Edit(int id)
        {
            PostDto postDto = _postService.GetPostById(id);
            WhiskyDto whiskyDto = _whiskyService.GetWhiskyById(postDto.WhiskyId);
            PostViewModel post = new()
            {
                Id = postDto.Id,
                UserId = postDto.UserId,
                WhiskyId = postDto.WhiskyId,
                Description = postDto.Description,
                Rating = postDto.Rating,
                CreatedAt = postDto.CreatedAt,
                Whisky = new WhiskyViewModel
                {
                    Id = whiskyDto.Id,
                    Name = whiskyDto.Name,
                    Age = whiskyDto.Age,
                    Year = whiskyDto.Year,
                    Country = whiskyDto.Country,
                    Region = whiskyDto.Region
                }
            };

            return View(post);
        }

        [HttpPost]
        public IActionResult Edit(PostViewModel post)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);

            WhiskyDto whiskyDto = _whiskyService.GetWhiskyById(post.WhiskyId);

            WhiskyDto whisky = new()
            {
                Id = whiskyDto.Id,
                Name = post.Whisky.Name,
                Age = post.Whisky.Age,
                Year = post.Whisky.Year,
                Country = post.Whisky.Country,
                Region = post.Whisky.Region
            };

            _whiskyService.UpdateWhisky(whisky);

            PostDto postDto = new()
            {
                Id = post.Id,
                UserId = userId,
                WhiskyId = whisky.Id,
                Description = post.Description,
                Rating = post.Rating,
                CreatedAt = post.CreatedAt
            };

            _postService.UpdatePost(postDto);

            return RedirectToAction("Profile", "User");
        }

        public IActionResult Delete(int id)
        {
            _postService.DeletePost(id);

            return RedirectToAction("Profile", "User");
        }
    }
} 