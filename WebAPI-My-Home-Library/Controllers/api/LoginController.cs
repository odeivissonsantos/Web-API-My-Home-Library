using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI_My_Home_Library.DTOs.Critica;
using WebAPI_My_Home_Library.DTOs.Login;
using WebAPI_My_Home_Library.Filters;
using WebAPI_My_Home_Library.Models;
using WebAPI_My_Home_Library.Services;
using WebAPI_My_Home_Library.Utils;

namespace WebAPI_My_Home_Library.Controllers.api
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginBusiness _loginBusiness;
        private readonly Utilies _utilies;

        public LoginController(LoginBusiness loginBusiness, Utilies utilies)
        {
            _loginBusiness = loginBusiness;
            _utilies = utilies;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginRetornoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Logar(LoginFilter filter)
        {
            LoginRetornoDTO retorno = new LoginRetornoDTO();
            try
            {
                if (string.IsNullOrEmpty(filter.Email)) throw new Exception("Campo Email é obrigatório.");
                if (string.IsNullOrEmpty(filter.Senha)) throw new Exception("Campo Senha é obrigatório.");

                filter.Senha = Utilies.SHA512(filter.Senha);
                retorno = _loginBusiness.Logar(filter);

                return retorno.IsOk ? Ok(retorno) : BadRequest(retorno);
            }
            catch(Exception ex)
            {
                retorno.IsOk = false;
                retorno.MensagemRetorno = ex.Message;
                return BadRequest(retorno);
            }
            
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultModel<CriticaDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CadastrarNovoUsuario(NovoUsuarioFilter filter)
        {
            CriticaDTO retorno = new();

            try
            {
                bool isExistsEmail = _utilies.VerificaSeExiste(1, filter.Email);

                if (isExistsEmail) throw new Exception("Já existe um usuário cadastrado com esse Email, troque o email ou solicite recuperação de senha.");

                if (filter.Senha != filter.ConfirmacaoSenha) throw new Exception("Senha e Confirmação de Senha não coincidem, tente novamente.");

                filter.Senha = Utilies.SHA512(filter.Senha);

                _loginBusiness.CadastrarNovoUsuario(filter);

                retorno = new CriticaDTO { IsOk = true, MensagemRetorno = "Usuário cadastrado com sucesso." };

                return Created("", retorno);
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
