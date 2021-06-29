using LibreMaragogi.Data;
using LibreMaragogi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreMaragogi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly LibreContext db;
        public LivrosController(LibreContext db)
        {
            this.db = db;
        }

        [HttpGet("{id}")]
        public List<Select2> Get(int id)
        {
            var livros = db.EmprestimosVw.Where(x => x.UsuariosId == id && x.Devolvido != "S").Select(x => new Select2()
            {
                Id = x.LivrosId,
                Text = x.Livro
            }).ToList();

            return livros;
        }
    }
}
