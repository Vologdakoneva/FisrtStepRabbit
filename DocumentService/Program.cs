using DocumentService.Data;
using DocumentService.Repositories.Entities;
using DocumentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Plain.RabbitMQ;
using RabbitMQ.Client;
using DocumentService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<DocumentDbContext>();

string connString = builder.Configuration.GetConnectionString("RabbitMQ");

builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(connString));
builder.Services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
    "promed-exchange",
    "docs-queue",
    "NaAnaliz.*",
 ExchangeType.Topic));
builder.Services.AddScoped<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
    "promed-exchange",
 ExchangeType.Topic));

builder.Services.AddHostedService<NaAnalizListener>();


builder.Services.AddControllers().AddJsonOptions(x =>
{
    // Serialize enums as strings in api responses (e.g. Gender)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    // Ignore possible object cycles
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<IDocAnaliz, DocAnalizRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var serviceUp = scope.ServiceProvider;

    var context = serviceUp.GetRequiredService<DocumentDbContext>();
    context.Database.Migrate();
}

app.Run();
