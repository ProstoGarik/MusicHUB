using NAudio.Wave;
using System.Diagnostics;
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
            string tempFilePath = "/tmp/temp_audio.mp3";
            File.WriteAllBytes(tempFilePath, TrackAudioBytes.ToArray());

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffprobe",
                    Arguments = $"-i \"{tempFilePath}\" -show_entries format=duration -v quiet -of csv=\"p=0\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            if (double.TryParse(output, out double seconds))
            {
                return TimeSpan.FromSeconds(seconds);
            }

            throw new Exception("Не удалось получить длительность MP3.");
        }

        public string TrackName { get => trackName; set => trackName = value; }
        public List<byte> TrackCoverBytes { get => trackCoverBytes; set => trackCoverBytes = value; }
        public List<byte> TrackAudioBytes { get => trackAudioBytes; set => trackAudioBytes = value; }
        public string TrackArtist { get => trackArtist; set => trackArtist = value; }
        public DateTime TrackAdded { get => trackAdded; set => trackAdded = value; }
    }
}
