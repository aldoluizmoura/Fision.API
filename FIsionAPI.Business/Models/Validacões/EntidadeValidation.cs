using FluentValidation;

namespace FIsionAPI.Business.Models.Validacões
{
    public class EntidadeValidation : AbstractValidator<Entidade>
    {
        public EntidadeValidation()
        {
            RuleFor(e => e.Classe)
                .NotEmpty().WithMessage("É necessário informar {PropertyName}");

            RuleFor(e=>e.Contrato)
                .NotEmpty().WithMessage("É necessário informar {PropertyName}");

            RuleFor(e => e.Pessoa)
                .NotEmpty().WithMessage("É necessário informar {PropertyName}");
        }
    }
}
