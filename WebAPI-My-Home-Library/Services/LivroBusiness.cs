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

        public ResultModel<SalvarLivroRetornoDTO> Salvar(SalvarLivroFilter filter)
        {
            bool novo = false;
            Livro newLivro = new Livro();
            UsuarioLivro newUsuarioLivro = new UsuarioLivro();
            ResultModel<SalvarLivroRetornoDTO> data;

            if (string.IsNullOrEmpty(filter.Guuid)) novo = true;

            try
            {
                if (string.IsNullOrEmpty(filter.Guuid_Usuario)) throw new Exception("Campo guid do usuário é obrigatório!");
                if (string.IsNullOrEmpty(filter.Autor)) throw new Exception("Campo autor é obrigatório!");
                if (filter.Ano <= 0) throw new Exception("Campo ano é obrigatório!");
                if (string.IsNullOrEmpty(filter.Editora)) throw new Exception("Campo editora é obrigatório!");
                if (string.IsNullOrEmpty(filter.Titulo)) throw new Exception("Campo título é obrigatório!");

                if (!novo)
                {
                    var query = _myHomeLibraryContext.Livro.Where(x => x.Guuid == filter.Guuid).FirstOrDefault();

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
                    bool isExistsUsuario = _utilies.VerificaSeExiste(3, filter.Guuid_Usuario);
                    if (!isExistsUsuario) throw new Exception("Usuário não encontrado, digite um usuário válido");

                    newLivro.Guuid = Guid.NewGuid().ToString();
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

                if(novo)
                {
                    newUsuarioLivro.Guuid_Livro = newLivro.Guuid;
                    newUsuarioLivro.Guuid_Usuario = filter.Guuid_Usuario;

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
    }
}
