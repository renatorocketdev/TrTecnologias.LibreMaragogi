using LibreMaragogi.Data;
using LibreMaragogi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;

namespace LibreMaragogi.Controllers
{
    public class PanelController : Controller
    {
        private readonly LibreContext db;

        public PanelController(LibreContext db)
        {
            this.db = db;
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Panel()
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return View();
        }
    }
}