using ClassLibrary1;
using Microsoft.AspNetCore.SignalR;


namespace WebApplication1.Hubs
{
    public class ChatHub : Hub
    {
        private TrackList trackList = new TrackList();
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("RecieveMessage", Context.ConnectionId+" Joined"); 
        }

        public void SendAudioBytes(List<byte> bytes, string trackName)
        {
            if(!trackList.CheckByName(trackName))
            {
                Clients.All.SendAsync("RecieveMessage", "Такого трека нет, создаём");
                trackList.AddNewTrack(trackName);
                trackList.AddBytesToExistingTrack(trackName, bytes, true);
            }
            else
            {
                Clients.All.SendAsync("RecieveMessage", "Трек с названием " + trackName + " уже существует");
                trackList.AddBytesToExistingTrack(trackName, bytes, true);
            }
            Clients.All.SendAsync("RecieveMessage", "Получено " + bytes.Count() + " байт звука");
        }
        public void SendCoverBytes(List<byte> bytes, string trackName)
        {
            if (!trackList.CheckByName(trackName))
            {
                trackList.AddNewTrack(trackName);
                trackList.AddBytesToExistingTrack(trackName, bytes, false);
            }
            else
            {
                trackList.AddBytesToExistingTrack(trackName, bytes, false);
            }
            Clients.All.SendAsync("RecieveMessage", "Получено " + bytes.Count() + " байт обложки");
        }
        public async Task GetAudioBytes(string trackName)
        {
            await Clients.All.SendAsync("RecieveMessage", "Начинаем получение по названию " + trackName);
            if(trackList.CheckByName(trackName))
            {
                foreach (var byteChunk in trackList.GetSplittedAudioBytes(trackName))
                {
                    await Clients.All.SendAsync("RecieveMessage", "Отправлено " + byteChunk.Count + " байт звука");
                    await Clients.Caller.SendAsync("RecieveAudioBytes", byteChunk);
                }
            }
            else
            {
                await Clients.All.SendAsync("RecieveMessage", "Трек не найден");
            }
        }
        public async Task GetCoverBytes(string trackName)
        {
            foreach (var byteChunk in trackList.GetSplittedCoverBytes(trackName))
            {
                await Clients.Caller.SendAsync("RecieveCoverBytes", byteChunk);
                await Clients.All.SendAsync("RecieveMessage", "Отправлено " + byteChunk.Count + " байт обложки");
            }
        }

        public async Task CreateTrack(string name)
        {
            trackList.AddNewTrack(name);
            await Clients.All.SendAsync("RecieveMessage", "Создан трек " + name);
        }

        public async Task CheckTrack(string name)
        {
            if (trackList.CheckByName(name))
            {
                await Clients.All.SendAsync("RecieveMessage", "Трек с названием " + name + " уже был создан");
            }
            else
            {
                await Clients.All.SendAsync("RecieveMessage", "Трек с названием " + name + " не существует");
            }
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("RecieveMessage", Context.ConnectionId + " Says: " + message);
        }

        //Тест сообщения: {"arguments":["Hello!"],"target":"SendMessage","type":1}
    }
}
