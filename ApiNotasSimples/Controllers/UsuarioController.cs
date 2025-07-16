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
    private readonly CryptoService _cryptoService;

    // Injeção de dependência
    public UsuarioController(UsuarioRepository repo, AuthService authService, CryptoService cryptoService)
    {
        _repo = repo;
        _authService = authService;
        _cryptoService = cryptoService;
    }

    // POST: Cadastra novo usuário
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Cadastrar([FromBody] UsuarioModel usuario)
    {
        usuario.Senha = _cryptoService.HashPassword(usuario.Senha);
        var id = await _repo.Adicionar(usuario);
        
        if (id <= 0)
            return BadRequest(new { mensagem = "Erro ao cadastrar usuário." });

            return Ok(new { mensagem = "Usuario cadastrado com sucesso" });

    }

    // GET: Lista todos os usuários
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult> ListarTodos()
    {
        return Ok(await _repo.ListarTodos());
    }

    // GET: Busca usuário por ID
    [HttpGet("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var usuario = await _repo.BuscarPorId(id);
        if (usuario == null)
            return NotFound();
        return Ok(usuario);
    }

    // PUT: Atualiza usuário por ID
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioModel usuario)
    {
        usuario.Senha = _cryptoService.HashPassword(usuario.Senha);
        var atualizado = await _repo.Atualizar(id, usuario);
        return Ok(new { mensagem = "Atualizado com sucesso" });
    }

    // DELETE: Remove usuário por ID
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Remover(int id)
    {
        var removido = await _repo.Remover(id);
        return Ok(new { mensagem = "Removido com sucesso" });
    }
}