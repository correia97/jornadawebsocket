using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Jornada.API.Interfaces;
using Jornada.API.Servicos;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

var options = new AWSOptions
{
    Credentials = new BasicAWSCredentials("teste", "teste"),
    Region = Amazon.RegionEndpoint.SAEast1,

};
options.DefaultClientConfig.ServiceURL = "http://localhost:4566";

builder.Services.AddAWSService<IAmazonSimpleNotificationService>(options);
builder.Services.AddDefaultAWSOptions(options);
builder.Services.AddScoped<INotificarService, NotificarService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
