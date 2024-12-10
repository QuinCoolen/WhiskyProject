using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WhiskyBLL;
using WhiskyBLL.Dto;
using WhiskyMVC.Models;

namespace WhiskyMVC.Controllers;

public class HomeController : Controller
{
    private readonly WhiskyService _whiskyService;

    public HomeController(WhiskyService whiskyService)
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
