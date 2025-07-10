using System.ComponentModel.DataAnnotations;

namespace ApiCadastroClientes.Models.DTO
{
    public class UsuarioDTO
    {
        [MinLength(4)]
        public string Nome { get; set; }
        public string Email { get; set; }
        [MinLength(4)]
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        [MinLength(4)]
        public string Endereco { get; set; }

    }
}