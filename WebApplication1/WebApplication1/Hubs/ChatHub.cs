using Microsoft.AspNetCore.SignalR;


namespace WebApplication1.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.RecieveMessage(Context.ConnectionId+" Joined");
        }

        //Подключение по команде: {"protocol":"json","version":1}

        public async Task SendMessage(string message)
        {
            await Clients.All.RecieveMessage(Context.ConnectionId + " Says: " + message);
        }

        //Тест сообщения: {"arguments":["Hello!"],"target":"SendMessage","type":1}
    }
}
