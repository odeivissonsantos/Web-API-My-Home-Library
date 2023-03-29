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
        private readonly ILogger<UsuarioController> _logger;
        private readonly UsuarioBusiness _usuarioBusiness;

        public UsuarioController(ILogger<UsuarioController> logger, UsuarioBusiness usuarioBusiness)
        {
            _logger = logger;
            _usuarioBusiness = usuarioBusiness;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultModel<SalvarUsuarioRetornoDTO>), 200)]
        public ResultModel<SalvarUsuarioRetornoDTO> Salvar(SalvarUsuarioFilter filter)
        {
            var retorno = _usuarioBusiness.Salvar(filter);
            return retorno;
        }

    }
}
