using System;
using System.Linq;
using WebAPI_My_Home_Library.Context;
using WebAPI_My_Home_Library.DTOs.Login;
using WebAPI_My_Home_Library.Filters;
using WebAPI_My_Home_Library.Models;
using WebAPI_My_Home_Library.Utils;
using static WebAPI_My_Home_Library.Enums.EnumCommon;

namespace WebAPI_My_Home_Library.Services
{
    public class LoginBusiness
    {
        public static bool isDesenv = Settings.IsDesenv;

        private readonly MyHomeLibraryContext _myHomeLibraryContext;
        public LoginBusiness(MyHomeLibraryContext myHomeLibraryContext)
        {
            _myHomeLibraryContext = myHomeLibraryContext;

        }

        #region LOGIN DO USUÁRIO
        public ResultModel<LoginRetornoDTO> Logar(LoginFilter filter)
        {
            ResultModel<LoginRetornoDTO> data;

            try
            {
                var query = _myHomeLibraryContext.Usuario.Where(x => x.Email == filter.Email).FirstOrDefault();

                if (query != null)
                {
                    if (query.Senha != filter.Senha) throw new Exception("Email/Senha inválido, verifique os dados e tente novamente!");

                    LoginRetornoDTO retorno = new LoginRetornoDTO()
                    {
                        NomeUsuario = query.Nome,
                        Email = query.Email,
                        IdeUsuario = query.Ide_Usuario
                    };

                    query.Data_Ultimo_Acesso = DateTime.Now;
                    query.Quanidade_Acessos += 1;

                    _myHomeLibraryContext.Update(query);
                    _myHomeLibraryContext.SaveChanges();

                    data = new ResultModel<LoginRetornoDTO>(true);
                    data.Items.Add(retorno);

                    return data;
                }
                else
                {
                    data = new ResultModel<LoginRetornoDTO>(false);
                    data.Messages.Add(new SystemMessageModel { Message = "Email/Senha inválido, verifique os dados e tente novamente!", Type = SystemMessageTypeEnum.Error });
                }

            }
            catch (Exception ex)
            {
                data = new ResultModel<LoginRetornoDTO>(false);
                data.Messages.Add(new SystemMessageModel { Message = ex.Message, Type = SystemMessageTypeEnum.Error });

            }

            return data;
        }
        #endregion

    }
}
