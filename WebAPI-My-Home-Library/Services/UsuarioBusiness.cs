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

        public ResultModel<SalvarUsuarioRetornoDTO> AlterarDados(AlterarUsuarioFilter filter)
        {

            ResultModel<SalvarUsuarioRetornoDTO> data;

            try
            {

                var query = _myHomeLibraryContext.Usuario.Where(x => x.Ide_Usuario == filter.Ide_Usuario).FirstOrDefault();

                if (query == null) throw new Exception("Usuário não encontrado!");

                query.Nome = filter.Nome;
                query.Sobrenome = filter.Sobrenome;
                query.Email = filter.Email;

                _myHomeLibraryContext.Update(query);
                _myHomeLibraryContext.SaveChanges();


                SalvarUsuarioRetornoDTO retorno = new SalvarUsuarioRetornoDTO()
                {
                    Mensagem = "Usuário atualizado com sucesso",
                };

                data = new ResultModel<SalvarUsuarioRetornoDTO>(true);
                data.Items.Add(retorno);
 
            }
            catch (Exception ex)
            {
                data = new ResultModel<SalvarUsuarioRetornoDTO>(false);
                data.Messages.Add(new SystemMessageModel { Message = ex.Message, Type = SystemMessageTypeEnum.Error });

            }

            return data;
        }

        #endregion


    }
}
