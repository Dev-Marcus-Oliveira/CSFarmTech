using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PIM.Models;

namespace PIM.Controllers;

public class HomeController : Controller
{
    // Redireciona a raiz para Home/Index
    [HttpGet("/")]
    public IActionResult RedirectToHome()
    {
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Index()
    {
        return View();
    }
}
