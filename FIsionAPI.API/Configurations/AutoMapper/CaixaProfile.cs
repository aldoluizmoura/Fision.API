using AutoMapper;
using FIsionAPI.API.ViewModels;
using FIsionAPI.Business.Models;

namespace FIsionAPI.API.Configurations.AutoMapper;

public class CaixaProfile : Profile
{
    public CaixaProfile()
    {
        CreateMap<CaixaViewModel, Caixa>().ReverseMap();
    }
}
