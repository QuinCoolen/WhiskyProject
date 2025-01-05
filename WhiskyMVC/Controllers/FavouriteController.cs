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
        private readonly WhiskyService _whiskyService;

        public FavouriteController(FavouriteService favouriteService, WhiskyService whiskyService)
        {
            _favouriteService = favouriteService;
            _whiskyService = whiskyService;
        }

        public IActionResult Index(int userId)
        {
            List<FavouriteDto> favouritesDto = _favouriteService.GetFavouritesByUserId(userId);
            List<FavouriteViewModel> favourites = favouritesDto.Select(fav => new FavouriteViewModel
            {
                UserId = fav.UserId,
                WhiskyId = fav.WhiskyId
            }).ToList();

            List<WhiskyViewModel> whiskyList = new List<WhiskyViewModel>();
            foreach (var favourite in favourites)
            {
                var whiskyDto = _whiskyService.GetWhiskyById(favourite.WhiskyId);
                var whiskyViewModel = new WhiskyViewModel
                {
                    Id = whiskyDto.Id,
                    Name = whiskyDto.Name,
                    Age = whiskyDto.Age,
                    Year = whiskyDto.Year,
                    Country = whiskyDto.Country,
                    Region = whiskyDto.Region
                };
                whiskyList.Add(whiskyViewModel);
            }

            return View(whiskyList);
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

            return RedirectToAction("Index", new { userId = favourite.UserId });
        }

        [HttpPost]
        public IActionResult Remove(int userId, int whiskyId)
        {
            _favouriteService.RemoveFavourite(userId, whiskyId);

            return RedirectToAction("Index", new { userId });
        }
    }
} 