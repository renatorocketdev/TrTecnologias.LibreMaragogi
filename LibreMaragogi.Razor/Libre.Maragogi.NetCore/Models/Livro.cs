using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibreMaragogi.Models
{
    [Table("Livros")]
    public class Livro
    {
        [Key]
        public int LivrosId { get; set; }

        [Required(ErrorMessage = "Campo Título é obrigatório.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Campo Autor é obrigatório.")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "Campo Assunto é obrigatório.")]
        public string Categoria { get; set; }
        public string Categoria2 { get; set; }
        public string Ano { get; set; }


        [Required(ErrorMessage = "Campo Quantidade de Exemplares é obrigatório.")]
        public int? Exemplares { get; set; }
        public string Volume { get; set; }
        public string Editora { get; set; }
        public string Serie { get; set; }


        //[Required(ErrorMessage = "Campo Quantidade de Exemplares é obrigatório.")]
        public string Tombo { get; set; }
        public string Classificacao { get; set; }

        [NotMapped]
        public string Logo { get; set; }
    }
}