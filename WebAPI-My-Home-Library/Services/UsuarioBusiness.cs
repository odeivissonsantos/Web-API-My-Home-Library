using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_My_Home_Library.Context;
using WebAPI_My_Home_Library.DTOs.Usuario;
using WebAPI_My_Home_Library.Filters;
using WebAPI_My_Home_Library.Models;
using WebAPI_My_Home_Library.Utils;
using static WebAPI_My_Home_Library.Enums.EnumCommon;

namespace WebAPI_My_Home_Library.Services
{
    public class UsuarioBusiness
    {
        public static bool isDesenv = Settings.IsDesenv;

        private readonly MyHomeLibraryContext _myHomeLibraryContext;
        private readonly Utilies _utilies;
        public UsuarioBusiness(MyHomeLibraryContext myHomeLibraryContext, Utilies utilies)
        {
            _myHomeLibraryContext = myHomeLibraryContext;
            _utilies = utilies;

        }

        #region SALVAR USUÁRIO

        public ResultModel<CadastrarUsuarioRetornoDTO> Salvar(SalvarUsuarioFilter filter)
        {
            bool novo = false;
            ResultModel<CadastrarUsuarioRetornoDTO> data;

            if (filter.Ide_Usuario <= 0) novo = true;

            try
            {
                if (!novo)
                {
                    var query = _myHomeLibraryContext.Usuario.Where(x => x.Ide_Usuario == filter.Ide_Usuario).FirstOrDefault();

                    if (query == null) throw new Exception("Usuário não encontrado!");

                    query.Nome = filter.Nome;
                    query.Sobrenome = filter.Sobrenome;
                    query.Email = filter.Email;

                    CadastrarUsuarioRetornoDTO retorno = new CadastrarUsuarioRetornoDTO()
                    {
                        Mensagem = "Usuário atualizado com sucesso",
                    };

                    _myHomeLibraryContext.Update(query);

                    data = new ResultModel<CadastrarUsuarioRetornoDTO>(true);
                    data.Items.Add(retorno);

                }
                else
                {
                    bool isExistsEmail = _utilies.VerificaSeExiste(1, filter.Email);

                    if (isExistsEmail) throw new Exception("Usuário já cadastrado com esse Email");

                    Usuario newUsuario = new Usuario()
                    {
                        Nome = filter.Nome,
                        Sobrenome = filter.Sobrenome,
                        Email = filter.Email,
                        Senha = filter.Senha
                    };

                    _myHomeLibraryContext.Add(newUsuario);

                    CadastrarUsuarioRetornoDTO retorno = new CadastrarUsuarioRetornoDTO()
                    {
                        Mensagem = "Usuário cadastrado com sucesso!",
                    };

                    data = new ResultModel<CadastrarUsuarioRetornoDTO>(true);
                    data.Items.Add(retorno);

                }

                _myHomeLibraryContext.SaveChanges();

            }
            catch (Exception ex)
            {
                data = new ResultModel<CadastrarUsuarioRetornoDTO>(false);
                data.Messages.Add(new SystemMessageModel { Message = ex.Message, Type = SystemMessageTypeEnum.Error });

            }

            return data;
        }

        #endregion


    }
}
