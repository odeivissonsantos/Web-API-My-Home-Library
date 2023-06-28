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
        public long Ide_Usuario_Livro { get; set; }

        [Column("ide_usuario")]
        public long Ide_Usuario { get; set; }

        [Column("ide_livro")]
        public long Ide_Livro { get; set; }
    }
}
