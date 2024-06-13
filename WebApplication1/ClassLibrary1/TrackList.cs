using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class TrackList
    {
        private List<Track> tracks;

        public TrackList()
        {
            Tracks = new List<Track>();
        }
        public List<Track> Tracks { get => tracks; set => tracks = value; }


        public bool CheckByName(string name)
        {
            int tempCounter = 0;
            foreach (var track in Tracks)
            {
                if (track.TrackName == name)
                {
                    tempCounter++;
                }
            }
            return tempCounter > 0;
        }

        public void AddNewTrack(string name)
        {
            Tracks.Add(new Track(name));
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
    }
}
