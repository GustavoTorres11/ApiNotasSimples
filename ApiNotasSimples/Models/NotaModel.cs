namespace ApiNotasSimples.Models
{
    public class NotaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataCriacao { get; set; }
        public int UsuarioId { get; set; }

        public NotaModel()
        {
            DataCriacao = DateTime.Now;
        }
    }
}
