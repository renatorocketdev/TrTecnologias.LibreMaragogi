using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace libre_api.Models
{
    [Table("EmprestimosVw")]
    public class EmprestimoVw
    {
        [Key]
        public int EmprestimosId { get; set; }
        public int LivrosId { get; set; }
        public int UsuariosId { get; set; }
        public DateTime DtEmprestimo { get; set; }
        public DateTime? DtDevolucao { get; set; }
        public DateTime? DtPrevistoDevolucao { get; set; }
        public string Observacao { get; set; }
        public string Devolvido { get; set; }
        public string Livro { get; set; }
        public string Usuario { get; set; }
        public string Cpf { get; set; }

        [NotMapped]
        public List<int> Livros { get; set; }
    }
}
