using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.Models
{
    [Table("usuario_livro")]
    public class UsuarioLivro
    {
        [Key]
        [Column("ide_usuario_livro")]
        public int Ide_Usuario_Livro { get; set; }

        [Column("guid_usuario")]
        public string Guuid_Usuario { get; set; }

        [Column("guid_livro")]
        public string Guuid_Livro { get; set; }
    }
}
