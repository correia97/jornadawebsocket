using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.SQS;
using Amazon.SQS.Model;
using AWS.Messaging;
using Jornada.Worker.Interfaces;
using Jornada.Worker.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;


namespace Jornada.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAmazonSQS _sqsCliente;
    private readonly IProcessarNotificacaoService _notificacaoService;

    public Worker(ILogger<Worker> logger, IAmazonSQS sqsCliente, IConfiguration configuration, IProcessarNotificacaoService notificacaoService)
    {
        _sqsCliente = sqsCliente;
        _configuration = configuration;
        _notificacaoService = notificacaoService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //var opt = new System.Text.Json.JsonSerializerOptions
        //{
        //    PropertyNameCaseInsensitive = true,
        //    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,

        //};
        while (!stoppingToken.IsCancellationRequested)
        {
            //    var result = await _sqsCliente.ReceiveMessageAsync(new ReceiveMessageRequest()
            //    {
            //        QueueUrl = _configuration.GetValue<string>("sqs:queueUrl"),
            //        MaxNumberOfMessages = 10,
            //        WaitTimeSeconds = 10

            //    }, stoppingToken);

            //    foreach (var message in result.Messages)
            //    {
            //        try
            //        {
            //            var mess = JsonSerializer.Deserialize<SqsMessageBody>(message.Body, opt);
            //            await _notificacaoService.Processar(JsonSerializer.Deserialize<NotificacaoModel>(mess.Message));

            //        }
            //        catch (Exception ex) 
            //        {

            //            throw;
            //        }
        }
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        }
        await Task.Delay(1000, stoppingToken);
    }
}
