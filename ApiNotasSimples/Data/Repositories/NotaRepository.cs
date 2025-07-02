using ApiNotasSimples.Data.Context;
using ApiNotasSimples.Models;
using Microsoft.Data.SqlClient;

namespace ApiNotasSimples.Data.Repositories
{
    public class NotaRepository
    {
        private readonly SqlServerContext _context;

        public NotaRepository(SqlServerContext context)
        {
            _context = context;
        }

        public List<NotaModel> ObterPorUsuario(int Id)
        {
            var notas = new List<NotaModel>();

            using (var conn = _context.GetConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Id, Titulo, Conteudo, DataCriacao FROM Notas WHERE UsuarioId = @UsuarioId";
                cmd.Parameters.AddWithValue("@Id", Id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notas.Add(new NotaModel
                        {
                            Id = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Conteudo = reader.GetString(2),
                            DataCriacao = reader.GetDateTime(3)
                        });
                    }
                }
            }

            return notas;
        }

        public async Task<int> Adicionar(NotaModel nota)
        {
            await using var conn = _context.GetConnection();
            await conn.OpenAsync();

            var sql = "INSERT INTO Notas (Titulo, Conteudo, DataCriacao) VALUES (@titulo, @conteudo, @dataCriacao) SELECT SCOPE_IDENTITY()";
            var cmd = new SqlCommand(sql, conn);

           
            cmd.Parameters.AddWithValue("@titulo", nota.Titulo);
            cmd.Parameters.AddWithValue("@conteudo", nota.Conteudo);
            cmd.Parameters.AddWithValue("@dataCriacao", DateTime.Now);

            var Id = Convert.ToInt32(await cmd.ExecuteScalarAsync()) ;
            return Id;
            
        }
    }
}

