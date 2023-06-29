using System;
using System.Linq;
using WebAPI_My_Home_Library.Context;
using WebAPI_My_Home_Library.DTOs.Critica;
using WebAPI_My_Home_Library.Filters;
using WebAPI_My_Home_Library.Utils;

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

        public CriticaDTO AlterarDados(AlterarUsuarioFilter filter)
        {

            CriticaDTO retorno = new CriticaDTO();

            try
            {
                var query = _myHomeLibraryContext.Usuario.Where(x => x.Ide_Usuario == filter.Ide_Usuario).FirstOrDefault();

                if (query == null) throw new Exception("Usuário não encontrado!");

                query.Nome = filter.Nome;
                query.Sobrenome = filter.Sobrenome;
                query.Email = filter.Email;

                _myHomeLibraryContext.Update(query);
                _myHomeLibraryContext.SaveChanges();

                retorno.IsOk = true;
                retorno.MensagemRetorno = "Usuário atualizado com sucesso";
 
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
