using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetoRedehost.ViewModels
{
    public class TldViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencher campo Extension")]
        public string Extension { get; set; }

        public string UsuarioAlteracao { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}