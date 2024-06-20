using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Text.Json;

namespace ClassLibrary1
{
    
    public  class FileManager
    {
        private string tempFolderPath = "F:\\TempFiles";
        public FileManager() {
            TrackList trackList = new TrackList();
        }

        public void SaveFile(TrackList trackList)
        {
            string jsonString = JsonSerializer.Serialize(trackList);
            File.WriteAllText(tempFolderPath + "\\tracklist.json", jsonString);
        }

        public TrackList LoadFile()
        {
            try
            {
                using (StreamReader r = new StreamReader(tempFolderPath + "\\tracklist.json"))
                {
                    string jsonFile = r.ReadToEnd();
                    return JsonSerializer.Deserialize<TrackList>(jsonFile);
                }
            }
            catch
            {
                return new TrackList();
            }
            
        }
    }
}
