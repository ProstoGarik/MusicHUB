using NAudio.Wave;
namespace HubClassLibrary
{
    public class Track
    {
        private string trackName;
        private string trackArtist;
        private DateTime trackAdded;
        private List<byte> trackCoverBytes;
        private List<byte> trackAudioBytes;

        public Track(string trackName, string trackArtist)
        {
            TrackName = trackName;
            TrackArtist = trackArtist;
            TrackCoverBytes = new List<byte>();
            TrackAudioBytes = new List<byte>();
            TrackAdded = DateTime.Now;
        }

        public List<List<byte>> GetSplittedAudioBytes()
        {
            return SplitList(TrackAudioBytes, 1000000);
        }

        public List<List<byte>> GetSplittedCoverBytes()
        {
            return SplitList(TrackCoverBytes, 1000000);
        }

        private static List<List<byte>> SplitList(List<byte> source, int size)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / size)
                .Select(g => g.Select(v => v.Value).ToList())
                .ToList();
        }

        public TimeSpan GetMp3Duration()
        {
            using (MemoryStream mp3Stream = new MemoryStream(TrackAudioBytes.ToArray(), writable: false))
            {
                using (Mp3FileReader mp3Reader = new Mp3FileReader(mp3Stream))
                {
                    return mp3Reader.TotalTime;
                }
            }
        }

        public string TrackName { get => trackName; set => trackName = value; }
        public List<byte> TrackCoverBytes { get => trackCoverBytes; set => trackCoverBytes = value; }
        public List<byte> TrackAudioBytes { get => trackAudioBytes; set => trackAudioBytes = value; }
        public string TrackArtist { get => trackArtist; set => trackArtist = value; }
        public DateTime TrackAdded { get => trackAdded; set => trackAdded = value; }
    }
}
