namespace ClassLibrary1
{
    public class Track
    {
        private string trackName;
        private List<byte> trackCoverBytes;
        private List<byte> trackAudioBytes;
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

        public string TrackName { get => trackName; set => trackName = value; }
        public List<byte> TrackCoverBytes { get => trackCoverBytes; set => trackCoverBytes = value; }
        public List<byte> TrackAudioBytes { get => trackAudioBytes; set => trackAudioBytes = value; }
    }

}
