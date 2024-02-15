using Ocelot.DependencyInjection;
using Ocelot.Middleware;

IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("ocelot.json")
        .Build();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot(configuration);
builder.Services.AddSwaggerForOcelot(configuration);


var app = builder.Build();

app.UseSwagger();
app
    .UseSwaggerForOcelotUI(options => options.PathToSwaggerGenerator = "/swagger/docs")
    .UseOcelot()
    .Wait();

app.Run();
