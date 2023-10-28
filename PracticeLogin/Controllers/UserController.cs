using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeLogin.Models.DB;
using System.Security.Claims;

namespace PracticeLogin.Controllers
{
    public class UserController : Controller
    {       
        public IActionResult Index()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if(claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult VistaLogueado()
        {

            return View();
        }

        [HttpPost]  
        public async Task<IActionResult> Index(Usuario model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new Models.DB.LoginTestContext())
                {
                    var user = db.Usuarios.SingleOrDefault(u => u.Email == model.Email);

                    if (user != null && user.Password == model.Password)
                    {
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, model.Email)                          
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties properties = new AuthenticationProperties() 
                        {
                            AllowRefresh = true,
                            IsPersistent = false
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity), properties);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Credenciales inválidas. Por favor, inténtelo de nuevo.");
                    }
                }
            }
            ViewData["ValidateMEssage"] = "user not found";
            return View();
        }


    }
}
