using ClassLibrary1;
using Microsoft.AspNetCore.SignalR;
using System.Transactions;


namespace WebApplication1.Hubs
{
    public class ChatHub : Hub
    {
        private FileManager fileManager = new FileManager();
        private TrackList trackList = new TrackList();
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("RecieveMessage", Context.ConnectionId+" Joined"); 
        }

        private void Load()
        {
            trackList = fileManager.LoadFile();
        }
        private void Save()
        {
            fileManager.SaveFile(trackList);
        }

        public void SendAudioBytes(List<byte> bytes, string trackName)
        {
            Load();
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
            Save();
        }
        public void SendCoverBytes(List<byte> bytes, string trackName)
        {
            Load();
            if (!trackList.CheckByName(trackName))
            {
                trackList.AddNewTrack(trackName);
                trackList.AddBytesToExistingTrack(trackName, bytes, false);

            }
            else
            {
                trackList.AddBytesToExistingTrack(trackName, bytes, false);
            }
            Save();
        }
        public async Task GetAudioBytes(string trackName)
        {
            Load();
            await Clients.All.SendAsync("RecieveMessage", "Начинаем получение аудио по названию..." + trackName);
            if(trackList.CheckByName(trackName))
            {
                await Clients.All.SendAsync("RecieveMessage", "Трек найден, загружаем аудио...");
                foreach (var byteChunk in trackList.GetSplittedAudioBytes(trackName))
                {     
                    await Clients.All.SendAsync("RecieveAudioBytes", byteChunk);
                    await Clients.All.SendAsync("RecievingAudioDone", trackList.GetTrackByteCount(trackName, true));
                }
                await Clients.All.SendAsync("RecieveMessage", "Звук загружен");
            }
            else
            {
                await Clients.All.SendAsync("RecieveMessage", "Трек не найден");
            }
        }
        public async Task GetCoverBytes(string trackName)
        {
            Load();
            await Clients.All.SendAsync("RecieveMessage", "Начинаем получение обложки по названию..." + trackName);
            if (trackList.CheckByName(trackName))
            {
                await Clients.All.SendAsync("RecieveMessage", "Трек найден, загружаем обложку...");
                foreach (var byteChunk in trackList.GetSplittedCoverBytes(trackName))
                {
                    await Clients.All.SendAsync("RecieveCoverBytes", byteChunk);
                    await Clients.All.SendAsync("RecievingCoverDone", trackList.GetTrackByteCount(trackName, false));
                }
                await Clients.All.SendAsync("RecieveMessage", "Обложка загружена");
            }
            else
            {
                await Clients.All.SendAsync("RecieveMessage", "Трек не найден");
            }
        }
        public async Task GetCoverForDisplay(int startIndex)
        {
            Load();
            await Clients.All.SendAsync("RecieveMessage", "Начинаем получение обложки для отображения...");
            for (int i = 0; i < trackList.CheckForTrackCount(); i++)
            {
                int byteCount = trackList.GetTrackByIndex(i).TrackCoverBytes.Count;
                foreach (var byteChunk in trackList.GetSplittedDisplayCoverBytes(startIndex, i))
                {
                    await Clients.All.SendAsync("RecieveDisplayCoverBytes",byteChunk);
                    await Clients.All.SendAsync("RecieveDisplayCoverBytesDone", byteCount, i);
                }
                
            }
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("RecieveMessage", Context.ConnectionId + " Says: " + message);
        }

        //Тест сообщения: {"arguments":["Hello!"],"target":"SendMessage","type":1}
    }
}
