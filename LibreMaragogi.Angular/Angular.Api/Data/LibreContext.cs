using libre_api.Models;
using Microsoft.EntityFrameworkCore;

namespace libre_api.Data
{
    public class LibreContext : DbContext
    {
        public LibreContext(DbContextOptions<LibreContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<EmprestimoVw> EmprestimosVw { get; set; }
    }
}