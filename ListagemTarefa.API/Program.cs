
using ListagemTarefa.Application.Interfaces;
using ListagemTarefa.Application.Servicos;
using ListagemTarefa.Domain.Repositorios;
using ListagemTarefa.Infraestructure.Data;
using ListagemTarefa.Infraestructure.Repositorios;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers()
            .AddJsonOptions(config => config.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ListagemTarefaContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("PermitirTudo",
                policy => policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());
        });
        //builder.Services.AddScoped<IJsonPlaceholderService, JsonPlaceholderService>();
        builder.Services.AddScoped<ITarefaServico, TarefaServico>();
        builder.Services.AddScoped<ITarefaRepositorio, TarefaRepositorio>();



        builder.Services.AddHttpClient<IJsonPlaceholderServico, JsonPlaceholderService>(config =>
        {
            config.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ListagemTarefaContext>();
            if (db.Database.IsRelational())
            {
                db.Database.Migrate();
            }
            else
            {
                db.Database.EnsureCreated();
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("PermitirTudo");

        var fileProvider = new FileExtensionContentTypeProvider();
        fileProvider.Mappings[".properties"] = "text/plain";
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = fileProvider
        });

        app.UseAuthorization();

        
        app.MapControllers();

        app.Run();
    }
}