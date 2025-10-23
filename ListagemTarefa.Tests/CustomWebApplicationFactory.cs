using ListagemTarefa.Infraestructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ListagemTarefa.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IDbContextOptionsConfiguration<ListagemTarefaContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<ListagemTarefaContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
        });
    }
}
