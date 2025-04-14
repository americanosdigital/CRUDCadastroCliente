using Microsoft.EntityFrameworkCore;
using CadastroClienteAPP.Application.Interfaces;
using CadastroClienteAPP.Application.Services;
using CadastroClienteAPP.Domain.Interfaces;
using CadastroClienteAPP.Infrastructure.Context;
using CadastroClienteAPP.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// DB
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Cadastro de Documentos API",
        Description = "API para cadastro de documentos para o Teste Prático da Profits Consulting",
        Contact = new OpenApiContact
        {
            Name = "Wellington Americano",
            Email = "americanosdigital@gmail.com"
        }
    });
});

// Add services to the container.

//Criando a configuração do CORS para dar permissão ao projeto Angular
builder.Services.AddCors(
    config => config.AddPolicy("DefaultPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200", "https://localhost:5256")
            .AllowAnyMethod()
            .AllowAnyHeader();
    })
    );

// DI
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddControllers();
builder.Services.AddRouting(config => { config.LowercaseUrls = true; });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use a política de CORS
//app.UseCors("AllowAllOrigins");
app.UseCors("DefaultPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware padrão
app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("DefaultPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
