using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI_My_Home_Library.DTOs.Critica;
using WebAPI_My_Home_Library.Filters;
using WebAPI_My_Home_Library.Services;
using WebAPI_My_Home_Library.Utils;

namespace WebAPI_My_Home_Library.Controllers.api
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioBusiness _usuarioBusiness;
        private readonly LoginBusiness _loginBusiness;
        private readonly Utilies _utilies;

        public UsuarioController( UsuarioBusiness usuarioBusiness, LoginBusiness loginBusiness, Utilies utilies)
        {
            _usuarioBusiness = usuarioBusiness;
            _loginBusiness = loginBusiness;
            _utilies = utilies;
        }

        [HttpPut]
        [ProducesResponseType(typeof(CriticaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AlterarDados([FromHeader(Name = "token")] string token, AlterarUsuarioFilter filter)
        {
            CriticaDTO retorno = new CriticaDTO();

            try
            {
                if (string.IsNullOrEmpty(token)) throw new Exception("Parâmetro Token é obrigatório");
                if (filter.Ide_Usuario <= 0 ) throw new Exception("Campo ID do usuário é obrigatório");
                if (string.IsNullOrEmpty(filter.Nome)) throw new Exception("Campo Nome é obrigatório");
                if (string.IsNullOrEmpty(filter.Sobrenome)) throw new Exception("Campo Sobrenome é obrigatório");
                if (string.IsNullOrEmpty(filter.Email)) throw new Exception("Campo Email é obrigatório");

                var tokenValido = _loginBusiness.ValidarToken(new LoginFilter { Token = token });

                if (tokenValido.IsOk)
                {
                    retorno = _usuarioBusiness.AlterarDados(filter);
                    return retorno.IsOk? Ok(retorno) : BadRequest(retorno);
                }
                else
                {
                    retorno.IsOk = false;
                    retorno.MensagemRetorno = tokenValido.MensagemRetorno;
                    return BadRequest(retorno);
                }
            }
            catch (Exception ex)
            {
                retorno.IsOk = false;
                retorno.MensagemRetorno = ex.Message;
                return BadRequest(retorno);
            }   
        }

        [HttpPut]
        [ProducesResponseType(typeof(CriticaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AlterarSenha([FromHeader(Name = "token")] string token, AlterarSenhaFilter filter)
        {
            CriticaDTO retorno = new CriticaDTO();

            try
            {
                if (string.IsNullOrEmpty(token)) throw new Exception("Parâmetro Token é obrigatório");
                if (filter.Ide_Usuario <= 0) throw new Exception("Campo ID do usuário é obrigatório");
                if (string.IsNullOrEmpty(filter.SenhaAtual)) throw new Exception("Campo Senha Atual é obrigatório.");
                if (string.IsNullOrEmpty(filter.NovaSenha)) throw new Exception("Campo Nova Senha é obrigatório.");
                if (string.IsNullOrEmpty(filter.ConfirmacaoNovaSenha)) throw new Exception("Campo Confirmacao de Nova Senha é obrigatório.");

                bool isExistsEmail = _utilies.VerificaSeExiste(2, filter.Ide_Usuario.ToString());

                if (!isExistsEmail) throw new Exception($"Não foi possível encontrar um usuário com o ID [{filter.Ide_Usuario}].");

                if (filter.NovaSenha != filter.ConfirmacaoNovaSenha) throw new Exception("Nova Senha e Confirmação de Senha não coincidem, tente novamente.");

                filter.SenhaAtual = Utilies.SHA512(filter.SenhaAtual);
                filter.NovaSenha = Utilies.SHA512(filter.NovaSenha);

                var tokenValido = _loginBusiness.ValidarToken(new LoginFilter { Token = token });

                if (tokenValido.IsOk)
                {
                    retorno = _usuarioBusiness.AlterarSenha(filter);
                    return retorno.IsOk ? Ok(retorno) : BadRequest(retorno);
                }
                else
                {
                    retorno.IsOk = false;
                    retorno.MensagemRetorno = tokenValido.MensagemRetorno;
                    return BadRequest(retorno);
                }
            }
            catch (Exception ex)
            {
                retorno.IsOk = false;
                retorno.MensagemRetorno = ex.Message;
                return BadRequest(retorno);
            }
        }

    }
}
