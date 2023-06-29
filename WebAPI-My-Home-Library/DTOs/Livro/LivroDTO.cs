using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_My_Home_Library.DTOs.Critica;

namespace WebAPI_My_Home_Library.DTOs.Livro
{
    public class LivroDTO : CriticaDTO
    {
        public long Ide_Livro { get; set; }

        public string Autor { get; set; }

        public int Ano { get; set; }

        public string Editora { get; set; }

        public long? Codigo_Barras { get; set; }

        public string Url_Capa { get; set; }

        public string Titulo { get; set; }

        public string Observacao { get; set; }

        public long Ide_Usuario { get; set; }

        public LivroDTO()
        {

        }

        public LivroDTO(Models.Livro item, Models.UsuarioLivro itemUsuario)
        {
            Ide_Livro = item.Ide_Livro;
            Autor = item.Autor;
            Ano = item.Ano;
            Editora = item.Editora;
            Codigo_Barras = item.Codigo_Barras;
            Url_Capa = item.Url_Capa;
            Titulo = item.Titulo;
            Observacao = item.Observacao;
            Ide_Usuario = itemUsuario.Ide_Usuario;
            IsOk = true;
            MensagemRetorno = "Livro encontrado com sucesso.";

        }
    }
}
