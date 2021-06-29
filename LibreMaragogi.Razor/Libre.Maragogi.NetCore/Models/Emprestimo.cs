using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibreMaragogi.Models
{
    [Table("Emprestimos")]
    public class Emprestimo
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

        [NotMapped]
        public List<int> Livros { get; set; }
    }
}