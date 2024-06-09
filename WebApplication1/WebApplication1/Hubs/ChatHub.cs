using Microsoft.AspNetCore.SignalR;


namespace WebApplication1.Hubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("RecieveMessage", Context.ConnectionId+" Joined");
        }

        public async Task SendBytes(List<byte> bytes)
        {
            await Clients.All.SendAsync("RecieveBytes", bytes);
        }
        //Подключение по команде: {"protocol":"json","version":1}

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("RecieveMessage", Context.ConnectionId + " Says: " + message);
        }

        //Тест сообщения: {"arguments":["Hello!"],"target":"SendMessage","type":1}
    }
}
