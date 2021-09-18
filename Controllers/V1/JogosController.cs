using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {

        private readonly IJogoService _ijogoService;


        public JogosController(IJogoService jogoService) 
        {
            this._ijogoService = jogoService;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1,50)] int quantidade = 5) 
        {

            var jogos = await _ijogoService.Obter(pagina, quantidade);

            if (jogos.Count() == 0)
            {
                return NoContent();
            }

            return Ok(jogos);
        }


        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid idJogo)
        {
            var jogo = await _ijogoService.Obter(idJogo);

            if (jogo == null) {
                return NoContent();
            }

            return Ok(jogo);
        }

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoInputModel jogoInputModel)
        {
            try {
                var jogo = await _ijogoService.Inserir(jogoInputModel);
                return Ok(jogo);
            }
            catch (JogoJaCadastradoException ex)
      
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora");
            }

        }


        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid id, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                await _ijogoService.Atualizar(id, jogoInputModel);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)     
            {
                return NotFound("Não existe este jogo");
            }
        }

        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid id, [FromRoute] double preco)
        {
            try
            {
                await _ijogoService.Atualizar(id, preco);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }

        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarJogo([FromRoute] Guid id)
        {
            try
            {
                await _ijogoService.Remover(id);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }
    }
}
