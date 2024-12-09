namespace Jornada.Websocket.Models
{
    public class Usuario
    {
        public Usuario()
        {
                
        }
        public Usuario(string id, string connection)
        {
            Id = Guid.Parse(id);
            ConnectionId = connection;
        }
        public Guid Id { get; set; }
        public string ConnectionId { get; set; }
    }
}
