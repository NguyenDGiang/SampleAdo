using Demo.Models;
using Demo.Services;
using DemoAdo.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View(_userService.GetAll());
        }

        public IActionResult CreateUser(UserTest userTest)
        {
            _userService.Add(userTest);
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Delete(int Id)
        {
            _userService.Delete(Id);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}