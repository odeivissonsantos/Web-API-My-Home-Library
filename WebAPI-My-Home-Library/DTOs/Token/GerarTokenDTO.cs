using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.DTOs.Login
{
    public class GerarTokenDTO
    {
        public string Token { get; set; }
        public DateTime DataExpiracaoToken { get; set; }
    }
}
