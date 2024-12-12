using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Jornada.API.Interfaces;
using Jornada.API.Models;
using Jornada.API.Servicos;
using LocalStack.Client.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Services.AddLocalStack(builder.Configuration);
// Add services to the container.

var options = builder.Configuration.GetAWSOptions();
options.Region = Amazon.RegionEndpoint.SAEast1;
options.DefaultClientConfig.RegionEndpoint = Amazon.RegionEndpoint.SAEast1;

options.DefaultClientConfig.ServiceURL = $"http://{builder.Configuration.GetValue<string>("LocalStack:Config:LocalStackHost")}:{builder.Configuration.GetValue<string>("LocalStack:Config:EdgePort")}";

options.DefaultClientConfig.AuthenticationRegion = builder.Configuration.GetValue<string>("AWS:Region");
var snsARN = builder.Configuration.GetValue<string>("sns:arn");
options.DefaultClientConfig.UseHttp = true;

builder.Services.AddDefaultAWSOptions(options);
builder.Services.AddAWSService<IAmazonSQS>(options);
builder.Services.AddAWSService<IAmazonSimpleNotificationService>(options);
builder.Services.AddScoped<INotificarServico, NotificarServico>();

builder.Services.AddAWSMessageBus(b =>
{
    b.ConfigureSerializationOptions(opt =>
    {
        opt.SystemTextJsonOptions = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    });
    b.AddSNSPublisher<Notificacao>(snsARN, nameof(Notificacao));

});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
