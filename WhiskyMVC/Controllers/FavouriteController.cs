using Microsoft.AspNetCore.Mvc;
using WhiskyBLL;
using WhiskyBLL.Dto;
using WhiskyBLL.Services;
using WhiskyMVC.Models;

namespace WhiskyMVC.Controllers
{
    public class FavouriteController : Controller
    {
        private readonly FavouriteService _favouriteService;

        public FavouriteController(FavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        [HttpPost]
        public IActionResult Add(FavouriteViewModel favourite)
        {
            FavouriteDto favouriteDto = new()
            {
                UserId = favourite.UserId,
                WhiskyId = favourite.WhiskyId
            };

            _favouriteService.AddFavourite(favouriteDto);

            return RedirectToAction("Details", "Whisky", new { id = favourite.WhiskyId });
        }

        [HttpPost]
        public IActionResult Remove(int userId, int whiskyId)
        {
            _favouriteService.RemoveFavourite(userId, whiskyId);

            return RedirectToAction("Details", "Whisky", new { id = whiskyId });
        }
    }
} 