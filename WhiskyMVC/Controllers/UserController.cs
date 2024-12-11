using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;
using WhiskyMVC.Models;

namespace WhiskyMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.Name),
                new Claim(ClaimTypes.Email, userDto.Email),
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "login");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
