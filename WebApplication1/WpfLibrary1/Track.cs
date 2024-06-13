namespace WpfLibrary1
{
    public class Track
    {
        private string trackName;
        private string trackArtist;
        private string trackCoverPath;
        private string trackAudioPath;

        public Track(string trackName, string trackArtist, string trackCoverPath, string trackAudioPath)
        {
            TrackName = trackName;
            TrackArtist = trackArtist;
            TrackCoverPath = trackCoverPath;
            TrackAudioPath = trackAudioPath;
        }

        public string TrackName { get => trackName; set => trackName = value; }
        public string TrackCoverPath { get => trackCoverPath; set => trackCoverPath = value; }
        public string TrackAudioPath { get => trackAudioPath; set => trackAudioPath = value; }
        public string TrackArtist { get => trackArtist; set => trackArtist = value; }
    }

}
