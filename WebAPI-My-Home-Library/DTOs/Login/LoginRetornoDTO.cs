using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI_My_Home_Library.DTOs.Critica;
using WebAPI_My_Home_Library.Models;

namespace WebAPI_My_Home_Library.DTOs.Login
{
    public class LoginRetornoDTO : CriticaDTO
    {
        public long IdeUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string SobrenomeUsuario { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public LoginRetornoDTO()
        { 
        
        }

        public LoginRetornoDTO(Models.Usuario item, Models.UsuarioToken token)
        {
            IdeUsuario = item.Ide_Usuario;
            NomeUsuario = item.Nome;
            SobrenomeUsuario = item.Sobrenome;
            Email = item.Email;
            Token = token.Token;
            MensagemRetorno = "Login efetuado com sucesso";
        }
    }
}
