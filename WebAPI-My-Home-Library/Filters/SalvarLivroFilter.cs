using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.Filters
{
    public class SalvarLivroFilter
    {
        public string Guuid { get; set; }
        public string Autor { get; set; }
        public int Ano { get; set; }
        public string Editora { get; set; }
        public int? CodigoBarras { get; set; }
        public string UrlCapa { get; set; }
        public string Titulo { get; set; }
        public string Observacao { get; set; }
        public string Guuid_Usuario { get; set; }
    }
}
