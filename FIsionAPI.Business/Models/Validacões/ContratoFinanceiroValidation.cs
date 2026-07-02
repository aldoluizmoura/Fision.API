using FIsionAPI.Business.Models.Enums;
using FluentValidation;

namespace FIsionAPI.Business.Models.Validacões;

public class ContratoFinanceiroValidation : AbstractValidator<ContratoFinanceiro>
{
    public ContratoFinanceiroValidation()
    {
        RuleFor(c => c.TipoContrato)
            .IsInEnum().WithMessage("O campo {PropertyName} é inválido");

        RuleFor(c => c.Vencimento)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        When(c => c.TipoContrato == TipoContrato.ContratoAluno, () =>
        {
            RuleFor(c => c.ValorMensal)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório para contrato de aluno")
                .GreaterThan(0).WithMessage("O campo {PropertyName} deve ser maior que zero");
        });

        When(c => c.TipoContrato == TipoContrato.ContratoProfissional, () =>
        {
            RuleFor(c => c.ValorUnitario)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório para contrato de profissional")
                .GreaterThan(0).WithMessage("O campo {PropertyName} deve ser maior que zero");

            RuleFor(c => c.MargemLucro)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório para contrato de profissional");

            RuleFor(c => c.Quantidade)
                .GreaterThan(0).WithMessage("O campo {PropertyName} deve ser maior que zero");
        });
    }
}
