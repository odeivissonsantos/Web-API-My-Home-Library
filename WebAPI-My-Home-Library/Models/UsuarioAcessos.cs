using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.Models
{
    [Table("usuario_acessos")]
    public class UsuarioAcessos
    {
        [Key]
        [Column("ide_usuario_acessos")]
        public long Ide_Usuario_Acessos { get; set; }

        [Column("ide_usuario")]
        public long Ide_Usuario { get; set; }

        [Column("qtd_acessos")]
        public long? Quanidade_Acessos { get; set; } = 0;

        [Column("dtc_ultimo_acesso")]
        public DateTime? Data_Ultimo_Acesso { get; set; }
    }
}
