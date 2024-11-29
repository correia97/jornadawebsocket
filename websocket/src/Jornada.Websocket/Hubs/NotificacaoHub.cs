using Microsoft.AspNetCore.SignalR;

namespace Jornada.Websocket.Hubs
{
    public class NotificacaoHub : Hub
    {
        public NotificacaoHub()
        {
                
        }

        public async Task NewMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToCaller(string user, string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToGroup(string group, string user, string message)
        {
            await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
        }
    }
}
