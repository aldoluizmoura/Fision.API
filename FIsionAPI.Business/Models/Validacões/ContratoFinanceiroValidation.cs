using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIsionAPI.Business.Models.Validacões
{
    public class ContratoFinanceiroValidation : AbstractValidator<ContratoFinanceiro>
    {
        public ContratoFinanceiroValidation()
        {
          
            RuleFor(c=>c.Tipo)
                .NotEmpty().WithMessage("O Campo {PropertyName} é Obrigatório");
        }
    }
}
