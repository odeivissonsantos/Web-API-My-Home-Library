using System;
using System.Linq;
using WebAPI_My_Home_Library.Context;
using WebAPI_My_Home_Library.DTOs.Critica;
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
        private readonly Utilies _utilies;
        public LoginBusiness(MyHomeLibraryContext myHomeLibraryContext, Utilies utilies)
        {
            _myHomeLibraryContext = myHomeLibraryContext;
            _utilies = utilies;

        }

        #region LOGIN DO USUÁRIO
        public LoginRetornoDTO Logar(LoginFilter filter)
        {
            LoginRetornoDTO retorno = new();
            var query = _myHomeLibraryContext.Usuario.Where(x => x.Email == filter.Email).FirstOrDefault();

            if(query == null || query.Senha != filter.Senha) throw new Exception("Email/Senha inválido, verifique os dados e tente novamente.");
            if (query.Status_Exclusao) throw new Exception("Este usuário não tem mais acesso ao sistema.");

            var usuarioAcessos = _myHomeLibraryContext.Usuario_Acessos.Where(x => x.Ide_Usuario == query.Ide_Usuario).FirstOrDefault();

            usuarioAcessos.Data_Ultimo_Acesso = DateTime.Now;
            usuarioAcessos.Quanidade_Acessos += 1;

            var usuarioToken = _myHomeLibraryContext.Usuario_Token.Where(x => x.Ide_Usuario == query.Ide_Usuario).FirstOrDefault();

            GerarTokenDTO tokenDTO = Utilies.GerarToken();
            usuarioToken.Token = tokenDTO.Token;
            usuarioToken.Data_Expiracao_Token = tokenDTO.DataExpiracaoToken;

            _myHomeLibraryContext.Usuario_Acessos.Update(usuarioAcessos);
            _myHomeLibraryContext.Usuario_Token.Update(usuarioToken);
            _myHomeLibraryContext.SaveChanges();

            retorno = new(query, usuarioToken);
            retorno.IsOk = true;

            return retorno;
        }
        #endregion

        #region CADASTRO DE NOVOS USUÁRIOS

        public void CadastrarNovoUsuario(NovoUsuarioFilter filter)
        {
            Usuario newUsuario = new Usuario()
            {
                Nome = filter.Nome,
                Sobrenome = filter.Sobrenome,
                Email = filter.Email,
                Senha = filter.Senha,
            };

            _myHomeLibraryContext.Usuario.Add(newUsuario);
            _myHomeLibraryContext.SaveChanges();

            UsuarioAcessos newUsuarioAcessos = new UsuarioAcessos()
            {
                Ide_Usuario = newUsuario.Ide_Usuario
            };

            _myHomeLibraryContext.Usuario_Acessos.Add(newUsuarioAcessos);

            UsuarioToken newUsuarioToken = new UsuarioToken()
            {
                Ide_Usuario = newUsuario.Ide_Usuario
            };

            _myHomeLibraryContext.Usuario_Token.Add(newUsuarioToken);
            _myHomeLibraryContext.SaveChanges();

        }

        #endregion

        #region VALIDAR TOKEN

        public CriticaDTO ValidarToken(LoginFilter filter)
        {
            CriticaDTO retorno = new CriticaDTO();

            try
            {               
                var usuarioToken = _myHomeLibraryContext.Usuario_Token.Where(x => x.Token == filter.Token).FirstOrDefault();

                if (usuarioToken != null)
                {
                    if (usuarioToken.Data_Expiracao_Token < DateTime.Now) throw new Exception("Token Expirado.");

                    retorno.IsOk = true;                  
                    retorno.MensagemRetorno = "Token válido";
                }
                else
                {
                    retorno.IsOk = false;
                    retorno.MensagemRetorno = "Token inválido.";
                }

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.IsOk = false;
                retorno.MensagemRetorno = ex.Message;

                return retorno;
            }        
        }

        #endregion

    }
}
