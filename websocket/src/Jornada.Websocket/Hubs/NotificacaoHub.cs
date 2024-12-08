using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Jornada.Websocket.Hubs
{
    public class NotificacaoHub : Hub
    {
        public NotificacaoHub()
        {

        }

        public async Task NewMessage(string user, string message)
        {
            PrintContext("NewMessage", Context);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToCaller(string user, string message)
        {
            PrintContext("SendMessageToCaller", Context);
            await Clients.Caller.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToGroup(string group, string user, string message)
        {
            PrintContext("SendMessageToGroup", Context);
            await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
        }

        public override Task OnConnectedAsync()
        {
            PrintContext("OnConnectedAsync", Context);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            PrintContext("OnDisconnectedAsync", Context);
            return base.OnDisconnectedAsync(exception);
        }

        public void PrintContext(string chamador, HubCallerContext context)
        {

            Console.WriteLine($"ConnectionID {context.ConnectionId}");

            Console.WriteLine($"UserIdentifier {context.UserIdentifier}");
            Debug.WriteLine($"ConnectionID {context.ConnectionId}");

            Debug.WriteLine($"UserIdentifier {context.UserIdentifier}");
            if (context.Items?.Count > 0)
                foreach (var item in context.Items)
                {
                    Console.WriteLine($"Key {item.Key}  Value {item.Value}");
                    Debug.WriteLine($"Key {item.Key}  Value {item.Value}");
                }
        }
    }
}
