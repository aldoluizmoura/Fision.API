using FIsionAPI.Business.Models.Validacões.Documentos;
using FluentValidation;

namespace FIsionAPI.Business.Models.Validacões;

public class PessoaValidation : AbstractValidator<Pessoa>
{
    public PessoaValidation()
    {
        RuleFor(p => p.CPF)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
            .Must(ValidacaoDocs.Validar).WithMessage("O CPF informado é inválido");

        RuleFor(p => p.Nome).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
        RuleFor(p => p.DataNascimento).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
        RuleFor(p => p.Sexo).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
        RuleFor(p => p.Endereco).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
    }
}
