using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_My_Home_Library.DTOs.Critica;
using WebAPI_My_Home_Library.DTOs.Livro;
using WebAPI_My_Home_Library.Filters;
using WebAPI_My_Home_Library.Models;
using WebAPI_My_Home_Library.Services;
using WebAPI_My_Home_Library.Utils;
using static WebAPI_My_Home_Library.Enums.EnumCommon;

namespace WebAPI_My_Home_Library.Controllers.api
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly LivroBusiness _livroBusiness;
        private readonly LoginBusiness _loginBusiness;
        private readonly Utilies _utilies;

        public LivroController(LivroBusiness livroBusiness, LoginBusiness loginBusiness, Utilies utilies)
        {
            _livroBusiness = livroBusiness;
            _loginBusiness = loginBusiness;
            _utilies = utilies;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CriticaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Novo([FromHeader(Name = "token")] string token, NovoLivroFilter filter)
        {
            CriticaDTO retorno = new();

            try
            {
                if (string.IsNullOrEmpty(filter.Autor)) throw new Exception("Campo autor é obrigatório!");
                if (filter.Ano <= 0) throw new Exception("Campo ano é obrigatório!");
                if (string.IsNullOrEmpty(filter.Editora)) throw new Exception("Campo editora é obrigatório!");
                if (string.IsNullOrEmpty(filter.Titulo)) throw new Exception("Campo título é obrigatório!");

                bool isExistsUsuario = _utilies.VerificaSeExiste(2, filter.Ide_Usuario.ToString());
                if (!isExistsUsuario) throw new Exception($"Não foi possível encontrar um usuário com o ID [{filter.Ide_Usuario}].");

                var tokenValido = _loginBusiness.ValidarToken(new LoginFilter { Token = token });

                if (tokenValido.IsOk)
                {
                    _livroBusiness.Novo(filter);
                    retorno = new CriticaDTO { IsOk = true, MensagemRetorno = "Livro cadastrado com sucesso." };

                    return Created("", retorno);
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
        public IActionResult Editar([FromHeader(Name = "token")] string token, EditarLivroFilter filter)
        {
            CriticaDTO retorno = new CriticaDTO();

            try
            {
                if (string.IsNullOrEmpty(filter.Autor)) throw new Exception("Campo autor é obrigatório!");
                if (filter.Ano <= 0) throw new Exception("Campo ano é obrigatório!");
                if (string.IsNullOrEmpty(filter.Editora)) throw new Exception("Campo editora é obrigatório!");
                if (string.IsNullOrEmpty(filter.Titulo)) throw new Exception("Campo título é obrigatório!");

                var tokenValido = _loginBusiness.ValidarToken(new LoginFilter { Token = token });

                if (tokenValido.IsOk)
                {
                    _livroBusiness.Editar(filter);
                    retorno = new CriticaDTO { IsOk = true, MensagemRetorno = "Livro atualizado com sucesso." };

                    return Ok(retorno);
                }
                else
                {
                    retorno.IsOk = false;
                    retorno.MensagemRetorno = tokenValido.MensagemRetorno;
                    return BadRequest(retorno);
                }
            }
            catch(Exception ex)
            {
                retorno.IsOk = false;
                retorno.MensagemRetorno = ex.Message;
                return BadRequest(retorno);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultModel<List<ListarLivrosUsuarioDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ListarPorUsuario([FromHeader(Name = "token")] string token, long ide_usuario)
        {
            ResultModel<List<ListarLivrosUsuarioDTO>> retorno = new();

            try
            {
                if (ide_usuario <= 0) throw new Exception("Campo id do usuário é obrigatório.");

                bool isExistsEmail = _utilies.VerificaSeExiste(2, ide_usuario.ToString());

                if (!isExistsEmail) throw new Exception($"Não foi possível encontrar um usuário com o ID [{ide_usuario}].");

                var tokenValido = _loginBusiness.ValidarToken(new LoginFilter { Token = token });

                if (tokenValido.IsOk)
                {
                    var listLivros = _livroBusiness.ListarPorUsuario(ide_usuario);

                    if(listLivros.Any())
                    {
                        retorno = new ResultModel<List<ListarLivrosUsuarioDTO>>(true);
                        retorno.Items.Add(listLivros);

                        decimal quantidadePaginas = (decimal)listLivros.Count() / 10;

                        retorno.Pages.TotalItems = listLivros.Count();
                        retorno.Pages.Total = (long)Math.Ceiling(quantidadePaginas);
                        retorno.Pages.Actual = 1;
                        retorno.Pages.Offset = 10 == int.MaxValue ? listLivros.Count() : 10;
                    }
                    else
                    {
                        retorno = new ResultModel<List<ListarLivrosUsuarioDTO>>(true);
                        retorno.Messages.Add(new SystemMessageModel { Message = "Este usuário ainda não possui livros cadastrados.", Type = SystemMessageTypeEnum.Info });
                        return Ok(retorno);
                    }

                }
                else
                {
                    retorno = new ResultModel<List<ListarLivrosUsuarioDTO>>(false);
                    retorno.Messages.Add(new SystemMessageModel { Message = tokenValido.MensagemRetorno, Type = SystemMessageTypeEnum.Error });
                    return BadRequest(retorno);
                }
            }
            catch (Exception ex)
            {
                retorno = new ResultModel<List<ListarLivrosUsuarioDTO>>(false);
                retorno.Messages.Add(new SystemMessageModel { Message = ex.Message, Type = SystemMessageTypeEnum.Error });
                return BadRequest(retorno);
            }
  
            return retorno.IsOk ? Ok(retorno) : NotFound(retorno);
        }

        [HttpGet]
        [ProducesResponseType(typeof(LivroDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BuscarPorID([FromHeader(Name = "token")] string token, long ide_livro)
        {
            LivroDTO retorno = new();

            try
            {
                if (ide_livro <= 0) throw new Exception("Campo ID do livro é obrigatório.");

                var tokenValido = _loginBusiness.ValidarToken(new LoginFilter { Token = token });

                if (tokenValido.IsOk)
                {
                    retorno = _livroBusiness.BuscarPorID(ide_livro);
                    return Ok(retorno);
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

        [HttpDelete]
        [ProducesResponseType(typeof(CriticaDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Excluir([FromHeader(Name = "token")] string token, long ide_livro)
        {
            CriticaDTO retorno = new CriticaDTO();

            try
            {
                if (ide_livro <= 0) throw new Exception("Campo ID do livro é obrigatório.");

                var tokenValido = _loginBusiness.ValidarToken(new LoginFilter { Token = token });

                if (tokenValido.IsOk)
                {
                    _livroBusiness.Excluir(ide_livro);
                    retorno = new CriticaDTO { IsOk = true, MensagemRetorno = "Livro excluído com sucesso." };
                    return Ok(retorno);
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
