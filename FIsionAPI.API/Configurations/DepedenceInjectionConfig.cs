using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Notificacões;
using FIsionAPI.Business.Services;
using FIsionAPI.Data.Contexto;
using FIsionAPI.Data.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace FIsionAPI.API.Configurations
{
    public static class DepedenceInjectionConfig
    {
        public static IServiceCollection ResolveDependecies(this IServiceCollection service)
        {
            service.AddScoped<FisionContext>();
            service.AddScoped<IPessoaRepositoy, PessoaRepository>();
            service.AddScoped<IEntidadeRepository, EntidadeRepository>();
            service.AddScoped<EspecialidadeRepository, EspecialidadeRepository>();
            service.AddScoped<IEnderecoPessoaRepository, EnderecoPessoaRepository>();
            service.AddScoped<IContratoFinanceiroRepository, ContratoFinanceiroRepository>();
            service.AddScoped<IMovimentoFinanceiroRepository, MovimentoFinanceiroRepository>();
            service.AddScoped<ICaixaRepository, CaixaRepository>();
            service.AddScoped<ICaixaService, CaixaService>();
            service.AddScoped<IMovimentoFinanceiroAvulsoRepository, MovimentoFinanceiroAvulsoRepository>();

            service.AddScoped<IEntidadeService, EntidadeService>();
            service.AddScoped<IMovimentoFinanceiroService, MovimentoFinanceiroService>();
            service.AddScoped<IMovimentoFinanceiroAvulsoService, MovimentoFinanceiroAvulsoService>();

            service.AddScoped<INotificador, Notificador>();

            return service;
        }
    }
}
