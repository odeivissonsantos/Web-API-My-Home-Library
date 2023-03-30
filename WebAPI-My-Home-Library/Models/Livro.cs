using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.Models
{
    [Table("livro")]
    public class Livro
    {
        [Key]
        [Column("ide_livro")]
        public int Ide_Livro { get; set; }

        [Column("guid")]
        public string Guuid { get; set; }

        [Column("autor")]
        public string Autor { get; set; }

        [Column("ano")]
        public int Ano { get; set; }

        [Column("editora")]
        public string Editora { get; set; }

        [Column("codigo_barras")]
        public int? Codigo_Barras { get; set; }

        [Column("url_capa")]
        public string Url_Capa { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; }

        [Column("observacao")]
        public string Observacao { get; set; }
    }
}
