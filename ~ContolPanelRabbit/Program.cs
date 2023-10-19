using Microsoft.EntityFrameworkCore;
using PacientService;
using PacientService.Data;
using PacientService.Entities;
using PacientService.Repositories.Entities;
using PacientService.Repositories.Interfaces;
using Plain.RabbitMQ;
using RabbitMQ.Client;
using System.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
