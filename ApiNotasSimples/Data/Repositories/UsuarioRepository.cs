using ApiNotasSimples.Data.Context;
using ApiNotasSimples.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ApiCadastroClientes.Data.Repositories
{
    public class UsuarioRepository
    {
        private readonly SqlServerContext _context;
        private readonly ILogger<UsuarioRepository> _logger;

        public UsuarioRepository(SqlServerContext context, ILogger<UsuarioRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //adicionar
        public async Task<int> Adicionar(UsuarioModel usuario)
        {
            await using var conn = _context.GetConnection();
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"INSERT INTO Usuarios (Nome, Email, Senha, Endereco, Cpf, Telefone)
                                       VALUES (@nome, @email, @senha, @endereco, @cpf, @telefone);
                                       SELECT SCOPE_IDENTITY();", conn);
            cmd.Parameters.AddWithValue("@nome", (object)usuario.Nome ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object)usuario.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@senha", (object)usuario.Senha ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@endereco", (object)usuario.Endereco ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cpf", (object)usuario.Cpf ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@telefone", (object)usuario.Telefone ?? DBNull.Value);

            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            _logger.LogInformation("Usuário adicionado com ID: {Id}", id);
            return id;
        }

        //buscar por email
        public async Task<UsuarioModel?> BuscarPorEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("E-mail inválido", nameof(email));
            await using var conn = _context.GetConnection();
            try
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT Id, Nome, Email, Senha, Role FROM Usuarios WHERE Email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var usuario = new UsuarioModel
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Email = reader.GetString(2),
                        Senha = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Role = reader.GetString(4)
                    };
                    _logger.LogDebug("Usuário retornado: {Email}, Senha: {Senha}", email, usuario.Senha);
                    return usuario;
                }
                _logger.LogWarning("Nenhum usuário encontrado para e-mail: {Email}", email);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuário por e-mail: {Email}", email);
                throw;
            }
        }

        //buscar por ID
        public async Task<UsuarioModel?> BuscarPorId(int id)
        {
            await using var conn = _context.GetConnection();
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT Id, Nome, Email, Endereco, Cpf, Telefone FROM Usuarios WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new UsuarioModel
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Email = reader.GetString(2),
                    Endereco = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Cpf = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Telefone = reader.IsDBNull(5) ? null : reader.GetString(5)
                };
            }

            return null;
        }

        //listar todos
        public async Task<List<UsuarioModel>> ListarTodos()
        {
            await using var conn = _context.GetConnection();
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM Usuarios", conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            var lista = new List<UsuarioModel>();
            while (await reader.ReadAsync())
            {
                lista.Add(new UsuarioModel
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Email = reader.GetString(2),
                    Endereco = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Cpf = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Telefone = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }

            return lista;
        }

        //Atualizar
        public async Task<bool> Atualizar(int id, UsuarioModel usuario)
        {
            await using var conn = _context.GetConnection();
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"UPDATE Usuarios 
                                       SET Nome = @nome, Email = @email, Senha = @senha, Endereco = @endereco, Cpf = @cpf, Telefone = @telefone 
                                       WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nome", (object)usuario.Nome ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object)usuario.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@senha", (object)usuario.Senha ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@endereco", (object)usuario.Endereco ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cpf", (object)usuario.Cpf ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@telefone", (object)usuario.Telefone ?? DBNull.Value);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        //remover
        public async Task<bool> Remover(int id)
        {
            await using var conn = _context.GetConnection();
            await conn.OpenAsync();

            var cmd = new SqlCommand("DELETE FROM Usuarios WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}