using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.DTOs.Livro
{
    public class ListarLivrosUsuarioDTO
    {
        public long Ide_Livro { get; set; }

        public string Autor { get; set; }

        public int Ano { get; set; }

        public string Editora { get; set; }

        public long? Codigo_Barras { get; set; }

        public string Url_Capa { get; set; }

        public string Titulo { get; set; }

        public string Observacao { get; set; }

        public ListarLivrosUsuarioDTO()
        {

        }

        public ListarLivrosUsuarioDTO(Models.Livro item)
        {
            Ide_Livro = item.Ide_Livro;
            Autor = item.Autor;
            Ano = item.Ano;
            Editora = item.Editora;
            Codigo_Barras = item.Codigo_Barras;
            Url_Capa = item.Url_Capa;
            Titulo = item.Titulo;
            Observacao = item.Observacao;
        }
    }
}
