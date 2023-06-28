﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI_My_Home_Library.Enums;

namespace WebAPI_My_Home_Library.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("ide_usuario")]
        public long Ide_Usuario { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("sobrenome")]
        public string Sobrenome { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("senha")]
        public string Senha { get; set; }

        [Column("dtc_inclusao")]
        public DateTime Data_Inclusao { get; set; } = DateTime.Now;

        [Column("sts_exclusao")]
        public bool Status_Exclusao { get; set; } = false;

        [Column("token")]
        public string Token { get; set; }

        [Column("qtd_acessos")]
        public int? Quanidade_Acessos { get; set; } = 0;

        [Column("dtc_ultimo_acesso")]
        public DateTime? Data_Ultimo_Acesso { get; set; }

        [Column("ide_perfil")]
        public int Ide_Perfil { get; set; } = EnumCommon.PerfilEnum.Padrao.GetHashCode();
    }
}
