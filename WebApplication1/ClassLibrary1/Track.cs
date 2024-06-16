namespace ClassLibrary1
{
    public class Track
    {
        private string trackName;
        private List<byte> trackCoverBytes;
        private List<byte> trackAudioBytes;

        public Track()
        {
            trackName = string.Empty;
            trackCoverBytes = new List<byte>();
            trackAudioBytes = new List<byte>();
        }
        public Track(string trackName)
        {
            TrackName = trackName;
            TrackCoverBytes = new List<byte>();
            TrackAudioBytes = new List<byte>();
        }
        public Track(string trackName, List<byte> trackCoverBytes, List<byte> trackAudioBytes)
        {
            TrackName = trackName;
            TrackCoverBytes = trackCoverBytes;
            TrackAudioBytes = trackAudioBytes;
        }

        public List<List<byte>> GetSplittedAudioBytes()
        {
            return SplitList(TrackAudioBytes, 5000);
        }

        public List<List<byte>> GetSplittedCoverBytes()
        {
            return SplitList(TrackCoverBytes, 5000);
        }

        private static List<List<byte>> SplitList(List<byte> source, int size)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / size)
                .Select(g => g.Select(v => v.Value).ToList())
                .ToList();
        }
        public string TrackName { get => trackName; set => trackName = value; }
        public List<byte> TrackCoverBytes { get => trackCoverBytes; set => trackCoverBytes = value; }
        public List<byte> TrackAudioBytes { get => trackAudioBytes; set => trackAudioBytes = value; }
    }

}
