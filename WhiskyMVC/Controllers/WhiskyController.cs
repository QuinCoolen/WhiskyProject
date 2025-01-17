using Microsoft.AspNetCore.Mvc;
using WhiskyBLL;
using WhiskyBLL.Dto;
using WhiskyBLL.Services;
using WhiskyMVC.Models;
using WhiskyBLL.Exceptions;

namespace WhiskyMVC.Controllers;

public class WhiskyController : Controller
{
  private readonly WhiskyService _whiskyService;
  private readonly FavouriteService _favouriteService;
  private readonly ILogger<WhiskyController> _logger;

  public WhiskyController(WhiskyService whiskyService, FavouriteService favouriteService, ILogger<WhiskyController> logger)
  {
      _whiskyService = whiskyService;
      _favouriteService = favouriteService;
      _logger = logger;
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
      try
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
      catch (NotFoundException ex)
      {
          _logger.LogError(ex, "Whisky not found");
          return NotFound();
      }
      catch (WhiskyAlreadyExistsException ex)
      {
          _logger.LogError(ex, "Whisky already exists");
          return StatusCode(500, "Whisky already exists");
      }
      catch (Exception ex)
      {
          _logger.LogError(ex, "An unexpected error occurred.");
          return StatusCode(500, "An unexpected error occurred.");
      }
  }

  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public IActionResult Create(WhiskyViewModel whisky)
  {
    try
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
    catch (WhiskyAlreadyExistsException ex)
    {
      _logger.LogError(ex, "Whisky already exists");
      return StatusCode(500, "Whisky already exists");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An unexpected error occurred.");
      return StatusCode(500, "An unexpected error occurred.");
    }
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
    try
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
    catch (WhiskyAlreadyExistsException ex)
    {
      _logger.LogError(ex, "Whisky already exists");
      return StatusCode(500, "Whisky already exists");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An unexpected error occurred.");
      return StatusCode(500, "An unexpected error occurred.");
    }
  }

  public IActionResult Delete(int id)
  {
    _whiskyService.DeleteWhisky(id);

    return RedirectToAction("Index");
  }
}