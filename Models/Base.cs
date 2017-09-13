using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetoRedehost.Models
{
    public abstract class Base
    { 
        public int Id { get; set; }
         
        [MaxLength(100)]
        public string UsuarioCriacao { get; set; }

        [MaxLength(100)]
        public string UsuarioAlteracao { get; set; }

        public DateTime? DataCriacao { get; set; }
        
        public DateTime? DataAlteracao { get; set; }
    }
}