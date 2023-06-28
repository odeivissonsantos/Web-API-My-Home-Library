using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_My_Home_Library.DTOs.Usuario;
using WebAPI_My_Home_Library.Filters;
using WebAPI_My_Home_Library.Models;
using WebAPI_My_Home_Library.Services;

namespace WebAPI_My_Home_Library.Controllers.api
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioBusiness _usuarioBusiness;
        private readonly LoginBusiness _loginBusiness;

        public UsuarioController( UsuarioBusiness usuarioBusiness, LoginBusiness loginBusiness)
        {
            _usuarioBusiness = usuarioBusiness;
            _loginBusiness = loginBusiness;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultModel<SalvarUsuarioRetornoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ResultModel<SalvarUsuarioRetornoDTO> AlterarDados([FromHeader(Name = "token")] string token, AlterarUsuarioFilter filter)
        {
            ResultModel<SalvarUsuarioRetornoDTO> data = new();

            var tokenValido = _loginBusiness.ValidarToken(new LoginFilter { Token = token });

            if (tokenValido.IsOk)
            {
                data = _usuarioBusiness.AlterarDados(filter);
            }
            else
            {
                data = new ResultModel<SalvarUsuarioRetornoDTO>(false);
                data.Messages = tokenValido.Messages;
            }

            return data;
        }

    }
}
