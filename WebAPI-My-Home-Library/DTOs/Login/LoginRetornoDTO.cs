using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.DTOs.Login
{
    public class LoginRetornoDTO
    {
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public long IdeUsuario { get; set; }
        public string Token { get; set; }
    }
}
