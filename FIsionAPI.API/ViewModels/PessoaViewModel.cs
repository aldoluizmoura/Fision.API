using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FIsionAPI.API.ViewModels;

public class PessoaViewModel
{
    [Key]
    public Guid Id { get; set; }
    public string CPF { get; set; }
    public string Nome { get; set; }
    public DateTime DtNascimento { get; set; }
    public string Sexo { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public DateTime DataCadastro { get; set; }
    public int Classe { get; set; }
    public EnderecoPessoaViewModel Endereco { get; set; }
    public IEnumerable<EntidadeViewModel> Entidades { get; set; }
}
