using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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

        public UserController(UserService userService, PostService postService)
        {
            _userService = userService;
            _postService = postService;
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
                new Claim(ClaimTypes.Name, userDto.Email),
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

        public IActionResult Profile()
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string? email = User.Identity.Name;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            UserDto userDto = _userService.GetUserByEmail(email);
            var postsDto = _postService.GetPosts().Where(p => p.UserId == userDto.Id).ToList();

            var model = new UserProfileViewModel
            {
                UserName = userDto.Name,
                Posts = postsDto.Select(post => new PostViewModel
                {
                    Id = post.Id,
                    Description = post.Description,
                    Rating = post.Rating,
                    CreatedAt = post.CreatedAt
                }).ToList()
            };

            return View(model);
        }
    }
}

