using System.ComponentModel.DataAnnotations;

namespace ProjetoRedehost.Models
{
    public class Tld : Base
    {
               
        [MaxLength(100)]
        public string Extension { get; set; }
    }
}