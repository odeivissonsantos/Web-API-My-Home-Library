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
            LoginRetornoDTO retorno = new LoginRetornoDTO();

            try
            {
                var query = _myHomeLibraryContext.Usuario.Where(x => x.Email == filter.Email).FirstOrDefault();

                if (query != null)
                {
                    if (query.Senha != filter.Senha) throw new Exception("Email/Senha inválido, verifique os dados e tente novamente!");
                    if (query.Status_Exclusao) throw new Exception("Este usuário não tem mais acesso ao sistema.");

                    query.Data_Ultimo_Acesso = DateTime.Now;
                    query.Quanidade_Acessos += 1;

                    GerarTokenDTO tokenDTO = Utilies.GerarToken();
                    query.Token = tokenDTO.Token;
                    query.Data_Expiracao_Token = tokenDTO.DataExpiracaoToken;

                    _myHomeLibraryContext.Update(query);
                    _myHomeLibraryContext.SaveChanges();

                    retorno = new(query);

                }
                else
                {
                    throw new Exception("Email/Senha inválido, verifique os dados e tente novamente.");
                }

            }
            catch (Exception ex)
            {
                retorno.IsOk = false;
                retorno.MensagemRetorno = ex.Message;
            }

            return retorno;
        }
        #endregion

        #region CADASTRO DE NOVOS USUÁRIOS

        public CadastrarUsuarioRetornoDTO CadastrarNovoUsuario(NovoUsuarioFilter filter)
        {
            CadastrarUsuarioRetornoDTO retorno = new CadastrarUsuarioRetornoDTO();

            try
            {
                
                Usuario newUsuario = new Usuario()
                {
                    Nome = filter.Nome,
                    Sobrenome = filter.Sobrenome,
                    Email = filter.Email,
                    Senha = filter.Senha,
                };

                _myHomeLibraryContext.Add(newUsuario);
                _myHomeLibraryContext.SaveChanges();

                retorno = new (newUsuario);
            }
            catch (Exception ex)
            {
                retorno.IsOk = false;
                retorno.MensagemRetorno = ex.Message;
            }

            return retorno;
        }

        #endregion

        #region VALIDAR TOKEN

        public CriticaDTO ValidarToken(LoginFilter filter)
        {
            CriticaDTO retorno = new CriticaDTO();

            try
            {               
                var usuario = _myHomeLibraryContext.Usuario.Where(x => x.Token == filter.Token).FirstOrDefault();

                if (usuario != null)
                {
                    if (usuario.Data_Expiracao_Token < DateTime.Now) throw new Exception("Token Expirado.");

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
