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
        public NovoLivroRetornoDTO Novo(NovoLivroFilter filter)
        {
            Livro newLivro = new Livro();
            UsuarioLivro newUsuarioLivro = new UsuarioLivro();
            NovoLivroRetornoDTO retorno = new NovoLivroRetornoDTO();

            try
            {
                bool isExistsUsuario = _utilies.VerificaSeExiste(2, filter.Ide_Usuario.ToString());
                if (!isExistsUsuario) throw new Exception("Usuário não encontrado, digite um usuário válido");

                newLivro.Ano = filter.Ano;
                newLivro.Autor = filter.Autor;
                newLivro.Codigo_Barras = filter.CodigoBarras;
                newLivro.Editora = filter.Editora;
                newLivro.Titulo = filter.Titulo;
                newLivro.Observacao = filter.Observacao;
                newLivro.Url_Capa = filter.UrlCapa;

                _myHomeLibraryContext.Livro.Add(newLivro);
                _myHomeLibraryContext.SaveChanges();

                newUsuarioLivro.Ide_Livro = newLivro.Ide_Livro;
                newUsuarioLivro.Ide_Usuario = filter.Ide_Usuario;
                
                _myHomeLibraryContext.Usuario_Livro.Add(newUsuarioLivro);
                _myHomeLibraryContext.SaveChanges();

                retorno = new(newLivro);

            }
            catch (Exception ex)
            {
                retorno.IsOk = false;
                retorno.MensagemRetorno = ex.Message;
            }

            return retorno;
        }
        #endregion

        #region EDITAR LIVRO
        public CriticaDTO Editar(EditarLivroFilter filter)
        {
            CriticaDTO retorno = new CriticaDTO();

            try
            {
                var query = _myHomeLibraryContext.Livro.Where(x => x.Ide_Livro == filter.Ide_Livro).FirstOrDefault();

                if (query == null) throw new Exception("Livro não encontrado!");

                query.Ano = filter.Ano;
                query.Autor = filter.Autor;
                query.Codigo_Barras = filter.CodigoBarras;
                query.Editora = filter.Editora;
                query.Titulo = filter.Titulo;
                query.Observacao = filter.Observacao;
                query.Url_Capa = filter.UrlCapa;

                _myHomeLibraryContext.Livro.Update(query);
                _myHomeLibraryContext.SaveChanges();

                retorno.IsOk = false;
                retorno.MensagemRetorno = "Livro atualizado com sucesso";         
            }
            catch (Exception ex)
            {
                retorno.IsOk = false;
                retorno.MensagemRetorno = ex.Message;
            }

            return retorno;
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
                    Livro livro = _myHomeLibraryContext.Livro.Where(x => x.Ide_Livro == item.Ide_Livro).FirstOrDefault();

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
            LivroDTO retorno = new LivroDTO();

            try
            {
                Livro livro = _myHomeLibraryContext.Livro.Where(x => x.Ide_Livro == ide_livro).FirstOrDefault();

                if (livro != null)
                {
                    var usuarioLivro = _myHomeLibraryContext.Usuario_Livro.Where(x => x.Ide_Livro == livro.Ide_Livro).FirstOrDefault();

                    retorno = new(livro, usuarioLivro);
                }
                else
                {
                    retorno.IsOk = false;
                    retorno.MensagemRetorno = $"O livro com o ID [{ide_livro}] não foi encontrado.";
                }
            }
            catch (Exception ex)
            {
                retorno.IsOk = false;
                retorno.MensagemRetorno = ex.Message;
            }

            return retorno;
        }
        #endregion

        #region EXCLUIR LIVRO
        public CriticaDTO Excluir(long ide_livro)
        {
            CriticaDTO retorno = new CriticaDTO();

            try
            {
                Livro livroUsuario = _myHomeLibraryContext.Livro.Where(x => x.Ide_Livro == ide_livro).FirstOrDefault();

                if (livroUsuario == null) throw new Exception("Livro não encontrado!");

                var query = _myHomeLibraryContext.Usuario_Livro.Where(x => x.Ide_Livro == livroUsuario.Ide_Livro).FirstOrDefault();

                _myHomeLibraryContext.Livro.Remove(livroUsuario);
                _myHomeLibraryContext.Usuario_Livro.Remove(query);
                _myHomeLibraryContext.SaveChanges();

                retorno.IsOk = false;
                retorno.MensagemRetorno = "Livro excluído com sucesso!";
            }
            catch (Exception ex)
            {
                retorno.IsOk = false;
                retorno.MensagemRetorno = ex.Message;
            }

            return retorno;
        }
        #endregion

    }
}
