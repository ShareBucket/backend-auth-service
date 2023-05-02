using Microsoft.Extensions.Configuration;
using ShareBucket.AuthenticationService.Authorization;
using ShareBucket.AuthenticationService.GrpcServices;
using ShareBucket.AuthenticationService.Helpers;
using ShareBucket.DataAccessLayer.Data;


var builder = WebApplication.CreateBuilder(args);

var _configuration = builder.Configuration;



builder.Services.AddGrpc();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(_configuration.GetSection("JwtAuthentication"));
builder.Services.AddScoped<IJwtUtils, JwtUtils>();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGrpcService<JwtValidatorGrpcService>();

app.UseHttpsRedirection();

app.UseCors("corsapp");

app.Run();
