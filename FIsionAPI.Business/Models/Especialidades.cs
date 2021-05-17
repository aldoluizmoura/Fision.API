using System;
using System.Collections.Generic;
using System.Text;

namespace FIsionAPI.Business.Models
{
    public class Especialidades : Entity
    {
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public IEnumerable<Entidade> Entidades { get; set; }
    }
}
