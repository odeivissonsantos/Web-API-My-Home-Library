using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.Models
{
    [Table("usuario_token")]
    public class UsuarioToken
    {
        [Key]
        [Column("ide_usuario_token")]
        public long Ide_Usuario_Token { get; set; }

        [Column("ide_usuario")]
        public long Ide_Usuario { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("dtc_exp_token")]
        public DateTime? Data_Expiracao_Token { get; set; }
    }
}
