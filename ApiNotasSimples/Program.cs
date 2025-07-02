using ApiNotasSimples.Data.Context;
using ApiNotasSimples.Data.Repositories;
using ApiNotasSimples.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Serviços da aplicação
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<SqlServerContext>();
builder.Services.AddScoped<NotaRepository>();
builder.Services.AddScoped<NotaServices>();

// Swagger simples (sem JWT)
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "API Notas", Version = "v1" });
});

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();


