﻿using ApiCadastroClientes.Data.Repositories;
using ApiCadastroClientes.Services;
using ApiNotasSimples.Data.Context;
using ApiNotasSimples.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiNotasSimples.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy
            .WithOrigins("http://localhost:4200")
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

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<security>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false 
        };
    });


var app = builder.Build();

// Pipeline de requisição
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();