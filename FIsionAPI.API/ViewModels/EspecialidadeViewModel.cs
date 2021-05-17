using FIsionAPI.Business.Models;
using System;
using System.Collections.Generic;

namespace FIsionAPI.API.ViewModels
{
    public class EspecialidadeViewModel
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public IEnumerable<EntidadeViewModel> Entidades { get; set; }
    }
}
