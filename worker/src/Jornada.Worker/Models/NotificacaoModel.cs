namespace Jornada.Worker.Models
{
    public class NotificacaoModel 
    {
        public Guid CorrelationId { get; set; }
        public string Usuario { get; set; }
        public string Mensagem { get; set; }
    }
}