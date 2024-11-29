using Amazon.SQS;
using Jornada.Worker;
using Jornada.Worker.Interfaces;
using Jornada.Worker.Models;
using Jornada.Worker.Servicos;
using LocalStack.Client.Extensions;

var builder = Host.CreateApplicationBuilder(args);



builder.Services.AddLocalStack(builder.Configuration);
// Add services to the container.

var options = builder.Configuration.GetAWSOptions();
options.Region = Amazon.RegionEndpoint.SAEast1;
options.DefaultClientConfig.RegionEndpoint = Amazon.RegionEndpoint.SAEast1;

options.DefaultClientConfig.ServiceURL = $"http://{builder.Configuration.GetValue<string>("LocalStack:Config:LocalStackHost")}:{builder.Configuration.GetValue<string>("LocalStack:Config:EdgePort")}";

options.DefaultClientConfig.AuthenticationRegion = builder.Configuration.GetValue<string>("AWS:Region");
options.DefaultClientConfig.UseHttp = true;
var sqsURL = builder.Configuration.GetValue<string>("sqs:queueUrl");

builder.Services.AddDefaultAWSOptions(options);
builder.Services.AddAWSService<IAmazonSQS>(options);
builder.Services.AddTransient<IProcessarNotificacaoService, ProcessarNotificacaoService>();
builder.Services.AddTransient<IRetornoService, RetornoService>();
builder.Services.AddAWSMessageBus(b =>
 {
     b.ConfigureSerializationOptions(opt =>
     {
         opt.SystemTextJsonOptions = new System.Text.Json.JsonSerializerOptions
         {
             PropertyNameCaseInsensitive = true,
             DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
         };
     });

     // Register an SQS Queue that the framework will poll for messages.
     // NOTE: The URL given below is an example. Use the appropriate URL for your SQS Queue.
     b.AddSQSPoller(sqsURL, opt =>
     {
         opt.MaxNumberOfConcurrentMessages = 10;
         opt.VisibilityTimeout = 60;
         opt.WaitTimeSeconds = 10;
     });

     // Register all IMessageHandler implementations with the message type they should process. 
     // Here messages that match our ChatMessage .NET type will be handled by our ChatMessageHandler
     b.AddMessageHandler<NotificacaoMessageHandler, NotificacaoModel>(nameof(NotificacaoModel));
 });


builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
