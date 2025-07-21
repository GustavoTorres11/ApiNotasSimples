using ApiCadastroClientes.Data.Repositories;
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
        if (usuario == null || !ModelState.IsValid)
            return BadRequest("Dados inválidos.");

        usuario.Senha = _cryptoService.HashPassword(usuario.Senha);
        var novoUsuario = await _repo.Adicionar(usuario);

        if (novoUsuario == null)
        {
            return BadRequest(new { mensagem = "Erro ao cadastrar usuário." });
        }

        return Ok(new { mensagem = "Usuário cadastrado com sucesso", id = novoUsuario.Id });

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
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var usuario = await _repo.BuscarPorId(id);
        if (usuario == null)
            return NotFound();
        return Ok(usuario);
    }

    // PUT: Atualiza usuário por ID
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioModel usuario)
    {
        usuario.Senha = _cryptoService.HashPassword(usuario.Senha);
        var atualizado = await _repo.Atualizar(id, usuario);
        return Ok(new { mensagem = "Atualizado com sucesso" });
    }

    // DELETE: Remove usuário por ID
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var removido = await _repo.Remover(id);
        return Ok(new { mensagem = "Removido com sucesso" });
    }
}