using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WebAPI_My_Home_Library.DTOs.Login;
using WebAPI_My_Home_Library.Filters;
using WebAPI_My_Home_Library.Models;
using WebAPI_My_Home_Library.Services;

namespace WebAPI_My_Home_Library.Controllers.api
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginBusiness _loginBusiness;

        public LoginController(LoginBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultModel<LoginRetornoDTO>), 200)]
        public ResultModel<LoginRetornoDTO> Logar(LoginFilter filter)
        {
            var retorno = _loginBusiness.Logar(filter);
            return retorno;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultModel<CadastrarUsuarioRetornoDTO>), 201)]
        public ResultModel<CadastrarUsuarioRetornoDTO> CadastrarNovoUsuario(NovoUsuarioFilter filter)
        {
            var retorno = _loginBusiness.CadastrarNovoUsuario(filter);
            return retorno;
        }

    }
}
