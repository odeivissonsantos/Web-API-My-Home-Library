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
        public UsuarioBusiness(MyHomeLibraryContext myHomeLibraryContext)
        {
            _myHomeLibraryContext = myHomeLibraryContext;

        }

        public ResultModel<SalvarUsuarioRetornoDTO> Salvar(SalvarUsuarioFilter filter)
        {
            bool novo = false;
            ResultModel<SalvarUsuarioRetornoDTO> data;

            if (string.IsNullOrEmpty(filter.Guuid)) novo = true;

            try
            {
                if(!novo)
                {
                    var query = _myHomeLibraryContext.Usuario.Where(x => x.Guuid == filter.Guuid).FirstOrDefault();

                    if (query == null) throw new Exception("Usuário não encontrado!");

                    query.Nome = filter.Nome;
                    query.Sobrenome = filter.Sobrenome;
                    query.Email = filter.Email;
                    query.Cpf = query.Cpf;

                    SalvarUsuarioRetornoDTO retorno = new SalvarUsuarioRetornoDTO()
                    {
                        Mensagem = "Usuário atualizado com sucesso",
                    };

                    _myHomeLibraryContext.Update(query);
                    
                    data = new ResultModel<SalvarUsuarioRetornoDTO>(true);
                    data.Items.Add(retorno);

                }
                else
                {
                    Usuario newUsuario = new Usuario()
                    {
                        Cpf = filter.Cpf,
                        Email = filter.Email,
                        Senha = filter.Senha,
                        Nome = filter.Nome,
                        Sobrenome = filter.Sobrenome,
                        Guuid = Guid.NewGuid().ToString()
                    };

                    _myHomeLibraryContext.Add(newUsuario);

                    SalvarUsuarioRetornoDTO retorno = new SalvarUsuarioRetornoDTO()
                    {
                        Mensagem = "Usuário cadastrado com sucesso!",
                    };

                    data = new ResultModel<SalvarUsuarioRetornoDTO>(true);
                    data.Items.Add(retorno);

                }

                _myHomeLibraryContext.SaveChanges();

            }
            catch (Exception ex)
            {
                data = new ResultModel<SalvarUsuarioRetornoDTO>(false);
                data.Messages.Add(new SystemMessageModel { Message = ex.Message, Type = SystemMessageTypeEnum.Error });

            }

            return data;
        }
    }
}
