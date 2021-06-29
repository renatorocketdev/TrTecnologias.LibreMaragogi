using LibreMaragogi.Data;
using LibreMaragogi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibreMaragogi.Controllers
{
    public class LeitoresController : Controller
    {
        private readonly LibreContext db;

        public LeitoresController(LibreContext db)
        {
            this.db = db;
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Index()
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return View(db.Usuarios.Where(x => x.Role != "Administrador").ToList());
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Index(string campo)
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var list = new List<Usuario>(db.Usuarios.Where(x => x.Role != "Administrador").ToList());
            if (!string.IsNullOrWhiteSpace(campo))
            {
                list = list
                .Where(x => x.Nome.Contains(campo) || x.Cpf.Contains(campo))
                .ToList();
            }

            return View(list);
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Create(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            db.Usuarios.Add(usuario);
            db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            if (id == null)
            {
                return NotFound();
            }

            var usuario = await db.Usuarios.FirstOrDefaultAsync(m => m.UsuariosId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var usuario = await db.Usuarios.FindAsync(id);
            var emprestimos = await db.Emprestimos.Where(x => x.UsuariosId == usuario.UsuariosId).ToListAsync();

            if (emprestimos != null)
            {
                ViewBag.Error = "Impossível deletar leitor, existem livros emprestados!";
                return View(usuario);
            }
            else
            {
                db.Usuarios.Remove(usuario);
                await db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await db.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.UsuariosId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(usuario);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuariotExists(usuario.UsuariosId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        private bool UsuariotExists(int id)
        {
            return db.Usuarios.Any(e => e.UsuariosId == id);
        }
    }
}
