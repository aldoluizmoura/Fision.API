using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIsionAPI.Business.Models.Validacões.Documentos
{
    public class EnderecoValidation : AbstractValidator<EnderecoPessoa>
    {
        public EnderecoValidation()
        {
            RuleFor(e => e.Logradouro)
                .NotEmpty().WithMessage("O campo {PropertyName} deve ser fornecido")
                .Length(2,200).WithMessage("O campo {PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres");
                
            RuleFor(e => e.Bairro)
                .NotEmpty().WithMessage("O campo {PropertyName} deve ser fornecido")
                .Length(2, 100).WithMessage("O campo {PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(e => e.Cidade)
                .NotEmpty().WithMessage("O campo {PropertyName} deve ser fornecido")
                .Length(2,100).WithMessage("O campo {PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres"); 

            RuleFor(e => e.Estado)
                .NotEmpty().WithMessage("O campo {PropertyName} deve ser fornecido")
                .Length(2,50).WithMessage("O campo {PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(e => e.Numero)
                .NotEmpty().WithMessage("O Campo {PropertyName} deve ser forncedio")
                .Length(2,50).WithMessage("O campo {PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(e => e.CEP)
                .NotEmpty().WithMessage("O campo {PropertyName} deve ser fornecido")
                .Length(2, 8).WithMessage("O campo {PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
