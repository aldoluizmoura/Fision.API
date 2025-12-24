using AutoMapper;
using FIsionAPI.API.ViewModels;
using FIsionAPI.Business.Models;

namespace FIsionAPI.API.Configurations.AutoMapper;

public class PessoaProfile : Profile
{
    public PessoaProfile()
    {
        CreateMap<PessoaViewModel, Pessoa>().ReverseMap();
        CreateMap<EnderecoPessoaViewModel, EnderecoPessoa>().ReverseMap();
        CreateMap<EntidadeViewModel, Entidade>().ReverseMap();
        CreateMap<EspecialidadeViewModel, Especialidades>().ReverseMap();
    }
}
