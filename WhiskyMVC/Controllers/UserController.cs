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

        // GET: User/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        public IActionResult Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
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
    }
}
