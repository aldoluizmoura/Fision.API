using AutoMapper;
using FIsionAPI.API.ViewModels;
using FIsionAPI.Business.Models;

namespace FIsionAPI.API.Configurations.AutoMapper;

public class PessoaProfile : Profile
{
    public PessoaProfile()
    {
        CreateMap<PessoaViewModel, Pessoa>()
            .ForMember(dest => dest.DataNascimento, opt => opt.MapFrom(src => src.DtNascimento))
            .ReverseMap()
            .ForMember(dest => dest.DtNascimento, opt => opt.MapFrom(src => src.DataNascimento));

        CreateMap<EnderecoPessoaViewModel, EnderecoPessoa>()
            .ForMember(dest => dest.CEP, opt => opt.MapFrom(src => src.Cep))
            .ForMember(dest => dest.PessoaId, opt => opt.MapFrom(src => src.EntidadeId))
            .ReverseMap()
            .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => src.CEP))
            .ForMember(dest => dest.EntidadeId, opt => opt.MapFrom(src => src.PessoaId));

        CreateMap<EntidadeViewModel, Entidade>()
            .ForMember(dest => dest.DataEntrada, opt => opt.MapFrom(src => src.DtEntrada))
            .ForMember(dest => dest.DataSaida, opt => opt.MapFrom(src => src.DtSaida))
            .ReverseMap()
            .ForMember(dest => dest.DtEntrada, opt => opt.MapFrom(src => src.DataEntrada))
            .ForMember(dest => dest.DtSaida, opt => opt.MapFrom(src => src.DataSaida));

        CreateMap<EspecialidadeViewModel, Especialidades>().ReverseMap();
    }
}
