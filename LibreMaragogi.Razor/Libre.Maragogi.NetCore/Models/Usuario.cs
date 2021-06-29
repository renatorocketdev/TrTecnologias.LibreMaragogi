using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibreMaragogi.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        private string cpf;

        [Key]
        public int UsuariosId { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Cpf 
        { 
            get => cpf.Replace(".", "").Replace("-", ""); 
            set => cpf = value.Replace(".", "").Replace("-", ""); 
        }
        public string Telefone { get; set; }
        public DateTime? Nascimento { get; set; }
        public string Logradouro { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Email { get; set; }

        public string Sexo { get; set; }
        public string AreaInteresse { get; set; }
        public string Profissao { get; set; }
        public string Role { get; set; }
    }
}