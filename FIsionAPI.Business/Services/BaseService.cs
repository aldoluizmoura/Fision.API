using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Business.Notificacões;
using FluentValidation;
using FluentValidation.Results;

namespace FIsionAPI.Business.Services
{
    public class BaseService
    {
        private readonly INotificador _notificador;

        public BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }
        protected void Notificar(string Mensagem)
        {
            _notificador.Handle(new Notificacao(Mensagem));
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notificar(validator);

            return false;
        }
        protected bool VerificarCondicoesQuitacao(MovimentoFinanceiroEntidade movimento)
        {
            if (movimento.Situacao == Models.Enums.SituacaoMovimento.Quitado)
            {
                Notificar("O Movimento já está quitado!");
                return false;
            }
            if (movimento.Classe == Models.Enums.ClasseMovimento.Outros)
            {
                Notificar("A classe do movimento não pode ser Outros");
                return false;
            }
            if (movimento == null)
            {
                Notificar("Movimento não encontrado!");
                return false;
            }
            else
            {
                return true;
            }
        }
        protected bool VerificarCondicoesDesquitacao(MovimentoFinanceiroEntidade movimento)
        {
            if (movimento.Situacao == Models.Enums.SituacaoMovimento.Aberto)
            {
                Notificar("O Movimento já está aberto!");
                return false;
            }
            if (movimento.Classe == Models.Enums.ClasseMovimento.Outros)
            {
                Notificar("A classe do movimento não pode ser Outros");
                return false;
            }
            if (movimento == null)
            {
                Notificar("Movimento não encontrado!");
                return false;
            }
            else
            {
                return true;
            }
        }
        protected bool VerificarCondicoesExclusao(MovimentoFinanceiroEntidade movimento)
        {
            if (movimento.Classe != Models.Enums.ClasseMovimento.Outros)
            {
                if (movimento.Situacao == Models.Enums.SituacaoMovimento.Quitado)
                {
                    Notificar("Não é possivel excluir um movimento já quitado!");
                    return false;
                }
                else return true;
            }
            return true;
        }
        protected decimal? VerificarClasseValorReceber(MovimentoFinanceiroEntidade movimento, ContratoFinanceiro contrato)
        {
            if (movimento.Classe == Models.Enums.ClasseMovimento.Aluno)
            {
               return movimento.ValorReceber = (contrato.ValorMensal) - (contrato.ValorMensal * (contrato.Desconto) / 100);               
            }
            else
            {
               return movimento.ValorReceber = ((contrato.ValorUnitario * contrato.Quantidade) * (contrato.MargemLucro) / 100);                 
            }            
        }
    }
}
