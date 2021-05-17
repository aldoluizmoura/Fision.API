using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using System;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Services
{
    public class MovimentoFinanceiroAvulsoService : BaseService, IMovimentoFinanceiroAvulsoService
    {
        private readonly IMovimentoFinanceiroAvulsoRepository _movimentoRepository;
        public MovimentoFinanceiroAvulsoService(INotificador notificador, IMovimentoFinanceiroAvulsoRepository movimentoRepository) : base(notificador)
        {
            _movimentoRepository = movimentoRepository;
        }

        public async Task<bool> AdicionarMovimentoFinanceiroAvulso(MovimentoFinanceiroAvulso movimentoAvulso)
        {
            if (String.IsNullOrEmpty(movimentoAvulso.CompetenciaMensalidade) || String.IsNullOrEmpty(movimentoAvulso.Descricao))
            {
                Notificar("Campos obrigatórios não informados");
                return false;
            }

            if (movimentoAvulso.Classe != Models.Enums.ClasseMovimento.Outros)
            {
                Notificar("A classe do movimento financeiro deve ser igual a 3");
                return false;
            }

            movimentoAvulso.DataCadastro = DateTime.Now;
            movimentoAvulso.Situacao = Models.Enums.SituacaoMovimento.Aberto;
            
            await _movimentoRepository.Adicionar(movimentoAvulso);
            return true;
        }

        public Task<bool> AtualizarMovimentoFinanceiroAvulso(MovimentoFinanceiroAvulso movimento)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
