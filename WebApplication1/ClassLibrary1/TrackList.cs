using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassLibrary1
{
    public class TrackList
    {
        private List<Track> tracks;
        private int trackListId;

        public TrackList()
        {
            Tracks = new List<Track>();
            TrackListId = new Random().Next(0,100);
        }
        public List<Track> Tracks { get => tracks; set => tracks = value; }
        public int TrackListId { get => trackListId; set => trackListId = value; }

        public bool CheckByName(string name)
        {
            bool trackFound = false;
            foreach (var track in Tracks)
            {
                if (track.TrackName == name)
                {
                    trackFound = true;
                }
            }
            return trackFound;
        }

        public void AddNewTrack(string name)
        {
            Tracks.Add(new Track(name));
        }

        public List<List<byte>> GetSplittedAudioBytes(string name)
        {
            foreach (var track in Tracks)
            {
                if (track.TrackName == name)
                {
                    return track.GetSplittedAudioBytes();
                }
            }
            return new List<List<byte>> { };
        }

        public List<List<byte>> GetSplittedCoverBytes(string name)
        {
            foreach (var track in Tracks)
            {
                if (track.TrackName == name)
                {
                    return track.GetSplittedCoverBytes();
                }
            }
            return new List<List<byte>> { };
        }


        public void AddBytesToExistingTrack(string name, List<byte> bytes, bool isAudio)
        {
            if (isAudio)
            {
                foreach (var track in Tracks)
                {
                    if (track.TrackName == name)
                    {
                        track.TrackAudioBytes.AddRange(bytes);
                    }
                }
            }
            else
            {
                foreach (var track in Tracks)
                {
                    if (track.TrackName == name)
                    {
                        track.TrackCoverBytes.AddRange(bytes);
                    }
                }
            }  
        }

        public int GetTrackByteCount(string trackName, bool isAudio)
        {
            if (isAudio)
            {
                foreach (var track in Tracks)
                {
                    if (track.TrackName == trackName)
                    {
                        return track.TrackAudioBytes.Count;
                    }
                }
            }
            else
            {
                foreach (var track in Tracks)
                {
                    if (track.TrackName == trackName)
                    {
                        return track.TrackCoverBytes.Count;
                    }
                }
            }
            
            return 0;
        }

        private static List<List<byte>> SplitList(List<byte> source, int size)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / size)
                .Select(g => g.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
