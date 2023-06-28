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
        private readonly LivroBusiness _livroBusiness;

        public LivroController(LivroBusiness livroBusiness)
        {
            _livroBusiness = livroBusiness;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultModel<SalvarLivroRetornoDTO>), 200)]
        public ResultModel<SalvarLivroRetornoDTO> Salvar(SalvarLivroFilter filter)
        {
            var retorno = _livroBusiness.Salvar(filter);
            return retorno;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultModel<Livro>), 200)]
        public ResultModel<Livro> BuscarLivrosPorUsuario(long ide_usuario)
        {
            var retorno = _livroBusiness.BuscarLivrosPorUsuario(ide_usuario);
            return retorno;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultModel<Livro>), 200)]
        public ResultModel<Livro> BuscarPorID(long ide_livro)
        {
            var retorno = _livroBusiness.BuscarPorID(ide_livro);
            return retorno;
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ResultModel<string>), 200)]
        public ResultModel<string> Excluir(long ide_livro)
        {
            var retorno = _livroBusiness.Excluir(ide_livro);
            return retorno;
        }

    }
}
