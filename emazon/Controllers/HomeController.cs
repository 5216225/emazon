using System.Data.SqlClient;
using System.Diagnostics;
using emazon.Models;
using Microsoft.AspNetCore.Mvc;

namespace emazon.Controllers
{
    public class HomeController : Controller
    {
        private const string connectionString = @"Data Source=(local);Initial Catalog=Emazon;Integrated Security=True";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Check if user is logged in
            bool isLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";

            // Pass isLoggedIn to the view
            ViewBag.IsLoggedIn = isLoggedIn;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.email) || string.IsNullOrWhiteSpace(model.password))
            {
                ViewBag.ErrorMessage = "Login did not work.";
                return View("Login", model);
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Id FROM Users WHERE email = @Email AND passwrod = @Password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", model.email);
                    command.Parameters.AddWithValue("@Password", model.password);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        int id = reader.GetInt32(0); // Retrieve the id from the result set
                        reader.Close();

                        // Login successful
                        ViewBag.ErrorMessage = "Login is Successful.";

                        // Set session value
                        HttpContext.Session.SetString("IsLoggedIn", "true");
                        return View("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid email or password.";
                        return View("Login", model);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error: " + ex.Message;
                    return View("Index", model);
                }
            }
        }

        public IActionResult Logout()
        {
            // Remove session value
            HttpContext.Session.Remove("IsLoggedIn");

            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            bool isLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";
            if (!isLoggedIn)
            {
                return RedirectToAction("Login");
            }

            var model = new LoginViewModel
            {
                email = HttpContext.Session.GetString("Email"),
                // Add more user details as needed
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
