using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using AWS.Messaging;
using Jornada.API.Interfaces;
using Jornada.API.Models;
using System.Diagnostics;
using System.Text.Json;

namespace Jornada.API.Servicos
{
    public class NotificarServico : INotificarServico
    {
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly IConfiguration _configuration;
        private readonly IAmazonSQS _sqsCliente;
        private readonly IMessagePublisher _messagePublisher;

        public NotificarServico(IAmazonSimpleNotificationService snsClient, IAmazonSQS sqsCliente, IMessagePublisher messagePublisher, IConfiguration configuration)
        {
            _snsClient = snsClient;
            _sqsCliente = sqsCliente;
            _configuration = configuration;
            _messagePublisher = messagePublisher;
        }

        public async Task Setup()
        {
            try
            {
                int maxMessage = 256 * 1024;
                var deadLetterQueueAttributes = new Dictionary<string, string>
                {
                    {
                        QueueAttributeName.MaximumMessageSize,
                        maxMessage.ToString()
                    }
                };

                var createDeadletterQueueRequest = new CreateQueueRequest()
                {
                    QueueName = "websocket-queue-deadLetter",
                    Attributes = deadLetterQueueAttributes
                };
                var createDeadLetterResponse = await _sqsCliente.CreateQueueAsync(createDeadletterQueueRequest);
                var deadLetterArn = await GetQueueArnByUrl(createDeadLetterResponse.QueueUrl);

                var p = new { deadLetterTargetArn = deadLetterArn, maxReceiveCount = 3 };

                var queueAttributes = new Dictionary<string, string>
                {
                    {
                        QueueAttributeName.MaximumMessageSize,
                        maxMessage.ToString()
                    },
                    {
                        QueueAttributeName.RedrivePolicy,
                        JsonSerializer.Serialize(p)
                    }
               };

                var createQueueRequest = new CreateQueueRequest()
                {
                    QueueName = "websocket-queue",
                    Attributes = queueAttributes
                };
                var createResponse = await _sqsCliente.CreateQueueAsync(createQueueRequest);

                var queueArn = await GetQueueArnByUrl(createDeadLetterResponse.QueueUrl);
                var request = new CreateTopicRequest
                {
                    Name = "websocket-topic",
                };

                var responseTopic = await _snsClient.CreateTopicAsync(request);


                var requestSubscribe = new SubscribeRequest()
                {
                    TopicArn = responseTopic.TopicArn,
                    ReturnSubscriptionArn = true,
                    Protocol = "sqs",
                    Endpoint = queueArn,
                    Attributes = new Dictionary<string, string>
                {
                    { "RawMessageDelivery","true" }
                }
                };

                var response = await _snsClient.SubscribeAsync(requestSubscribe);
            }
            catch (Exception ex)
            {

                throw;
            }



        }

        public async Task<string> GetQueueArnByUrl(string queueUrl)
        {
            var getAttributesRequest = new GetQueueAttributesRequest()
            {
                QueueUrl = queueUrl,
                AttributeNames = new List<string>() { QueueAttributeName.QueueArn }
            };

            var getAttributesResponse = await _sqsCliente.GetQueueAttributesAsync(
                getAttributesRequest);

            return getAttributesResponse.QueueARN;
        }

        public async Task PublishToTopicAsync(Notificacao notificacao)
        {
            try
            {
                var response = await _messagePublisher.PublishAsync(notificacao);

                Console.WriteLine($"Successfully published message ID: {response.MessageId}");
            }
            catch (Amazon.SimpleNotificationService.Model.InvalidParameterException ex)
            {

                Debug.WriteLine(ex);
                Console.WriteLine(ex);
                Debug.WriteLine(ex.InnerException);
                Console.WriteLine(ex.InnerException);
                if (ex.InnerException is Amazon.Runtime.Internal.HttpErrorResponseException)
                {
                    var response = ((Amazon.Runtime.Internal.HttpErrorResponseException)ex.InnerException);
                    Console.WriteLine(response.Message);
                    Debug.WriteLine(response.Message);
                }

                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
