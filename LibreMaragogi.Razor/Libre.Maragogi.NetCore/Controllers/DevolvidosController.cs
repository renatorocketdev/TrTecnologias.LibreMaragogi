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
    public class DevolvidosController : Controller
    {
        private readonly LibreContext db;

        public DevolvidosController(LibreContext db)
        {
            this.db = db;
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Index()
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return View(db.EmprestimosVw.Where(x => x.Devolvido == "S").ToList());
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Index(string campo)
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var list = new List<EmprestimoVw>(db.EmprestimosVw.Where(x => x.Devolvido == "S").ToList());
            if (!string.IsNullOrWhiteSpace(campo))
            {
                if (!campo.Contains("/"))
                {
                    list = list.Where(
                        x => x.Usuario.Contains(campo) ||
                        x.Cpf.Contains(campo) || x.Livro.Contains(campo)).ToList();
                }
                else
                {
                    list = list.Where(
                        x => x.DtEmprestimo.Date == DateTime.Parse(campo) ||
                        x.DtDevolucao.Value == DateTime.Parse(campo)).ToList();
                }
            }

            return View(list);
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            ViewBag.Usuarios = db.Usuarios.Where(x => x.Role != "Administrador")
                .Select(c => new SelectListItem()
                {
                    Text = c.Nome + " - " + c.Cpf,
                    Value = c.UsuariosId.ToString(),
                }).ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Create(Emprestimo emprestimo)
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            foreach (var livroId in emprestimo.Livros)
            {
                var newEmprestimo = db.Emprestimos.Where(x => x.UsuariosId == emprestimo.UsuariosId && x.LivrosId == livroId).FirstOrDefault();
                newEmprestimo.DtDevolucao = emprestimo.DtDevolucao;
                newEmprestimo.Devolvido = "S";
                db.Update(newEmprestimo);
                db.SaveChanges();

                var livro = db.Livros.Find(livroId);
                livro.Exemplares++;
                db.Update(livro);
                db.SaveChanges();
            }

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

            var emprestimo = await db.EmprestimosVw.FirstOrDefaultAsync(m => m.EmprestimosId == id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var emprestimo = await db.Emprestimos.FindAsync(id);
            db.Emprestimos.Remove(emprestimo);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await db.Emprestimos.FindAsync(id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            ViewBag.Usuarios = db.Usuarios.Where(x => x.Role != "Administrador")
                .Select(c => new SelectListItem()
                {
                    Text = c.Nome + " - " + c.Cpf,
                    Value = c.UsuariosId.ToString(),
                }).ToList();

            ViewBag.Livros = db.Livros
                .Select(c => new SelectListItem()
                {
                    Text = c.Titulo,
                    Value = c.LivrosId.ToString(),
                }).ToList();

            return View(emprestimo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int id, Emprestimo emprestimo)
        {
            if (id != emprestimo.EmprestimosId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (emprestimo.DtDevolucao < DateTime.Now || emprestimo.DtDevolucao == null)
                    {
                        emprestimo.Devolvido = "";
                    }
                    else
                    {
                        emprestimo.Devolvido = "S";
                    }

                    db.Update(emprestimo);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmprestimoExists(emprestimo.EmprestimosId))
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
            return View(emprestimo);
        }

        private bool EmprestimoExists(int id)
        {
            return db.Emprestimos.Any(e => e.EmprestimosId == id);
        }
    }
}