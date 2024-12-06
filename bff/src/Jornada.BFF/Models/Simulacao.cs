namespace Jornada.BFF.Models
{
    public class Simulacao
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
    }
}
