using ApiCadastroClientes.Data.Repositories;
using ApiCadastroClientes.Services;
using ApiNotasSimples.Data.Context;
using ApiNotasSimples.Services;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy
            .WithOrigins("https://localhost:7135/")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Adiciona serviços ao container
builder.Services.AddControllers();
builder.Services.AddSingleton<SqlServerContext>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<CryptoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug));

// Swagger
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "API-CadastroCliente", Version = "v1" })
);

var app = builder.Build();

// Pipeline de requisição
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();