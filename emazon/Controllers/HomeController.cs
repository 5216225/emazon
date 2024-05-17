using emazon.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace emazon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            // Display login view
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            // Perform authentication (e.g., validate credentials)
            if (ModelState.IsValid)
            {
                // Example: Check if username and password match in your authentication mechanism
                if (model.Username == "demo" && model.Password == "password")
                {
                    // Authentication successful, sign in the user (e.g., using cookies)
                    // For demonstration purpose, signing in user using cookies
                    var claims = new[] { new Claim(ClaimTypes.Name, model.Username) };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    // Authentication failed, display error or redirect back to login page
                    ModelState.AddModelError("", "Invalid username or password");
                    return View(model);
                }
            }
            else
            {
                // Model validation failed, redisplay login form with validation errors
                return View(model);
            }
        }
    
        public IActionResult Register()
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
