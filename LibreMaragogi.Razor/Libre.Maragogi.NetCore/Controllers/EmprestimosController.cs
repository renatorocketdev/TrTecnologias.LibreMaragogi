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
    public class EmprestimosController : Controller
    {
        private readonly LibreContext db;

        public EmprestimosController(LibreContext db)
        {
            this.db = db;
        }

       

        [Authorize(Policy = "Administrador")]
        public IActionResult Index()
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return View(db.EmprestimosVw.Where(x => string.IsNullOrWhiteSpace(x.Devolvido)).ToList());
        }

        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Renovar(int id)
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var emprestimo = await db.Emprestimos.FirstOrDefaultAsync(x => x.EmprestimosId == id);

            if (id != emprestimo.EmprestimosId)
            {
                return NotFound();
            }

            try
            {
                if (emprestimo.DtPrevistoDevolucao.HasValue)
                {
                    emprestimo.DtPrevistoDevolucao = emprestimo.DtPrevistoDevolucao.Value.AddDays(7);
                    db.Update(emprestimo);
                    await db.SaveChangesAsync();
                }
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

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Index(string campo)
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var list = new List<EmprestimoVw>(db.EmprestimosVw.Where(x => string.IsNullOrWhiteSpace(x.Devolvido)).ToList());
            if (!string.IsNullOrWhiteSpace(campo))
            {
                if (!campo.Contains("/"))
                {
                    list = db.EmprestimosVw.Where(
                        x => x.Devolvido == null && (x.Usuario.Contains(campo) ||
                        x.Cpf.Contains(campo) || x.Livro.Contains(campo))).ToList();
                }
                else
                {
                    list = db.EmprestimosVw.Where(
                        x => x.Devolvido == null && (x.DtEmprestimo.Date == DateTime.Parse(campo) ||
                        x.DtDevolucao.Value == DateTime.Parse(campo))).ToList();
                }
            }

            return View();
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

            ViewBag.Livros = db.Livros
                .Select(c => new SelectListItem()
                    {
                        Text = c.Titulo,
                        Value = c.LivrosId.ToString(),
                    }).ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Create(Emprestimo emprestimo)
        {
            foreach (var item in emprestimo.Livros)
            {
                emprestimo.EmprestimosId = 0;
                emprestimo.LivrosId = item;

                var livro = await db.Livros.FindAsync(item);
                livro.Exemplares--;

                db.Emprestimos.Add(emprestimo);
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

            var emprestimo = await db.EmprestimosVw.FindAsync(id);
            
            if (emprestimo.DtDevolucao != null) {
                ViewBag.Error = "Empréstimo com Data de Devolução, por favor remover Data de Devolução antes de excluir Empréstimo";
                return View(emprestimo);
            }
            else
            {
                db.EmprestimosVw.Remove(emprestimo);
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
            return db.Livros.Any(e => e.LivrosId == id);
        }
    }
}