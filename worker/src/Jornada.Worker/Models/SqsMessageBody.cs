namespace Jornada.Worker.Models
{
    public class SqsMessageBody
    {
        public string Type { get; set; }
        public string MessageId { get; set; }
        public string TopicArn { get; set; }
        public string Message { get; set; }
        public DateTime? Timestamp { get; set; }
        public string UnsubscribeURL { get; set; }
        public string SignatureVersion { get; set; }
        public string Signature { get; set; }
        public string SigningCertURL { get; set; }
    }
    public class SqsMessageBody<T> where T : class
    {
        public string Type { get; set; }
        public string MessageId { get; set; }
        public string TopicArn { get; set; }
        public T Message { get; set; }
        public DateTime? Timestamp { get; set; }
        public string UnsubscribeURL { get; set; }
        public string SignatureVersion { get; set; }
        public string Signature { get; set; }
        public string SigningCertURL { get; set; }
    }

}
