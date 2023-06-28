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

        public UsuarioController( UsuarioBusiness usuarioBusiness)
        {
            _usuarioBusiness = usuarioBusiness;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultModel<SalvarUsuarioRetornoDTO>), 201)]
        [ProducesResponseType(typeof(ResultModel<SalvarUsuarioRetornoDTO>), 400)]
        public ResultModel<SalvarUsuarioRetornoDTO> Salvar(SalvarUsuarioFilter filter)
        {
            var retorno = _usuarioBusiness.Salvar(filter);
            return retorno;
        }

    }
}
