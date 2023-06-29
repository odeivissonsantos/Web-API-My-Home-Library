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

        public LoginRetornoDTO(Models.Usuario item)
        {
            IdeUsuario = item.Ide_Usuario;
            NomeUsuario = item.Nome;
            SobrenomeUsuario = item.Sobrenome;
            Email = item.Email;
            Token = item.Token;
            IsOk = true;
            MensagemRetorno = "Login efetuado com sucesso";
        }
    }
}
