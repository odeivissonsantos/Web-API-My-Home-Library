using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.Filters
{
    public class SalvarUsuarioFilter
    {
        public string Guuid { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
    }
}
