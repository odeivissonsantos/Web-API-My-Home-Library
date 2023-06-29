using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_My_Home_Library.DTOs.Critica;

namespace WebAPI_My_Home_Library.DTOs.Login
{
    public class CadastrarUsuarioRetornoDTO : CriticaDTO
    {
        public long Ide_Usuario { get; set; }


        public CadastrarUsuarioRetornoDTO()
        {

        }


        public CadastrarUsuarioRetornoDTO(Models.Usuario item)
        {
            Ide_Usuario = item.Ide_Usuario;
            IsOk = true;
            MensagemRetorno = "Usuário cadastrado com sucesso";
        }

    }

}
