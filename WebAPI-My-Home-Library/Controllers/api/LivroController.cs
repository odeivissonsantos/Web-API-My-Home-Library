﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly LoginBusiness _loginBusiness;

        public LivroController(LivroBusiness livroBusiness, LoginBusiness loginBusiness)
        {
            _livroBusiness = livroBusiness;
            _loginBusiness = loginBusiness;
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
        public ResultModel<Livro> BuscarLivrosPorUsuario([FromHeader(Name = "token")] string token, long ide_usuario)
        {
            ResultModel<Livro> data = new();

            var tokenValido = _loginBusiness.ValidarToken(new LoginFilter { Token = token });

            if(tokenValido.IsOk)
            {
                data = _livroBusiness.BuscarLivrosPorUsuario(ide_usuario);
            }
            else
            {
                data = new ResultModel<Livro>(false);
                data.Messages = tokenValido.Messages;
            }
            
            return data;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultModel<Livro>), 200)]
        public ResultModel<Livro> BuscarPorID([FromHeader(Name = "token")] string token, long ide_livro)
        {
            var tokenValido = _loginBusiness.ValidarToken(new LoginFilter { Token = token });

            var retorno = _livroBusiness.BuscarPorID(ide_livro);
            return retorno;
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ResultModel<string>), 200)]
        public ResultModel<string> Excluir([FromHeader(Name = "token")] string token, long ide_livro)
        {
            var tokenValido = _loginBusiness.ValidarToken(new LoginFilter { Token = token });

            var retorno = _livroBusiness.Excluir(ide_livro);
            return retorno;
        }

    }
}
