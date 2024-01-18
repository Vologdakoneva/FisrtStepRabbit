using Plain.RabbitMQ;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();

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

builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.PropertyNamingPolicy = null);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();





//app.MapRazorPages();


app.UseEndpoints(endpoints => {
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllers();
});
app.MapControllers();
app.Run();
