﻿using System.ComponentModel.DataAnnotations;

namespace ApiNotasSimples.Models
{
    public class UsuarioModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
       public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }

        // Campo que define o tipo de usuário
        public string Role { get; set; } = "user"; // padrão é 'user'

        public bool Record_status { get; set; } = true;

        public UsuarioModel (){}



    }
}