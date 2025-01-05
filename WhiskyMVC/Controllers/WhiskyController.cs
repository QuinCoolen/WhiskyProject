using Microsoft.AspNetCore.Mvc;
using WhiskyBLL;
using WhiskyBLL.Dto;
using WhiskyBLL.Services;
using WhiskyMVC.Models;

namespace WhiskyMVC.Controllers;

public class WhiskyController : Controller
{
  private readonly WhiskyService _whiskyService;
  private readonly FavouriteService _favouriteService;

  public WhiskyController(WhiskyService whiskyService, FavouriteService favouriteService)
  {
      _whiskyService = whiskyService;
      _favouriteService = favouriteService;
  }

  public IActionResult Index()
  {
    List<WhiskyDto> whiskysDto = _whiskyService.GetWhiskys();
    List<WhiskyViewModel> whiskys = whiskysDto.Select(whisky => new WhiskyViewModel
    {
      Id = whisky.Id,
      Name = whisky.Name,
      Age = whisky.Age,
      Year = whisky.Year,
      Country = whisky.Country,
      Region = whisky.Region
    }).ToList();

    return View(whiskys);
  }

  public IActionResult Details(int id)
  {
      var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
      bool isFavourite = false;

      if (userId != null)
      {
          isFavourite = _favouriteService.IsWhiskyInFavourites(int.Parse(userId), id);
      }

      ViewBag.IsFavourite = isFavourite;

      WhiskyDto whiskyDto = _whiskyService.GetWhiskyById(id);
      WhiskyViewModel whisky = new()
      {
          Id = whiskyDto.Id,
          Name = whiskyDto.Name,
          Age = whiskyDto.Age,
          Year = whiskyDto.Year,
          Country = whiskyDto.Country,
          Region = whiskyDto.Region
      };

      return View(whisky);
  }

  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public IActionResult Create(WhiskyViewModel whisky)
  {
    WhiskyDto whiskyDto = new()
    {
      Name = whisky.Name,
      Age = whisky.Age,
      Year = whisky.Year,
      Country = whisky.Country,
      Region = whisky.Region
    };

    _whiskyService.CreateWhisky(whiskyDto);

    return RedirectToAction("Index");
  }

  public IActionResult Edit(int id)
  {
    WhiskyDto whiskyDto = _whiskyService.GetWhiskyById(id);
    WhiskyViewModel whisky = new()
    {
      Id = whiskyDto.Id,
      Name = whiskyDto.Name,
      Age = whiskyDto.Age,
      Year = whiskyDto.Year,
      Country = whiskyDto.Country,
      Region = whiskyDto.Region
    };

    return View(whisky);
  }

  [HttpPost]
  public IActionResult Edit(WhiskyViewModel whisky)
  {
    WhiskyDto whiskyDto = new()
    {
      Id = whisky.Id,
      Name = whisky.Name,
      Age = whisky.Age,
      Year = whisky.Year,
      Country = whisky.Country,
      Region = whisky.Region
    };

    _whiskyService.UpdateWhisky(whiskyDto);

    return RedirectToAction("Index");
  }

  public IActionResult Delete(int id)
  {
    _whiskyService.DeleteWhisky(id);

    return RedirectToAction("Index");
  }
}