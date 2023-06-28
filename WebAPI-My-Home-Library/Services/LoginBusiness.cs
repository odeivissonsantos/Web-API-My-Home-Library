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
        private readonly Utilies _utilies;
        public LoginBusiness(MyHomeLibraryContext myHomeLibraryContext, Utilies utilies)
        {
            _myHomeLibraryContext = myHomeLibraryContext;
            _utilies = utilies;

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
                    string senhaEncryptada = Utilies.SHA512(filter.Senha);
                    if (query.Senha != senhaEncryptada) throw new Exception("Email/Senha inválido, verifique os dados e tente novamente!");
                    if (query.Status_Exclusao) throw new Exception("Este usuário não tem mais acesso ao sistema.");

                    query.Data_Ultimo_Acesso = DateTime.Now;
                    query.Quanidade_Acessos += 1;

                    GerarTokenDTO tokenDTO = Utilies.GerarToken();
                    query.Token = tokenDTO.Token;
                    query.Data_Expiracao_Token = tokenDTO.DataExpiracaoToken;

                    _myHomeLibraryContext.Update(query);
                    _myHomeLibraryContext.SaveChanges();

                    LoginRetornoDTO retorno = new LoginRetornoDTO()
                    {
                        NomeUsuario = query.Nome,
                        Email = query.Email,
                        IdeUsuario = query.Ide_Usuario,
                        Token = query.Token
                    };

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

        #region CADASTRO DE NOVOS USUÁRIOS

        public ResultModel<CadastrarUsuarioRetornoDTO> CadastrarNovoUsuario(NovoUsuarioFilter filter)
        {
            ResultModel<CadastrarUsuarioRetornoDTO> data;

            try
            {
                bool isExistsEmail = _utilies.VerificaSeExiste(1, filter.Email);

                if (isExistsEmail) throw new Exception("Já existe um usuário cadastrado com esse Email, mude o email ou solicite recuperação de senha.");
                if (filter.Senha != filter.ConfirmacaoSenha) throw new Exception("Senha e Confirmação de Senha não coincidem, tente novamente.");

                string senhaEncryptada = Utilies.SHA512(filter.Senha);

                Usuario newUsuario = new Usuario()
                {
                    Nome = filter.Nome,
                    Sobrenome = filter.Sobrenome,
                    Email = filter.Email,
                    Senha = senhaEncryptada
                };

                _myHomeLibraryContext.Add(newUsuario);
                _myHomeLibraryContext.SaveChanges();

                CadastrarUsuarioRetornoDTO retorno = new CadastrarUsuarioRetornoDTO()
                {
                    Mensagem = "Usuário cadastrado com sucesso!",
                };

                data = new ResultModel<CadastrarUsuarioRetornoDTO>(true);
                data.Items.Add(retorno);

            }
            catch (Exception ex)
            {
                data = new ResultModel<CadastrarUsuarioRetornoDTO>(false);
                data.Messages.Add(new SystemMessageModel { Message = ex.Message, Type = SystemMessageTypeEnum.Error });

            }

            return data;
        }

        #endregion

        #region VALIDAR TOKEN

        public ResultModel<bool> ValidarToken(LoginFilter filter)
        {
            ResultModel<bool> data;
            bool tokenValido = false;

            try
            {               
                var usuario = _myHomeLibraryContext.Usuario.Where(x => x.Token == filter.Token).FirstOrDefault();

                if (usuario != null)
                {
                    if (usuario.Data_Expiracao_Token < DateTime.Now) throw new Exception("Token Expirado.");

                    data = new ResultModel<bool>(true);                   
                    data.AddMessage("Token válido");
                    tokenValido = true;
                    data.Items.Add(tokenValido);
                }
                else
                {
                    data = new ResultModel<bool>(false);
                    data.Pages = null;
                    data.AddMessage("Token Invalido.");
                }

                return data;
            }
            catch (Exception ex)
            {
                data = new ResultModel<bool>(false);
                data.Pages = null;
                data.Items.Add(tokenValido);
                data.AddMessage(ex.Message);

                return data;
            }        
        }

        #endregion

    }
}
