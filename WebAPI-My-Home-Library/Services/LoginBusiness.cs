using System;
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

        public static ResultModel<LoginRetornoDTO> Logar(LoginFilter filter)
        {
            ResultModel<LoginRetornoDTO> data;
            //DBGerenciamentoContext context = new DBGerenciamentoContext();

            try
            {
                

                LoginRetornoDTO retorno = new LoginRetornoDTO()
                {
                    
                };

                data = new ResultModel<LoginRetornoDTO>();
                data.Items.Add(retorno);

                return data;
            }
            catch (Exception ex)
            {
                data = new ResultModel<LoginRetornoDTO>(false);
                data.Messages.Add(new SystemMessageModel { Message = ex.Message, Type = SystemMessageTypeEnum.Error });

            }

            return null;
        }
        
    }
}
