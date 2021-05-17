using FIsionAPI.Business.Models.Enums;
using System;
using System.Collections.Generic;

namespace FIsionAPI.Business.Models
{
    public class Pessoa : Entity
    {
        public string CPF { get; set; }
        public string Nome { get; set; }
        public DateTime DtNascimento { get; set; }
        public string Sexo { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }        
        public EnderecoPessoa Endereco { get; set; }
        public IEnumerable<Entidade> Entidades { get; set; }


    }
}
