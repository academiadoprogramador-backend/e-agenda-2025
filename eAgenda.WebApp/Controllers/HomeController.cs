using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Controllers;

[Authorize]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("erro")]
    public IActionResult Erro()
    {
        return View();
    }
}
