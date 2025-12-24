using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FIsionAPI.API.ViewModels;

public class EntidadeViewModel
{
    [Key]
    public Guid Id { get; set; }
    public DateTime DtEntrada { get; set; }
    public DateTime DtSaida { get; set; }
    public string Matricula { get; set; }
    public PessoaViewModel Pessoa { get; set; }
    public Guid PessoaId { get; set; }
    public ContratoFinanceiroViewModel Contrato { get; set; }
    public DateTime DataCadastro { get; set; }
    public int Classe { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public IEnumerable<EspecialidadeViewModel>? Especialidades { get; set; }
}
