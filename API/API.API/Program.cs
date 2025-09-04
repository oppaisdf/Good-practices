using API.Application.Services.Contracts;
using API.Application.Services.Create;
using API.Application.Services.Delete;
using API.Application.Services.GetById;
using API.Infrastructure.Persistence;
using API.Infrastructure.Persistence.Interceptors;
using API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración (SQLite por defecto; override con env: ConnectionStrings__Default)
var connString = builder.Configuration.GetConnectionString("Default") ?? "Data Source=Data/app.db";

// Logging
builder.Services.AddLogging(cfg => cfg.AddSimpleConsole());

// Infraestructura
builder.Services.AddSingleton<AuditSaveChangesInterceptor>();
builder.Services.AddDbContext<Context>(opt =>
{
    opt.UseSqlite(connString);
    opt.EnableSensitiveDataLogging(false);
});

// Repos
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

// Handlers (use-case handlers explícitos; sin mediator)
builder.Services.AddScoped<CreateServiceHandler>();
builder.Services.AddScoped<GetServiceByIdHandler>();
builder.Services.AddScoped<DeleteServiceHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
