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
    public class LoginController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly LibreContext db;
        public LoginController(LibreContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.Usuarios.ToList());
        }

        [HttpPost]
        public IActionResult Post([FromBody] Usuario usuario)
        {
            var user = db.Usuarios
                .Where(x => x.Cpf.Equals(usuario.Cpf) && x.Senha.Equals(usuario.Senha))
                .FirstOrDefault();

            if(user == null) {
                return NotFound();
            }

            return Ok(user);
        }
    }
}