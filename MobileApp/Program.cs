using Microsoft.EntityFrameworkCore;
using MobileApp.Data;
using MobileApp.EntitiesRealiz;
using MobileApp.Iterfaces;
using ServiceTask;
using System.ServiceModel;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MobileDbContext>(); 
builder.Services.AddScoped<IUsers, UserEntities>();


builder.Services.AddControllers().AddJsonOptions(x =>
{
    // Serialize enums as strings in api responses (e.g. Gender)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    // Ignore possible object cycles
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<TaskPortType>(provider => {
    var client = new TaskPortTypeClient(TaskPortTypeClient.EndpointConfiguration.TaskSoap);

    // Use configuration object to read it from appconfig.json
    client.ClientCredentials.UserName.UserName = "IIS_USER";
    client.ClientCredentials.UserName.Password = "457970";
    client.Endpoint.Address = new EndpointAddress("http://192.168.3.150:8080/ElPol_report_2/WS/Task/1cws");

    return client;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var serviceUp = scope.ServiceProvider;

    var context = serviceUp.GetRequiredService<MobileDbContext>();
    context.Database.Migrate();
}


app.Run();
