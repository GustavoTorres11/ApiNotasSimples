using ApiCadastroClientes.Data.Repositories;
using ApiCadastroClientes.Models.DTO;
using ApiCadastroClientes.Services;
using ApiNotasSimples.Models;
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

    // POST: Cadastra novo usuário
    [HttpPost("Cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] UsuarioDTO usuario)
    {
        var usuarioCadastrado = await _authService.Cadastrar(usuario);
        return Ok(usuarioCadastrado);
    }

    // POST: Realiza login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        var usuario = await _authService.Logar(login);
        if (usuario == null)
            return Unauthorized("Email ou senha incorretos.");
        return Ok("Login realizado com sucesso " + usuario);
    }

    // GET: Lista todos os usuários
    [HttpGet]
    public IActionResult ListarTodos() => Ok(_repo.ListarTodos());

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