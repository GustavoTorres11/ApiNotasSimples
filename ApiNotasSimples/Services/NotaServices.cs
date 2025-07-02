using ApiNotasSimples.Data.Repositories;
using ApiNotasSimples.Models;
using ApiNotasSimples.Models.DTO;

namespace ApiNotasSimples.Services
{
    public class NotaServices
    {
        private readonly NotaRepository _repo;
        public NotaServices(NotaRepository repo)
        {
            _repo = repo;
        }

        public async Task<NotaModel> Adicionar(NotaDTO notaDto, int usuarioId)
        {
            if (notaDto == null)
                throw new ArgumentNullException(nameof(notaDto), "Nota não pode ser nula");

            var nota = new NotaModel
            {
                Titulo = notaDto.Titulo,
                Conteudo = notaDto.Conteudo,
                UsuarioId = usuarioId
            };
            var id = await _repo.Adicionar(nota);
            nota.Id = id;
             return nota;
        }
        



    }   
}
