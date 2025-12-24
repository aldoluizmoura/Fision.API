using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIsionAPI.Business.Models.Validacões
{
    public class PessoaValidation : AbstractValidator<Pessoa>
    {
        public PessoaValidation()
        {
            RuleFor(p => p.CPF).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            RuleFor(p => p.Nome).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            RuleFor(p => p.DataNascimento).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");            
            RuleFor(p => p.Sexo).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            RuleFor(p => p.Endereco).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");        }
    }
}
