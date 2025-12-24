using FIsionAPI.Business.Models;
using System;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces;

public interface ICaixaService : IDisposable
{
    Task<bool> Adicionar(Caixa caixa);
    Task<bool> Remover(Caixa caixa);
    Task<bool> FecharCaixa(Caixa caixa);
    Task<bool> ReabrirCaixa(Caixa caixa);
}
