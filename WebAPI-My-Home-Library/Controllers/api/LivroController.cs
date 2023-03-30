using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_My_Home_Library.DTOs.Livro;
using WebAPI_My_Home_Library.Filters;
using WebAPI_My_Home_Library.Models;
using WebAPI_My_Home_Library.Services;

namespace WebAPI_My_Home_Library.Controllers.api
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class LivroController : ControllerBase
    {

        private readonly ILogger<LivroController> _logger;
        private readonly LivroBusiness _livroBusiness;

        public LivroController(ILogger<LivroController> logger, LivroBusiness livroBusiness)
        {
            _logger = logger;
            _livroBusiness = livroBusiness;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultModel<SalvarLivroRetornoDTO>), 200)]
        public ResultModel<SalvarLivroRetornoDTO> Salvar(SalvarLivroFilter filter)
        {
            var retorno = _livroBusiness.Salvar(filter);
            return retorno;
        }

    }
}
