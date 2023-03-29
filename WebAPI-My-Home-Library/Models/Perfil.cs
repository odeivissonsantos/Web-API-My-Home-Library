using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.Models
{
    [Table("perfil")]
    public class Perfil
    {
        [Key]
        [Column("ide_perfil")]
        public int Ide_perfil { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }

        [Column("sts_exclusao")]
        public bool Status_Exclusao { get; set; } = false;

    }
}
