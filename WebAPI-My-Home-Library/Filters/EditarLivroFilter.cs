using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.Filters
{
    public class EditarLivroFilter
    {
        public long Ide_Livro { get; set; }
        public string Autor { get; set; }
        public int Ano { get; set; }
        public string Editora { get; set; }
        public long? CodigoBarras { get; set; }
        public string UrlCapa { get; set; }
        public string Titulo { get; set; }
        public string Observacao { get; set; }
    }
}
