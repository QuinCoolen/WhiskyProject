using Microsoft.AspNetCore.Mvc;
using WhiskyBLL;
using WhiskyMVC.Models;

namespace WhiskyMVC.Controllers;

public class WhiskyController : Controller
{
  private readonly WhiskyService _whiskyService;

    public WhiskyController(WhiskyService whiskyService)
    {
        _whiskyService = whiskyService;
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
}