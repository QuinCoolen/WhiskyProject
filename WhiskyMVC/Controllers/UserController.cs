using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WhiskyBLL;
using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;
using WhiskyBLL.Services;
using WhiskyMVC.Models;

namespace WhiskyMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly PostService _postService;
        private readonly FavouriteService _favouriteService;
        private readonly WhiskyService _whiskyService;

        public UserController(UserService userService, PostService postService, FavouriteService favouriteService, WhiskyService whiskyService)
        {
            _userService = userService;
            _postService = postService;
            _favouriteService = favouriteService;
            _whiskyService = whiskyService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_userService.UserExists(model.Email))
                {
                    ModelState.AddModelError("", "User with this email already exists.");
                    return View(model);
                }

                var registerDto = new RegisterUserDto
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password
                };

                try
                {
                    _userService.RegisterUser(registerDto);
                    TempData["SuccessMessage"] = "Registration successful!";
                    return RedirectToAction("Login", "User");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Registration failed: " + ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserDto userDto = _userService.GetUserByEmail(model.Email);

            if (userDto == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            if(!BCrypt.Net.BCrypt.Verify(model.Password, userDto.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            Console.WriteLine("User authenticated: " + userDto.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.Name),
                new Claim(ClaimTypes.Email, userDto.Email),
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // This will create a persistent cookie
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) // Cookie expires in 7 days
            };
            // Sign in
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Profile(int? id)
        {
            int userId;

            if (id.HasValue)
            {
                // Viewing another user's profile
                userId = id.Value;
            }
            else
            {
                // Viewing your own profile
                if (User.Identity?.IsAuthenticated != true)
                {
                    return RedirectToAction("Login");
                }

                string? email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return RedirectToAction("Login");
                }

                UserDto currentUser = _userService.GetUserByEmail(email);
                if (currentUser == null)
                {
                    return RedirectToAction("Login");
                }

                userId = currentUser.Id;
            }

            UserDto userDto = _userService.GetUserById(userId);

            if (userDto == null)
            {
                return NotFound();
            }

            List<PostDto> postsDto = _postService.GetPosts().Where(p => p.UserId == userDto.Id).ToList();
            List<FavouriteDto> favouritesDto = _favouriteService.GetFavouritesByUserId(userDto.Id);

            foreach (var favourite in favouritesDto)
            {
                WhiskyDto whisky = _whiskyService.GetWhiskyById(favourite.WhiskyId);
                favourite.Whisky = whisky;
            }

            bool isCurrentUser = User.Identity.IsAuthenticated && userId.ToString() == User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            UserProfileViewModel model = new UserProfileViewModel
            {
                UserName = userDto.Name,
                Posts = postsDto.Select(post => new PostViewModel
                {
                    Id = post.Id,
                    Description = post.Description,
                    Rating = post.Rating,
                    CreatedAt = post.CreatedAt
                }).ToList(),
                Favourites = favouritesDto.Select(favourite => new FavouriteWhiskyViewModel
                {
                    Id = favourite.WhiskyId,
                    Name = favourite.Whisky?.Name ?? "Unknown",
                    Age = favourite.Whisky?.Age ?? 0,
                }).ToList(),
                IsCurrentUser = isCurrentUser
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult ViewProfile(int id)
        {
            // Fetch the user by ID
            UserDto userDto = _userService.GetUserById(id);

            if (userDto == null)
            {
                return NotFound(); // User not found
            }

            // Retrieve the user's posts
            List<PostDto> postsDto = _postService.GetPosts().Where(p => p.UserId == userDto.Id).ToList();

            // Retrieve the user's favourites
            List<FavouriteDto> favouritesDto = _favouriteService.GetFavouritesByUserId(userDto.Id);

            // Populate Whisky details in favourites
            foreach (var favourite in favouritesDto)
            {
                WhiskyDto whisky = _whiskyService.GetWhiskyById(favourite.WhiskyId);
                favourite.Whisky = whisky;
            }

            // Determine if the profile being viewed is the current user's
            bool isCurrentUser = User.Identity.IsAuthenticated && userDto.Id.ToString() == User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Create the view model
            UserProfileViewModel model = new UserProfileViewModel
            {
                UserName = userDto.Name,
                Posts = postsDto.Select(post => new PostViewModel
                {
                    Id = post.Id,
                    Description = post.Description,
                    Rating = post.Rating,
                    CreatedAt = post.CreatedAt
                }).ToList(),
                Favourites = favouritesDto.Select(favourite => new FavouriteWhiskyViewModel
                {
                    Id = favourite.WhiskyId,
                    Name = favourite.Whisky?.Name ?? "Unknown",
                    Age = favourite.Whisky?.Age ?? 0,
                }).ToList(),
                IsCurrentUser = isCurrentUser // Add this property to identify if it's the current user
            };

            return View("Profile", model); // Reuse the existing Profile view
        }
    }
}

