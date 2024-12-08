using Amazon.SQS;
using Jornada.Worker.Interfaces;


namespace Jornada.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAmazonSQS _sqsCliente;
    private readonly IProcessarNotificacaoServico _notificacaoService;

    public Worker(ILogger<Worker> logger, IAmazonSQS sqsCliente, IConfiguration configuration, IProcessarNotificacaoServico notificacaoService)
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
