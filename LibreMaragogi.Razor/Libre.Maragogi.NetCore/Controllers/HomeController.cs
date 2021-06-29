using LibreMaragogi.Data;
using LibreMaragogi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibreMaragogi.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibreContext db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, LibreContext db)
        {
            this.db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return View();
        }

        public IActionResult Acervo()
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return View();
        }

        public IActionResult Reserva()
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registrar()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return View(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Usuario usuario)
        {
            if (!string.IsNullOrWhiteSpace(usuario.Nome))
            {
                db.Usuarios.Add(usuario);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Role, usuario.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                    });

                db.SaveChanges();
            }

            return RedirectToAction(nameof(Acervo));
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            var user = db.Usuarios
                    .FirstOrDefault(
                    x => x.Cpf == usuario.Cpf &&
                    x.Senha == usuario.Senha);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                    });

                if (user.Role.Equals("Administrador"))
                {
                    return RedirectToAction("Panel", "Panel");
                }
                else
                {
                    return View(nameof(Login));
                }
            }
            else
            {
                return View(nameof(Login));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}