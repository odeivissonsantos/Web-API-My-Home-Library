using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(typeof(ResultModel<LoginRetornoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Logar(LoginFilter filter)
        {
            var retorno = _loginBusiness.Logar(filter);
            return Ok(retorno);
            //return retorno;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultModel<CadastrarUsuarioRetornoDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CadastrarNovoUsuario(NovoUsuarioFilter filter)
        {
            var retorno = _loginBusiness.CadastrarNovoUsuario(filter);

            return Created("" , retorno);
        }

    }
}
