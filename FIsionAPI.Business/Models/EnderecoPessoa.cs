﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FIsionAPI.Business.Models
{
    public class EnderecoPessoa : Entity
    {
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }                
        public string Complemento { get; set; }
        public Guid PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }

    }
}
