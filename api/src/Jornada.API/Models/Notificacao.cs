namespace Jornada.API.Models
{
    public class Notificacao
    {
        public Guid CorrelationId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Mensagem { get; set; }
    }
}
