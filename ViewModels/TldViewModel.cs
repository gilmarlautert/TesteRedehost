using System.ComponentModel.DataAnnotations;

namespace ProjetoRedehost.ViewModels
{
    public class TldViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Preencher campo Extension")]
        public string Extension { get; set; }
    }
}