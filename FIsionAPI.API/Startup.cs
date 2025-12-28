using FIsionAPI.API.Authentication.AuthenticationConfig;
using FIsionAPI.API.Authentication.Models;
using FIsionAPI.API.Configurations;
using FIsionAPI.Data.Contexto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;

namespace FIsionAPI.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<FisionContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddDbContext<AuthenticationDbContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("AuthenticationConnection")));

        services.AddAuthorization();

        services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<AuthenticationDbContext>();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "FIsionAPI.API", Version = "v1" });
        });

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        services.AddAutoMapper(typeof(Startup));
        services.AddControllers();        
        services.ResolveDependecies();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FIsionAPI.API v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
