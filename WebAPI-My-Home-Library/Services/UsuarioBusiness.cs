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

        public void AlterarDados(AlterarUsuarioFilter filter)
        {
            var query = _myHomeLibraryContext.Usuario.Where(x => x.Ide_Usuario == filter.Ide_Usuario).FirstOrDefault();

            if (query == null) throw new Exception($"Não foi possível encontrar um usuário com o ID [{filter.Ide_Usuario}].");

            query.Nome = filter.Nome;
            query.Sobrenome = filter.Sobrenome;
            query.Email = filter.Email;

            _myHomeLibraryContext.Update(query);
            _myHomeLibraryContext.SaveChanges();
        }

        #endregion

        #region ALTERAR SENHA

        public void AlterarSenha(AlterarSenhaFilter filter)
        {
            var query = _myHomeLibraryContext.Usuario.Where(x => x.Ide_Usuario == filter.Ide_Usuario).FirstOrDefault();

            if (query == null) throw new Exception($"Não foi possível encontrar um usuário com o ID [{filter.Ide_Usuario}].");
            if (query.Senha != filter.SenhaAtual) throw new Exception("Senha Atual não coincide a Senha Cadastrada, verifique os dados e tente novamente.");

            query.Senha = filter.NovaSenha;

            _myHomeLibraryContext.Update(query);
            _myHomeLibraryContext.SaveChanges();
        }

        #endregion


    }
}
