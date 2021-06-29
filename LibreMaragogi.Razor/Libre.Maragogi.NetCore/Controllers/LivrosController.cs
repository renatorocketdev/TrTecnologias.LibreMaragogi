using LibreMaragogi.Data;
using LibreMaragogi.Models;
using LibreMaragogi.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibreMaragogi.Controllers
{
    public class LivrosController : Controller
    {
        private readonly IWebHostEnvironment env;
        private readonly LibreContext db;
        public LivrosController(LibreContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Index()
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return View(db.Livros.ToList());
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Index(string campo, string filtro)
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var list = new List<Livro>(db.Livros.ToList());

            if (!string.IsNullOrWhiteSpace(campo) && filtro.Equals("Assunto1"))
            {
                list = db.Livros.Where(x => x.Categoria == campo).ToList();
            }

            if (!string.IsNullOrWhiteSpace(campo) && filtro.Equals("Assunto2"))
            {
                list = db.Livros.Where(x => x.Categoria2 == campo).ToList();
            }

            if (!string.IsNullOrWhiteSpace(campo) && filtro.Equals("Titulo"))
            {
                list = db.Livros.Where(x => x.Titulo == campo).ToList();
            }

            if (!string.IsNullOrWhiteSpace(campo) && filtro.Equals("Autor"))
            {
                list = db.Livros.Where(x => x.Autor == campo).ToList();
            }

            if (!string.IsNullOrWhiteSpace(campo) && filtro.Equals("Serie"))
            {
                list = db.Livros.Where(x => x.Serie == campo).ToList();
            }

            if (!string.IsNullOrWhiteSpace(campo) && filtro.Equals("Tombo"))
            {
                list = db.Livros.Where(x => x.Tombo == campo).ToList();
            }

            if (!string.IsNullOrWhiteSpace(campo) && filtro.Equals(""))
            {
                list = db.Livros
                .Where(
                    x => x.Categoria.Contains(campo) || x.Autor.Contains(campo) || 
                    x.Ano.Contains(campo) || x.Titulo.Contains(campo) || 
                    x.Categoria2.Contains(campo) || x.Tombo.Contains(campo) || x.Serie.Contains(campo))
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
        public IActionResult Create(Livro livro)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            db.Livros.Add(livro);
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

            var livro = await db.Livros.FirstOrDefaultAsync(m => m.LivrosId == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Nome = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var livro = await db.Livros.FindAsync(id);
            db.Livros.Remove(livro);
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

            var livro = await db.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            return View(livro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int id, Livro livro)
        {
            if (id != livro.LivrosId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(livro);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivrosExists(livro.LivrosId))
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
            return View(livro);
        }

        private bool LivrosExists(int id)
        {
            return db.Livros.Any(e => e.LivrosId == id);
        }

        [Authorize(Policy = "Administrador")]
        public void Tag()
        {
            //Creating new PDF document instance
            PdfDocument document = new PdfDocument();
            //Setting margin
            document.PageSettings.Margins.All = 0;
            //Adding a new page
            PdfPage page = document.Pages.Add();
            PdfGraphics g = page.Graphics;

            float headerBulletsXposition = 40;
            var txtElement = new PdfTextElement("About Syncfusion");
            txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
            var result = txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition + 290, 30, 450, 200));

            //Saving the PDF to the MemoryStream
            MemoryStream ms = new MemoryStream();
            document.Save(ms);
            //If the position is not set to '0' then the PDF will be empty.
            ms.Position = 0;

            //Download the PDF document in the browser.
            FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");
            fileStreamResult.FileDownloadName = "Sample.pdf";
        }

        public async Task<IActionResult> Logo(int? id, List<IFormFile> files)
        {
            var categoria = await db.Livros.FirstOrDefaultAsync(m => m.LivrosId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            //Images Handler
            var handler = new ImagesHandler($"{env.WebRootPath}/livros/capas/{id}/");

            //getting files
            ViewBag.Images = handler.GettingFiles();

            if (files.Count != 0)
            {
                var result = await handler.SaveImage(files);
                if (result)
                {
                    categoria.Logo = handler.path;
                    db.Update(categoria);
                    await db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return View(categoria);
            }
        }
    }
}
