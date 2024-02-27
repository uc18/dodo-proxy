using LousBot.Options;
using LousBot.Service;
using LousBot.Service.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<ServiceDeskOptions>(builder.Configuration.GetSection(nameof(ServiceDeskOptions)));
builder.Services.AddScoped<IPyrusService, PyrusService>();
builder.Services.AddScoped<ILoopService, LoopService>();
builder.Services.AddSingleton<IHttpService, HttpService>();
builder.Services.AddSingleton<IMattermostService, MattermostService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapControllers();

app.Run();