using AutoMapper;
using FIsionAPI.API.ViewModels;
using FIsionAPI.Business.Models;

namespace FIsionAPI.API.Configurations.AutoMapper;

public class FinanceiroProfile : Profile
{
    public FinanceiroProfile()
    {
        CreateMap<ContratoFinanceiroViewModel, ContratoFinanceiro>().ReverseMap();
        CreateMap<MovimentoFinanceiroViewModel, MovimentoFinanceiroEntidade>().ReverseMap();
        CreateMap<MovimentoFinanceiroAvulsoViewModel, MovimentoFinanceiroAvulso>().ReverseMap();
        CreateMap<CaixaViewModel, Caixa>().ReverseMap();
    }
}
