using ApiCadastroClientes.Data.Repositories;
using ApiCadastroClientes.Models.DTO;
using ApiCadastroClientes.Services;
using ApiNotasSimples.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioRepository _repo; 
    private readonly AuthService _authService; 

    // Injeção de dependência
    public UsuarioController(UsuarioRepository repo, AuthService authService)
    {
        _repo = repo;
        _authService = authService;
    }

    // GET: Lista todos os usuários
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> ListarTodos()
    {
        return Ok(await _repo.ListarTodos());
    }

    // GET: Busca usuário por ID
    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var usuario = _repo.BuscarPorId(id);
        if (usuario == null)
            return NotFound();
        return Ok(usuario);
    }

    // PUT: Atualiza usuário por ID
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioModel usuario)
    {
        var atualizado = await _repo.Atualizar(id, usuario);
        return atualizado ? Ok("Atualizado com sucesso") : NotFound();
    }

    // DELETE: Remove usuário por ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        var removido = await _repo.Remover(id);
        return removido ? Ok("Removido com sucesso") : NotFound();
    }
}