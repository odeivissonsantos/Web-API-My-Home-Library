using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_My_Home_Library.Context;
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
        public ResultModel<SalvarLivroRetornoDTO> Salvar(SalvarLivroFilter filter)
        {
            bool novo = false;
            Livro newLivro = new Livro();
            UsuarioLivro newUsuarioLivro = new UsuarioLivro();
            ResultModel<SalvarLivroRetornoDTO> data;

            if (filter.Ide_Livro <= 0) novo = true;

            try
            {
                if (filter.Ide_Usuario <= 0) throw new Exception("Campo id do usuário é obrigatório!");
                if (string.IsNullOrEmpty(filter.Autor)) throw new Exception("Campo autor é obrigatório!");
                if (filter.Ano <= 0) throw new Exception("Campo ano é obrigatório!");
                if (string.IsNullOrEmpty(filter.Editora)) throw new Exception("Campo editora é obrigatório!");
                if (string.IsNullOrEmpty(filter.Titulo)) throw new Exception("Campo título é obrigatório!");

                if (!novo)
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

                    SalvarLivroRetornoDTO retorno = new SalvarLivroRetornoDTO()
                    {
                        Mensagem = "Livro atualizado com sucesso",
                    };

                    _myHomeLibraryContext.Livro.Update(query);

                    data = new ResultModel<SalvarLivroRetornoDTO>(true);
                    data.Items.Add(retorno);

                }
                else
                {
                    bool isExistsUsuario = _utilies.VerificaSeExiste(3, filter.Ide_Usuario.ToString());
                    if (!isExistsUsuario) throw new Exception("Usuário não encontrado, digite um usuário válido");

                    newLivro.Ano = filter.Ano;
                    newLivro.Autor = filter.Autor;
                    newLivro.Codigo_Barras = filter.CodigoBarras;
                    newLivro.Editora = filter.Editora;
                    newLivro.Titulo = filter.Titulo;
                    newLivro.Observacao = filter.Observacao;
                    newLivro.Url_Capa = filter.UrlCapa;

                    _myHomeLibraryContext.Livro.Add(newLivro);

                    SalvarLivroRetornoDTO retorno = new SalvarLivroRetornoDTO()
                    {
                        Mensagem = "Livro cadastrado com sucesso!",
                    };

                    data = new ResultModel<SalvarLivroRetornoDTO>(true);
                    data.Items.Add(retorno);

                }

                _myHomeLibraryContext.SaveChanges();

                if (novo)
                {
                    newUsuarioLivro.Ide_Livro = newLivro.Ide_Livro;
                    newUsuarioLivro.Ide_Usuario = filter.Ide_Usuario;

                    _myHomeLibraryContext.Usuario_Livro.Add(newUsuarioLivro);
                    _myHomeLibraryContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                data = new ResultModel<SalvarLivroRetornoDTO>(false);
                data.Messages.Add(new SystemMessageModel { Message = ex.Message, Type = SystemMessageTypeEnum.Error });

            }

            return data;
        }
        #endregion

        #region LISTAR LIVROS POR USUÁRIO
        public ResultModel<Livro> BuscarLivrosPorUsuario(long ide_usuario)
        {
            ResultModel<Livro> data = new ResultModel<Livro>(true);

            try
            {
                if (ide_usuario <= 0) throw new Exception("Campo id do usuário é obrigatório!");
                var query = _myHomeLibraryContext.Usuario_Livro.Where(x => x.Ide_Usuario == ide_usuario).ToList();

                if (query.Any())
                {
                    foreach (var item in query)
                    {
                        Livro livroUsuario = _myHomeLibraryContext.Livro.Where(x => x.Ide_Livro == item.Ide_Livro).FirstOrDefault();

                        if(livroUsuario != null)
                        {
                            data.Items.Add(livroUsuario);
                        }
                    }
                    data.Pages.TotalItems = data.Items.Count();
                    
                }
                else
                {
                    data = new ResultModel<Livro>(true);
                    data.Messages.Add(new SystemMessageModel { Message = "Este usuário ainda não possui livros cadastrados.", Type = SystemMessageTypeEnum.Info });
                }
            }
            catch (Exception ex)
            {
                data = new ResultModel<Livro>(false);
                data.Messages.Add(new SystemMessageModel { Message = ex.Message, Type = SystemMessageTypeEnum.Error });

            }

            return data;
        }
        #endregion

        #region BUSCAR LIVRO POR ID
        public ResultModel<Livro> BuscarPorID(long ide_livro)
        {
            ResultModel<Livro> data = new ResultModel<Livro>(true);

            try
            {
                if (ide_livro <= 0) throw new Exception("Campo id do livro é obrigatório!");
                Livro livroUsuario = _myHomeLibraryContext.Livro.Where(x => x.Ide_Livro == ide_livro).FirstOrDefault();

                if (livroUsuario != null)
                {
                    data.Items.Add(livroUsuario);
                }
                else
                {
                    data = new ResultModel<Livro>(true);
                    data.Messages.Add(new SystemMessageModel { Message = "Livro não encontrado!", Type = SystemMessageTypeEnum.Info });
                }
            }
            catch (Exception ex)
            {
                data = new ResultModel<Livro>(false);
                data.Messages.Add(new SystemMessageModel { Message = ex.Message, Type = SystemMessageTypeEnum.Error });

            }

            return data;
        }
        #endregion

        #region EXCLUIR LIVRO
        public ResultModel<string> Excluir(long ide_livro)
        {
            ResultModel<string> data = new ResultModel<string>(true);

            try
            {
                if (ide_livro <= 0) throw new Exception("Campo id do livro é obrigatório!");
                Livro livroUsuario = _myHomeLibraryContext.Livro.Where(x => x.Ide_Livro == ide_livro).FirstOrDefault();

                if (livroUsuario == null) throw new Exception("Livro não encontrado!");

                var query = _myHomeLibraryContext.Usuario_Livro.Where(x => x.Ide_Livro == livroUsuario.Ide_Livro).FirstOrDefault();

                _myHomeLibraryContext.Livro.Remove(livroUsuario);
                _myHomeLibraryContext.SaveChanges();

                _myHomeLibraryContext.Usuario_Livro.Remove(query);
                _myHomeLibraryContext.SaveChanges();

                data.Messages.Add(new SystemMessageModel { Message = "Livro excluído com sucesso!", Type = SystemMessageTypeEnum.Success });
                data.Items.Add(data.Messages[0].Message);

            }
            catch (Exception ex)
            {
                data = new ResultModel<string>(false);
                data.Messages.Add(new SystemMessageModel { Message = ex.Message, Type = SystemMessageTypeEnum.Error });
                data.Items.Add(data.Messages[0].Message);

            }

            return data;
        }
        #endregion

    }
}
