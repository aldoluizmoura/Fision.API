﻿using AutoMapper;
using FIsionAPI.API.ViewModels;
using FIsionAPI.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.API.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {            
            CreateMap<PessoaViewModel, Pessoa>().ReverseMap();
            CreateMap<EnderecoPessoaViewModel, EnderecoPessoa>().ReverseMap();
            CreateMap<ContratoFinanceiroViewModel, ContratoFinanceiro>().ReverseMap();
            CreateMap<EntidadeViewModel, Entidade>().ReverseMap();
            CreateMap<EspecialidadeViewModel, Especialidades>().ReverseMap();
            CreateMap<MovimentoFinanceiroViewModel, MovimentoFinanceiroEntidade>().ReverseMap();
            CreateMap<MovimentoFinanceiroAvulsoViewModel, MovimentoFinanceiroAvulso>().ReverseMap();
            CreateMap<CaixaViewModel, Caixa>().ReverseMap();            
        }
    }
}
