using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using System;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Services;

public class MovimentoFinanceiroAvulsoService : BaseService, IMovimentoFinanceiroAvulsoService
{
    private readonly IMovimentoFinanceiroAvulsoRepository _movimentoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MovimentoFinanceiroAvulsoService(INotificador notificador,
                                            IMovimentoFinanceiroAvulsoRepository movimentoRepository,
                                            IUnitOfWork unitOfWork) : base(notificador)
    {
        _movimentoRepository = movimentoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AdicionarMovimentoFinanceiroAvulso(MovimentoFinanceiroAvulso movimentoAvulso)
    {
        if (!movimentoAvulso.ValidarMovimentoFinanceiro(out string erro))
        {
            Notificar(erro);
            return false;
        }

        if (movimentoAvulso.Classe != Models.Enums.ClasseMovimento.Outros)
        {
            Notificar("Classe de movimento inválida para lançamento avulso");
            return false;
        }

        movimentoAvulso.DataCadastro = DateTime.Now;
        movimentoAvulso.Situacao = Models.Enums.SituacaoMovimento.Aberto;
        
        await _movimentoRepository.Adicionar(movimentoAvulso);
        await _unitOfWork.Commit();

        return true;
    }

    public async Task<bool> AtualizarMovimentoFinanceiroAvulso(MovimentoFinanceiroAvulso movimento)
    {
        if (!movimento.ValidarMovimentoFinanceiro(out string erro))
        {
            Notificar(erro);
            return false;
        }

        if (movimento.Classe != ClasseMovimento.Outros)
        {
            Notificar("A classe do movimento financeiro deve ser igual a Outros.");
            return false;
        }

        if (movimento.Situacao == SituacaoMovimento.Quitado)
        {
            Notificar("Movimentos quitados não podem ser alterados");
            return false;
        }

        await _movimentoRepository.Atualizar(movimento);
        await _unitOfWork.Commit();
        return true;
    }    

    public void Dispose()
    {
        _movimentoRepository.Dispose();
        _unitOfWork.Dispose();
    }
}
