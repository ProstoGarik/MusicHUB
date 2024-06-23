using ClassLibrary1;
using Microsoft.AspNetCore.SignalR;
using System.Transactions;


namespace WebApplication1.Hubs
{
    public class ChatHub : Hub
    {
        private FileManager fileManager = new FileManager();
        private TrackList trackList = new TrackList();

        private void Load()
        {
            trackList = fileManager.LoadFile();
        }
        private void Save()
        {
            fileManager.SaveFile(trackList);
        }

        public void SendAudioBytes(List<byte> bytes, string trackName, string trackArtist)
        {
            Load();
            if(!trackList.CheckByName(trackName))
            {
                trackList.AddNewTrack(trackName, trackArtist);
                trackList.AddBytesToExistingTrack(trackName, bytes, true);
                
            }
            else
            {
                trackList.AddBytesToExistingTrack(trackName, bytes, true);
            }
            Save();
        }
        public void SendCoverBytes(List<byte> bytes, string trackName, string trackArtist)
        {
            Load();
            if (!trackList.CheckByName(trackName))
            {
                trackList.AddNewTrack(trackName, trackArtist);
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
            if(trackList.CheckByName(trackName))
            {
                foreach (var byteChunk in trackList.GetSplittedAudioBytes(trackName))
                {     
                    await Clients.All.SendAsync("RecieveAudioBytes", byteChunk);
                    await Clients.All.SendAsync("RecievingAudioDone", trackList.GetTrackByteCount(trackName, true));
                }
            }
            else
            {}
        }
        public async Task GetCoverForDisplay(int startIndex)
        {
            Load();
            for (int i = 0; i < trackList.CheckForTrackCount(startIndex); i++)
            {
                int byteCount = trackList.GetTrackByIndex(i+startIndex).TrackCoverBytes.Count;
                foreach (var byteChunk in trackList.GetSplittedDisplayCoverBytes(i+startIndex))
                {
                    await Clients.All.SendAsync("RecieveDisplayCoverBytes",byteChunk);
                }
                await Clients.All.SendAsync("RecieveDisplayCoverBytesDone", byteCount, i);
            }
        }

        public async Task GetNamesForDisplay(int startIndex)
        {
            Load();
            for (int i = 0; i < trackList.CheckForTrackCount(startIndex); i++)
            {
                await Clients.All.SendAsync("RecieveDisplayName", trackList.GetDisplayName(startIndex, i), i);
            }
        }

        public async Task GetArtistsForDisplay(int startIndex)
        {
            Load();
            for (int i = 0; i < trackList.CheckForTrackCount(startIndex); i++)
            {
                await Clients.All.SendAsync("RecieveDisplayArtist", trackList.GetDisplayArtist(startIndex, i), i);
            }
        }

        public async Task GetDurationForDisplay(int startIndex)
        {
            Load();
            for (int i = 0; i < trackList.CheckForTrackCount(startIndex); i++)
            {
                await Clients.All.SendAsync("RecieveDisplayDuration", trackList.GetDisplayDuration(startIndex, i), i);
            }
        }

        public async Task GetDatesForDisplay(int startIndex)
        {
            Load();
            for (int i = 0; i < trackList.CheckForTrackCount(startIndex); i++)
            {
                await Clients.All.SendAsync("RecieveDisplayDate", trackList.GetDisplayDate(startIndex, i), i);
            }
        }

        public async Task DeleteTrack(string trackName)
        {
            Load();
            await trackList.DeleteTrack(trackName);
            Save();
        }

    }
}
