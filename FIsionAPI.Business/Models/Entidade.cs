using FIsionAPI.Business.Models.Enums;
using System;
using System.Collections.Generic;

namespace FIsionAPI.Business.Models;

public class Entidade : Entity
{
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public string Matricula { get; set; }
    public Pessoa Pessoa { get; set; }
    public Guid PessoaId { get; set; }
    public ClasseEntidade Classe { get; set; }
    public virtual ContratoFinanceiro Contrato { get; set; }
    public IEnumerable<Especialidades>? Especialidades { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public DateTime DataCadastro { get; set; }

}
