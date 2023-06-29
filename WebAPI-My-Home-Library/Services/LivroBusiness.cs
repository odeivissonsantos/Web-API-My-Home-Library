using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_My_Home_Library.Context;
using WebAPI_My_Home_Library.DTOs.Critica;
using WebAPI_My_Home_Library.DTOs.Livro;
using WebAPI_My_Home_Library.Filters;
using WebAPI_My_Home_Library.Models;
using WebAPI_My_Home_Library.Utils;
using static WebAPI_My_Home_Library.Enums.EnumCommon;

namespace WebAPI_My_Home_Library.Services
{
    public class LivroBusiness
    {
        public static bool isDesenv = Settings.IsDesenv;

        private readonly MyHomeLibraryContext _myHomeLibraryContext;
        private readonly Utilies _utilies;

        public LivroBusiness(MyHomeLibraryContext myHomeLibraryContext, Utilies utilies)
        {
            _myHomeLibraryContext = myHomeLibraryContext;
            _utilies = utilies;
        }

        #region SALVAR LIVRO
        public void Novo(NovoLivroFilter filter)
        {
            Livro newLivro = new()
            {
                Ano = filter.Ano,
                Autor = filter.Autor,
                Codigo_Barras = filter.CodigoBarras,
                Editora = filter.Editora,
                Titulo = filter.Titulo,
                Observacao = filter.Observacao,
                Url_Capa = filter.UrlCapa,
            };

            _myHomeLibraryContext.Livro.Add(newLivro);
            _myHomeLibraryContext.SaveChanges();

            UsuarioLivro newUsuarioLivro = new()
            {
                Ide_Livro = newLivro.Ide_Livro,
                Ide_Usuario = filter.Ide_Usuario,
            };

            _myHomeLibraryContext.Usuario_Livro.Add(newUsuarioLivro);
            _myHomeLibraryContext.SaveChanges();
        }
        #endregion

        #region EDITAR LIVRO
        public void Editar(EditarLivroFilter filter)
        {
            var query = BuscarDadosLivro(filter.Ide_Livro);

            if (query == null) throw new Exception($"Não foi possível encontrar um livro com o ID [{filter.Ide_Livro}].");

            query.Ano = filter.Ano;
            query.Autor = filter.Autor;
            query.Codigo_Barras = filter.CodigoBarras;
            query.Editora = filter.Editora;
            query.Titulo = filter.Titulo;
            query.Observacao = filter.Observacao;
            query.Url_Capa = filter.UrlCapa;

            _myHomeLibraryContext.Livro.Update(query);
            _myHomeLibraryContext.SaveChanges();
        }
        #endregion

        #region LISTAR LIVROS POR USUÁRIO
        public List<ListarLivrosUsuarioDTO> ListarPorUsuario(long ide_usuario)
        {
            List<ListarLivrosUsuarioDTO> listLivrosUsuario = new List<ListarLivrosUsuarioDTO>();
            ListarLivrosUsuarioDTO livroUsuario = new ListarLivrosUsuarioDTO();

            var query = _myHomeLibraryContext.Usuario_Livro.Where(x => x.Ide_Usuario == ide_usuario).ToList();

            if (query.Any())
            {
                foreach (var item in query)
                {
                    Livro livro = BuscarDadosLivro(item.Ide_Livro);

                    if (livro != null)
                    {
                        livroUsuario = new(livro);
                        listLivrosUsuario.Add(livroUsuario);
                    }
                }
            }

            return listLivrosUsuario;
        }
        #endregion

        #region BUSCAR LIVRO POR ID
        public LivroDTO BuscarPorID(long ide_livro)
        {
            LivroDTO livro = new LivroDTO();

            var query = BuscarDadosLivro(ide_livro);
            if (query == null) throw new Exception($"Não foi possível encontrar um livro com o ID [{ide_livro}].");

            var usuarioLivro = _myHomeLibraryContext.Usuario_Livro.Where(x => x.Ide_Livro == query.Ide_Livro).FirstOrDefault();
            livro = new(query, usuarioLivro);
            livro.IsOk = true;

            return livro;            
        }
        #endregion

        #region EXCLUIR LIVRO
        public void Excluir(long ide_livro)
        {
            Livro livroUsuario = BuscarDadosLivro(ide_livro);

            if (livroUsuario == null) throw new Exception($"Não foi possível encontrar um livro com o ID [{ide_livro}].");

            var query = _myHomeLibraryContext.Usuario_Livro.Where(x => x.Ide_Livro == livroUsuario.Ide_Livro).FirstOrDefault();

            _myHomeLibraryContext.Livro.Remove(livroUsuario);
            _myHomeLibraryContext.Usuario_Livro.Remove(query);
            _myHomeLibraryContext.SaveChanges();
        }
        #endregion

        #region UTILS
        private Livro BuscarDadosLivro(long ide_livro)
        {
            return _myHomeLibraryContext.Livro.Where(x => x.Ide_Livro == ide_livro).FirstOrDefault();
        }
        #endregion

    }
}
