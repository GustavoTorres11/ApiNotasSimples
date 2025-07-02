using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiNotasSimples.Data.Repositories;
using ApiNotasSimples.Models;
using System.Security.Claims;
using ApiNotasSimples.Models.DTO;
using ApiNotasSimples.Services;

namespace ApiNotasSimples.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotaController : ControllerBase
    {
        private readonly NotaRepository _repo;

        private readonly NotaServices _services;
        public NotaController(NotaRepository repo, NotaServices services)
        {
            _repo = repo;
            _services = services;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var Id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var notas = _repo.ObterPorUsuario(Id);
            return Ok(notas);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NotaDTO nota)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var nota1 = await _services.Adicionar(nota, usuarioId);


                return Ok(nota1);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }
    }
}

