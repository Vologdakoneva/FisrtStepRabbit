//using Microsoft.EntityFrameworkCore;
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

// update migration
//dotnet ef migrations add InitialCreate
//dotnet ef database update
//
//sudo apt install pgadmin3 -y
//sudo -u postgres psql
//
//  repository [docker.io/vologdakoneva/pacientservice]


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PacientDbContext>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    // Serialize enums as strings in api responses (e.g. Gender)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    // Ignore possible object cycles
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var services = builder.Services;
builder.Services.AddScoped<ISetups, SetupsRepository>();
builder.Services.AddScoped<IPerson, PersonsRepository>();
builder.Services.AddScoped<IErrorPerson, ErrorPersonRepository>();

string connString = builder.Configuration.GetConnectionString("RabbitMQ");

builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(connString));
builder.Services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
    "promed-exchange",
    "pacient-queue",
    "pacientkey.*",
 ExchangeType.Topic));
builder.Services.AddScoped<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
    "promed-exchange",
 ExchangeType.Topic));

builder.Services.AddHostedService<PacientListener>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();

//app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var serviceUp = scope.ServiceProvider;

    var context = serviceUp.GetRequiredService<PacientDbContext>();
    context.Database.Migrate();
}

app.Run();
