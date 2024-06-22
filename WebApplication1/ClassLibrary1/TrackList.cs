using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        public void AddNewTrack(string name, string artist)
        {
            Tracks.Add(new Track(name, artist));
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

        public List<List<byte>> GetSplittedDisplayCoverBytes(int startingIndex, int currentIndex)
        {
            return Tracks[currentIndex + startingIndex].GetSplittedCoverBytes();
        }

        public string GetDisplayName(int startingIndex, int currentIndex)
        {
            return Tracks[currentIndex + startingIndex].TrackName;
        }

        public string GetDisplayArtist(int startingIndex, int currentIndex)
        {
            return Tracks[currentIndex + startingIndex].TrackArtist;
        }

        public DateTime GetDisplayDate(int startingIndex, int currentIndex)
        {
            return Tracks[currentIndex + startingIndex].TrackAdded;
        }

        public Track GetTrackByIndex(int index)
        {
            return Tracks[index];
        }
        private static List<List<byte>> SplitList(List<byte> source, int size)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / size)
                .Select(g => g.Select(v => v.Value).ToList())
                .ToList();
        }
        public int CheckForTrackCount(int startingIndex)
        {
            if(tracks.Count == 0 + startingIndex)
            {
                return 0;
            }
            else if(tracks.Count == 1 + startingIndex)
            {
                return 1;
            }
            else if (tracks.Count == 2 + startingIndex)
            {
                return 2;
            }
            else if (tracks.Count == 3 + startingIndex)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
    }
}
