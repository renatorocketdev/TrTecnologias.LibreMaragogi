using libre_api.Data;
using libre_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace libre_api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly LibreContext db;
        public LivrosController(LibreContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.Livros.ToList());
        }

        [HttpGet("campo")]
        public async Task<IActionResult> Index(string campo)
        {
            var list = await db.Livros.ToListAsync();
            if (!string.IsNullOrWhiteSpace(campo))
            {
                list = await db.Livros
                    .Where(x => x.Categoria.Contains(campo) || x.Autor.Contains(campo) || x.Ano.Contains(campo) || x.Titulo.Contains(campo))
                    .ToListAsync();

                return Ok(list);
            }

            return Ok(list);
        }

        [HttpPost]
        public IActionResult Post(Livro livro){
            db.Livros.Add(livro);
            db.SaveChanges();
            return Ok(livro);
        }
    }
}